using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Fuzzy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.armory
{
    public class Blaster: Raven_Weapon
{
        protected override void InitializeFuzzyModule()
        {
            m_FuzzyModule = new FuzzyModule();
            FuzzyVariable  DistToTarget = m_FuzzyModule.CreateFLV("DistToTarget");

            FzSet  Target_Close = DistToTarget.AddLeftShoulderSet("Target_Close", 0, 25, 150);
            FzSet  Target_Medium = DistToTarget.AddTriangularSet("Target_Medium", 25, 150, 300);
            FzSet  Target_Far = DistToTarget.AddRightShoulderSet("Target_Far", 150, 300, 1000);

            FuzzyVariable  Desirability = m_FuzzyModule.CreateFLV("Desirability");
            FzSet  VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);
            FzSet  Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FzSet  Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);

            FuzzyTerm tc = (FuzzyTerm) Target_Close;
            FuzzyTerm tm = (FuzzyTerm)Target_Medium;
            FuzzyTerm tf = (FuzzyTerm)Target_Far;
            FuzzyTerm desirable = (FuzzyTerm) Desirable;
            FuzzyTerm undesirable = (FuzzyTerm) new FzVery(Undesirable);

            m_FuzzyModule.AddRule(ref tc, ref desirable);
            m_FuzzyModule.AddRule(ref tm, ref undesirable);
            m_FuzzyModule.AddRule(ref tf, ref undesirable);
        }

public Blaster(AbstractBot owner): base((int) Raven_Objects.type_blaster,
                                   script.GetInt("Blaster_DefaultRounds"),
                                   script.GetInt("Blaster_MaxRoundsCarried"),
                                   script.GetDouble("Blaster_FiringFreq"),
                                   script.GetDouble("Blaster_IdealRange"),
                                   script.GetDouble("Bolt_MaxSpeed"),
                                   owner)
{
    if(m_vecWeaponVB == null)
    {
        m_vecWeaponVB = new List<Vector2>();
        m_vecWeaponVB.Add(new Vector2(0, -1));
        m_vecWeaponVB.Add(new Vector2(10, -1));
        m_vecWeaponVB.Add(new Vector2(10, 1));
        m_vecWeaponVB.Add(new Vector2(0, 1));
    }

  
  //setup the fuzzy module
  InitializeFuzzyModule();
}


public override void Render(PrimitiveBatch batch)
{
    Vector2 ownerPosition = m_pOwner.Pos();
    Vector2 ownerFacing = m_pOwner.Facing();
    Vector2 ownerFacingPerp = m_pOwner.Facing().Perp();
    Vector2 ownerScale = m_pOwner.Scale();
    m_vecWeaponVBTrans = Transformations.WorldTransform(m_vecWeaponVB,
                                    ref ownerPosition,
                                    ref ownerFacing,
                                    ref ownerFacingPerp,
                                    ref ownerScale);
    Drawing.DrawClosedShaped(batch, m_vecWeaponVBTrans, Color.Green);
}

public override void ShootAt(Vector2 pos)
{
    if (isReadyForNextShot())
    {
        //fire!
        m_pOwner.GetWorld().AddBolt(m_pOwner, pos);

        UpdateTimeWeaponIsNextAvailable();

        //add a trigger to the game so that the other bots can hear this shot
        //(provided they are within range)
        m_pOwner.GetWorld().GetMap().AddSoundTrigger(m_pOwner, script.GetDouble("Blaster_SoundRange"));
    }
}

public override float GetDesirability(float DistToTarget){
  //fuzzify distance and amount of ammo
  m_FuzzyModule.Fuzzify("DistToTarget", DistToTarget);

  m_dLastDesirabilityScore = m_FuzzyModule.DeFuzzify("Desirability", Common.Fuzzy.FuzzyModule.DefuzzifyMethod.max_av);

  return m_dLastDesirabilityScore;
}
}
}
