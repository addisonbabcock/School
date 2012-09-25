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
    public class RocketLauncher : Raven_Weapon
    {
        protected override void InitializeFuzzyModule()
        {
            m_FuzzyModule = new FuzzyModule();
            FuzzyVariable  DistToTarget = m_FuzzyModule.CreateFLV("DistToTarget");

            FuzzyTerm Target_Close = DistToTarget.AddLeftShoulderSet("Target_Close", 0, 25, 150);
            FuzzyTerm Target_Medium = DistToTarget.AddTriangularSet("Target_Medium", 25, 150, 300);
            FuzzyTerm Target_Far = DistToTarget.AddRightShoulderSet("Target_Far", 150, 300, 1000);

            FuzzyVariable Desirability = m_FuzzyModule.CreateFLV("Desirability");
            FuzzyTerm VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);
            FuzzyTerm Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FuzzyTerm Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);

            FuzzyVariable AmmoStatus = m_FuzzyModule.CreateFLV("AmmoStatus");
            FuzzyTerm Ammo_Loads = AmmoStatus.AddRightShoulderSet("Ammo_Loads", 10, 30, 100);
            FuzzyTerm Ammo_Okay = AmmoStatus.AddTriangularSet("Ammo_Okay", 0, 10, 30);
            FuzzyTerm Ammo_Low = AmmoStatus.AddTriangularSet("Ammo_Low", 0, 0, 10);

            FuzzyTerm targetClose_AmmoLoads = new FzAND(ref Target_Close, ref Ammo_Loads);
            FuzzyTerm targetClose_AmmoOkay = new FzAND(ref Target_Close, ref Ammo_Okay);
            FuzzyTerm targetClose_AmmoLow = new FzAND(ref Target_Close, ref Ammo_Low);

            FuzzyTerm targetMedium_AmmoLoads = new FzAND(ref Target_Medium, ref Ammo_Loads);
            FuzzyTerm targetMedium_AmmoOkay = new FzAND(ref Target_Medium, ref Ammo_Okay);
            FuzzyTerm targetMedium_AmmoLow = new FzAND(ref Target_Medium, ref Ammo_Low);

            FuzzyTerm targetFar_AmmoLoads = new FzAND(ref Target_Far, ref Ammo_Loads);
            FuzzyTerm targetFar_AmmoOkay = new FzAND(ref Target_Far, ref Ammo_Okay);
            FuzzyTerm targetFar_AmmoLow = new FzAND(ref Target_Far, ref Ammo_Low);

            m_FuzzyModule.AddRule(ref targetClose_AmmoLoads, ref Undesirable);
            m_FuzzyModule.AddRule(ref targetClose_AmmoOkay, ref Undesirable);
            m_FuzzyModule.AddRule(ref targetClose_AmmoLow, ref Undesirable);

            m_FuzzyModule.AddRule(ref targetMedium_AmmoLoads, ref VeryDesirable);
            m_FuzzyModule.AddRule(ref targetMedium_AmmoOkay, ref VeryDesirable);
            m_FuzzyModule.AddRule(ref targetMedium_AmmoLow, ref Desirable);

            m_FuzzyModule.AddRule(ref targetFar_AmmoLoads, ref Desirable);
            m_FuzzyModule.AddRule(ref targetFar_AmmoOkay, ref Undesirable);
            m_FuzzyModule.AddRule(ref targetFar_AmmoLow, ref Undesirable);
        }



        public RocketLauncher(AbstractBot owner):

                      base((int) Raven_Objects.type_rocket_launcher,
                                   script.GetInt("RocketLauncher_DefaultRounds"),
                                   script.GetInt("RocketLauncher_MaxRoundsCarried"),
                                   script.GetDouble("RocketLauncher_FiringFreq"),
                                   script.GetDouble("RocketLauncher_IdealRange"),
                                   script.GetDouble("Rocket_MaxSpeed"),
                                   owner)
{
            m_vecWeaponVB = new List<Vector2>();
            m_vecWeaponVB.Add(new Vector2(0, -3));
            m_vecWeaponVB.Add(new Vector2(6, -3));
            m_vecWeaponVB.Add(new Vector2(6, -1));
            m_vecWeaponVB.Add(new Vector2(15, -1));
            m_vecWeaponVB.Add(new Vector2(15, 1));
            m_vecWeaponVB.Add(new Vector2(6, 1));
            m_vecWeaponVB.Add(new Vector2(6, 3));
            m_vecWeaponVB.Add(new Vector2(0, 3));
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
      Drawing.DrawClosedShaped(batch, m_vecWeaponVBTrans, Color.Red);
  }

  public override void ShootAt(Vector2 pos)
  {
      if (NumRoundsRemaining() > 0 && isReadyForNextShot())
      {
          //fire off a rocket!
          m_pOwner.GetWorld().AddRocket(m_pOwner, pos);

          m_iNumRoundsLeft--;

          UpdateTimeWeaponIsNextAvailable();

          //add a trigger to the game so that the other bots can hear this shot
          //(provided they are within range)
          m_pOwner.GetWorld().GetMap().AddSoundTrigger(m_pOwner, script.GetDouble("RocketLauncher_SoundRange"));
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
    m_FuzzyModule.Fuzzify("DistToTarget", DistToTarget);
    m_FuzzyModule.Fuzzify("AmmoStatus", (float)m_iNumRoundsLeft);

    m_dLastDesirabilityScore = m_FuzzyModule.DeFuzzify("Desirability", Common.Fuzzy.FuzzyModule.DefuzzifyMethod.max_av);
  }

  return m_dLastDesirabilityScore;
}
    }
}
