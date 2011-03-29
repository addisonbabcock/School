using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Fuzzy;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.armory
{
    public class ShotGun : Raven_Weapon
    {
        //how much shot the each shell contains
        private int m_iNumBallsInShell;

        //how much the shot spreads out when a cartridge is discharged
        private float m_dSpread;

        protected override void InitializeFuzzyModule()
        {
            m_FuzzyModule = new FuzzyModule();
            FuzzyVariable  DistanceToTarget = m_FuzzyModule.CreateFLV("DistanceToTarget");

            FuzzyTerm  Target_Close = DistanceToTarget.AddLeftShoulderSet("Target_Close", 0, 25, 150);
            FuzzyTerm Target_Medium = DistanceToTarget.AddTriangularSet("Target_Medium", 25, 150, 300);
            FuzzyTerm Target_Far = DistanceToTarget.AddRightShoulderSet("Target_Far", 150, 300, 1000);

            FuzzyVariable  Desirability = m_FuzzyModule.CreateFLV("Desirability");

            FuzzyTerm VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);
            FuzzyTerm Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FuzzyTerm Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);

            FuzzyVariable  AmmoStatus = m_FuzzyModule.CreateFLV("AmmoStatus");
            FuzzyTerm Ammo_Loads = AmmoStatus.AddRightShoulderSet("Ammo_Loads", 30, 60, 100);
            FuzzyTerm Ammo_Okay = AmmoStatus.AddTriangularSet("Ammo_Okay", 0, 30, 60);
            FuzzyTerm Ammo_Low = AmmoStatus.AddTriangularSet("Ammo_Low", 0, 0, 30);

            FuzzyTerm targetClose_AmmoLoads = new FzAND(ref Target_Close, ref Ammo_Loads);
            FuzzyTerm targetClose_AmmoOkay = new FzAND(ref Target_Close, ref Ammo_Okay);
            FuzzyTerm targetClose_AmmoLow = new FzAND(ref Target_Close, ref Ammo_Low);

            FuzzyTerm targetMedium_AmmoLoads = new FzAND(ref Target_Medium, ref Ammo_Loads);
            FuzzyTerm targetMedium_AmmoOkay = new FzAND(ref Target_Medium, ref Ammo_Okay);
            FuzzyTerm targetMedium_AmmoLow = new FzAND(ref Target_Medium, ref Ammo_Low);

            FuzzyTerm targetFar_AmmoLoads = new FzAND(ref Target_Far, ref Ammo_Loads);
            FuzzyTerm targetFar_AmmoOkay = new FzAND(ref Target_Far, ref Ammo_Okay);
            FuzzyTerm targetFar_AmmoLow = new FzAND(ref Target_Far, ref Ammo_Low);
            m_FuzzyModule.AddRule(ref targetClose_AmmoLoads, ref VeryDesirable);
            m_FuzzyModule.AddRule(ref targetClose_AmmoOkay, ref VeryDesirable);
            m_FuzzyModule.AddRule(ref targetClose_AmmoLow, ref VeryDesirable);

            m_FuzzyModule.AddRule(ref targetMedium_AmmoLoads, ref VeryDesirable);
            m_FuzzyModule.AddRule(ref targetMedium_AmmoOkay, ref Desirable);
            m_FuzzyModule.AddRule(ref targetMedium_AmmoLow, ref Undesirable);

            m_FuzzyModule.AddRule(ref targetFar_AmmoLoads, ref Desirable);
            m_FuzzyModule.AddRule(ref targetFar_AmmoOkay, ref Undesirable);
            m_FuzzyModule.AddRule(ref targetFar_AmmoLow, ref Undesirable);
        }



        public ShotGun(AbstractBot owner):

                      base((int) Raven_Objects.type_shotgun,
                                   script.GetInt("ShotGun_DefaultRounds"),
                                   script.GetInt("ShotGun_MaxRoundsCarried"),
                                   script.GetDouble("ShotGun_FiringFreq"),
                                   script.GetDouble("ShotGun_IdealRange"),
                                   script.GetDouble("Pellet_MaxSpeed"),
                                   owner)
{
            m_iNumBallsInShell = script.GetInt("ShotGun_NumBallsInShell");
            m_dSpread = script.GetDouble("ShotGun_Spread");

            m_vecWeaponVB = new List<Vector2>();
            m_vecWeaponVB.Add(new Vector2(0, 0));
            m_vecWeaponVB.Add(new Vector2(0, -2));
            m_vecWeaponVB.Add(new Vector2(10, -2));
            m_vecWeaponVB.Add(new Vector2(10, 0));
            m_vecWeaponVB.Add(new Vector2(0, 0));
            m_vecWeaponVB.Add(new Vector2(0, 2));
            m_vecWeaponVB.Add(new Vector2(10, 2));
            m_vecWeaponVB.Add(new Vector2(10, 0));
                                           

 

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
      Drawing.DrawClosedShaped(batch, m_vecWeaponVBTrans, Color.Brown);
  }

  public override void ShootAt(Vector2 pos)
  {
      if (NumRoundsRemaining() > 0 && isReadyForNextShot())
      {
          //a shotgun cartridge contains lots of tiny metal balls called pellets. 
          //Therefore, every time the shotgun is discharged we have to calculate
          //the spread of the pellets and add one for each trajectory
          for (int b = 0; b < m_iNumBallsInShell; ++b)
          {
              //determine deviation from target using a bell curve type distribution
              float deviation = Utils.RandInRange(0, m_dSpread) + Utils.RandInRange(0, m_dSpread) - m_dSpread;

              Vector2 AdjustedTarget = pos - m_pOwner.Pos();

              //rotate the target vector by the deviation
AdjustedTarget = Transformations.Vec2DRotateAroundOrigin(ref AdjustedTarget, deviation);

              //add a pellet to the game world
              m_pOwner.GetWorld().AddShotGunPellet(m_pOwner, AdjustedTarget + m_pOwner.Pos());

          }

          m_iNumRoundsLeft--;

          UpdateTimeWeaponIsNextAvailable();

          //add a trigger to the game so that the other bots can hear this shot
          //(provided they are within range)
          m_pOwner.GetWorld().GetMap().AddSoundTrigger(m_pOwner, script.GetDouble("ShotGun_SoundRange"));
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
    m_FuzzyModule.Fuzzify("AmmoStatus", m_iNumRoundsLeft);

    m_dLastDesirabilityScore = m_FuzzyModule.DeFuzzify("Desirability", Common.Fuzzy.FuzzyModule.DefuzzifyMethod.max_av);
  }

  return m_dLastDesirabilityScore;
}
    }
}
