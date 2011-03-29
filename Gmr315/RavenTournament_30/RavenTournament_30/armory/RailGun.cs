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
   public class RailGun: Raven_Weapon
{


       protected override void InitializeFuzzyModule()
       {
           m_FuzzyModule = new FuzzyModule();
           FuzzyVariable DistanceToTarget = m_FuzzyModule.CreateFLV("DistanceToTarget");

           FuzzyTerm Target_Close = DistanceToTarget.AddLeftShoulderSet("Target_Close", 0, 25, 150);
           FuzzyTerm Target_Medium = DistanceToTarget.AddTriangularSet("Target_Medium", 25, 150, 300);
           FuzzyTerm Target_Far = DistanceToTarget.AddRightShoulderSet("Target_Far", 150, 300, 1000);

           FuzzyVariable Desirability = m_FuzzyModule.CreateFLV("Desirability");

           FuzzyTerm VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);
           FuzzyTerm Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
           FuzzyTerm Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);

           FuzzyVariable AmmoStatus = m_FuzzyModule.CreateFLV("AmmoStatus");
           FuzzyTerm Ammo_Loads = AmmoStatus.AddRightShoulderSet("Ammo_Loads", 15, 30, 100);
           FuzzyTerm Ammo_Okay = AmmoStatus.AddTriangularSet("Ammo_Okay", 0, 15, 30);
           FuzzyTerm Ammo_Low = AmmoStatus.AddTriangularSet("Ammo_Low", 0, 0, 15);


           FuzzyTerm targetClose_AmmoLoads = new FzAND(ref Target_Close, ref Ammo_Loads);
           FuzzyTerm targetClose_AmmoOkay = new FzAND(ref Target_Close, ref Ammo_Okay);
           FuzzyTerm targetClose_AmmoLow = new FzAND(ref Target_Close, ref Ammo_Low);

           FuzzyTerm desirable = new FzFairly((FzSet) Desirable);
           FuzzyTerm veryDesirable = new FzVery((FzSet) VeryDesirable);
           FuzzyTerm ammoLow = new FzFairly((FzSet) Ammo_Low);

           FuzzyTerm targetMedium_AmmoLoads = new FzAND(ref Target_Medium, ref Ammo_Loads);
           FuzzyTerm targetMedium_AmmoOkay = new FzAND(ref Target_Medium, ref Ammo_Okay);
           FuzzyTerm targetMedium_AmmoLow = new FzAND(ref Target_Medium, ref Ammo_Low);

           FuzzyTerm targetFar_AmmoLoads = new FzAND(ref Target_Far, ref Ammo_Loads);
           FuzzyTerm targetFar_AmmoOkay = new FzAND(ref Target_Far, ref Ammo_Okay);
           FuzzyTerm targetFar_AmmoLow = new FzAND(ref Target_Far, ref ammoLow);

           m_FuzzyModule.AddRule(ref targetClose_AmmoLoads, ref desirable);
           m_FuzzyModule.AddRule(ref targetClose_AmmoOkay, ref desirable);
           m_FuzzyModule.AddRule(ref targetClose_AmmoLow, ref Undesirable);

           m_FuzzyModule.AddRule(ref targetMedium_AmmoLoads, ref VeryDesirable);
           m_FuzzyModule.AddRule(ref targetMedium_AmmoOkay, ref Desirable);
           m_FuzzyModule.AddRule(ref targetMedium_AmmoLow, ref Desirable);

           m_FuzzyModule.AddRule(ref targetFar_AmmoLoads, ref veryDesirable);
           m_FuzzyModule.AddRule(ref targetFar_AmmoOkay, ref veryDesirable);
           m_FuzzyModule.AddRule(ref targetFar_AmmoLow, ref VeryDesirable);
       }

private static List<Vector2> m_vecWeaponVB;

public RailGun(AbstractBot owner):

                      base((int) Raven_Objects.type_rail_gun,
                                   script.GetInt("RailGun_DefaultRounds"),
                                   script.GetInt("RailGun_MaxRoundsCarried"),
                                   script.GetDouble("RailGun_FiringFreq"),
                                   script.GetDouble("RailGun_IdealRange"),
                                   script.GetDouble("Slug_MaxSpeed"),
                                   owner)
{
    if (m_vecWeaponVB == null)
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

    Drawing.DrawClosedShaped(batch, m_vecWeaponVBTrans, Color.Blue);
}

public override void ShootAt(Vector2 pos)
{
    if (NumRoundsRemaining() > 0 && isReadyForNextShot())
    {
        //fire a round
        m_pOwner.GetWorld().AddRailGunSlug(m_pOwner, pos);

        UpdateTimeWeaponIsNextAvailable();

        m_iNumRoundsLeft--;

        //add a trigger to the game so that the other bots can hear this shot
        //(provided they are within range)
        m_pOwner.GetWorld().GetMap().AddSoundTrigger(m_pOwner, script.GetDouble("RailGun_SoundRange"));
    }
}

public override float GetDesirability(float DistToTarget){
  if (m_iNumRoundsLeft == 0)
  {
    m_dLastDesirabilityScore = 0;
  }
  else
  {
    //fuzzify distance and amount of ammo
    m_FuzzyModule.Fuzzify("DistanceToTarget", DistToTarget);
    m_FuzzyModule.Fuzzify("AmmoStatus",(float)m_iNumRoundsLeft);

    m_dLastDesirabilityScore = m_FuzzyModule.DeFuzzify("Desirability", Common.Fuzzy.FuzzyModule.DefuzzifyMethod.max_av);
  }

  return m_dLastDesirabilityScore;
}
}
}
