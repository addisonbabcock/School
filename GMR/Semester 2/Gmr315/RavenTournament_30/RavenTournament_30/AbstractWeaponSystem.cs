﻿using System;
using Common._2D;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.armory;
using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven
{
    public class AbstractWeaponSystem : IDisposable
    {
        protected  float m_dAimAccuracy;

        //the amount of time a bot will continue aiming at the position of the target
        //even if the target disappears from view.
        protected  float m_dAimPersistance;
        protected  float m_dReactionTime;
        protected  AbstractBot m_pOwner;

        //pointers to the weapons the bot is carrying (a bot may only carry one
        //instance of each weapon)
        protected  WeaponMap m_WeaponMap;

        //a pointer to the weapon the bot is currently holding
        protected Raven_Weapon m_pCurrentWeapon;

        public AbstractWeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance)
        {
            m_pOwner = owner;
            m_dReactionTime = ReactionTime;
            m_dAimAccuracy = AimAccuracy;
            m_dAimPersistance = AimPersistance;
            m_WeaponMap = new WeaponMap();
            Initialize();
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_WeaponMap.Clear();
        }

        #endregion

        //this is the minimum amount of time a bot needs to see an opponent before
        //it can react to it. This variable is used to prevent a bot shooting at
        //an opponent the instant it becomes visible.

        //predicts where the target will be by the time it takes the current weapon's
        //projectile type to reach it. Used by TakeAimAndShoot
        protected Vector2 PredictFuturePositionOfTarget()
        {
            float MaxSpeed = GetCurrentWeapon().GetMaxProjectileSpeed();

            //if the target is ahead and facing the agent shoot at its current pos
            Vector2 ToEnemy = m_pOwner.GetTargetBot().Pos() - m_pOwner.Pos();

            //the lookahead time is proportional to the distance between the enemy
            //and the pursuer; and is inversely proportional to the sum of the
            //agent's velocities
            float LookAheadTime = ToEnemy.Length()/
                                  (MaxSpeed + m_pOwner.GetTargetBot().MaxSpeed());

            //return the predicted future position of the enemy
            return m_pOwner.GetTargetBot().Pos() +
                   m_pOwner.GetTargetBot().Velocity()*LookAheadTime;
        }

        //adds a random deviation to the firing angle not greater than m_dAimAccuracy 
        //rads
        protected void AddNoiseToAim(ref Vector2 AimingPos)
        {
            Vector2 toPos = AimingPos - m_pOwner.Pos();

            toPos = Transformations.Vec2DRotateAroundOrigin(ref toPos, Utils.RandInRange(-m_dAimAccuracy, m_dAimAccuracy));

            AimingPos = toPos + m_pOwner.Pos();
        }


        //sets up the weapon map with just one weapon: the blaster
        public virtual void Initialize()
        {
            foreach (int key in m_WeaponMap.Keys)
            {
                if (m_WeaponMap[key] != null)
                {
                    m_WeaponMap[key].Dispose();
                }
            }

            m_WeaponMap.Clear();

            //set up the container
            m_pCurrentWeapon = new Blaster(m_pOwner);

            m_WeaponMap.Add((int) Raven_Objects.type_blaster, m_pCurrentWeapon);
            m_WeaponMap.Add((int) Raven_Objects.type_shotgun, null);
            m_WeaponMap.Add((int) Raven_Objects.type_rail_gun, null);
            m_WeaponMap.Add((int) Raven_Objects.type_rocket_launcher, null);
        }

        //this method aims the bot's current weapon at the target (if there is a
        //target) and, if aimed correctly, fires a round. (Called each update-step
        //from Raven_Bot::Update)
        public virtual void TakeAimAndShoot()
        {
            //aim the weapon only if the current target is shootable or if it has only
            //very recently gone out of view (this latter condition is to ensure the 
            //weapon is aimed at the target even if it temporarily dodges behind a wall
            //or other cover)
            if (m_pOwner.GetTargetSys().isTargetShootable() ||
                (m_pOwner.GetTargetSys().GetTimeTargetHasBeenOutOfView().TotalSeconds <
                 m_dAimPersistance))
            {
                //the position the weapon will be aimed at
                Vector2 AimingPos = m_pOwner.GetTargetBot().Pos();

                //if the current weapon is not an instant hit type gun the target position
                //must be adjusted to take into account the predicted movement of the 
                //target
                if (GetCurrentWeapon().GetWeaponType() == (int) Raven_Objects.type_rocket_launcher ||
                    GetCurrentWeapon().GetWeaponType() == (int) Raven_Objects.type_blaster)
                {
                    AimingPos = PredictFuturePositionOfTarget();

                    //if the weapon is aimed correctly, there is line of sight between the
                    //bot and the aiming position and it has been in view for a period longer
                    //than the bot's reaction time, shoot the weapon
                    if (m_pOwner.RotateFacingTowardPosition(AimingPos) &&
                        (m_pOwner.GetTargetSys().GetTimeTargetHasBeenVisible().TotalSeconds >
                         m_dReactionTime) &&
                        m_pOwner.hasLOSto(AimingPos))
                    {
                        AddNoiseToAim(ref AimingPos);

                        GetCurrentWeapon().ShootAt(AimingPos);
                    }
                }

                    //no need to predict movement, aim directly at target
                else
                {
                    //if the weapon is aimed correctly and it has been in view for a period
                    //longer than the bot's reaction time, shoot the weapon
                    if (m_pOwner.RotateFacingTowardPosition(AimingPos) &&
                        (m_pOwner.GetTargetSys().GetTimeTargetHasBeenVisible().TotalSeconds >
                         m_dReactionTime))
                    {
                        AddNoiseToAim(ref AimingPos);

                        GetCurrentWeapon().ShootAt(AimingPos);
                    }
                }
            }
  
                //no target to shoot at so rotate facing to be parallel with the bot's
                //heading direction
            else
            {
                m_pOwner.RotateFacingTowardPosition(m_pOwner.Pos() + m_pOwner.Heading());
            }
        }

        //this method determines the most appropriate weapon to use given the current
        //game state. (Called every n update-steps from Raven_Bot::Update)
        public virtual void SelectWeapon()
        {
            //if a target is present use fuzzy logic to determine the most desirable 
            //weapon.
            if (m_pOwner.GetTargetSys().isTargetPresent())
            {
                //calculate the distance to the target
                float DistToTarget = Vector2.Distance(m_pOwner.Pos(), m_pOwner.GetTargetSys().GetTarget().Pos());

                //for each weapon in the inventory calculate its desirability given the 
                //current situation. The most desirable weapon is selected
                float BestSoFar = float.MinValue;

                foreach (int key in m_WeaponMap.Keys)
                {
                    Raven_Weapon curWeap = m_WeaponMap[key];
                    //grab the desirability of this weapon (desirability is based upon
                    //distance to target and ammo remaining)
                    if (curWeap != null)
                    {
                        float score = curWeap.GetDesirability(DistToTarget);

                        //if it is the most desirable so far select it
                        if (score > BestSoFar)
                        {
                            BestSoFar = score;

                            //place the weapon in the bot's hand.
                            m_pCurrentWeapon = curWeap;
                        }
                    }
                }
            }

            else
            {
                m_pCurrentWeapon = m_WeaponMap[(int) Raven_Objects.type_blaster];
            }
        }

        //this will add a weapon of the specified type to the bot's inventory. 
        //If the bot already has a weapon of this type only the ammo is added. 
        //(called by the weapon giver-triggers to give a bot a weapon)
        public virtual  void AddWeapon(int weapon_type)
        {
            //create an instance of this weapon
            Raven_Weapon w = null;

            switch (weapon_type)
            {
                case (int) Raven_Objects.type_rail_gun:

                    w = new RailGun(m_pOwner);
                    break;

                case (int) Raven_Objects.type_shotgun:

                    w = new ShotGun(m_pOwner);
                    break;

                case (int) Raven_Objects.type_rocket_launcher:

                    w = new RocketLauncher(m_pOwner);
                    break;
            } //end switch


            //if the bot already holds a weapon of this type, just add its ammo
            Raven_Weapon present = GetWeaponFromInventory(weapon_type);

            if (present != null && w != null)
            {
                present.IncrementRounds(w.NumRoundsRemaining());

                w.Dispose();
            }
  
                //if not already holding, add to inventory
            else
            {
                m_WeaponMap[weapon_type] = w;
            }
        }

        //changes the current weapon to one of the specified type (provided that type
        //is in the bot's possession)
        public void ChangeWeapon(int type)
        {
            Raven_Weapon w = GetWeaponFromInventory(type);

            if (w != null) m_pCurrentWeapon = w;
        }

        //shoots the current weapon at the given position
        public void ShootAt(Vector2 pos)
        {
            GetCurrentWeapon().ShootAt(pos);
        }

        //returns a pointer to the current weapon
        public Raven_Weapon GetCurrentWeapon()
        {
            return m_pCurrentWeapon;
        }

        //returns a pointer to the specified weapon type (if in inventory, null if 
        //not)
        public Raven_Weapon GetWeaponFromInventory(int weapon_type)
        {
            return m_WeaponMap[weapon_type];
        }

        //returns the amount of ammo remaining for the specified weapon
        public int GetAmmoRemainingForWeapon(int weapon_type)
        {
            if (m_WeaponMap[weapon_type] != null)
            {
                return m_WeaponMap[weapon_type].NumRoundsRemaining();
            }

            return 0;
        }

        public float ReactionTime()
        {
            return m_dReactionTime;
        }

        public void RenderCurrentWeapon(PrimitiveBatch batch)
        {
            GetCurrentWeapon().Render(batch);
        }

        public void RenderDesirabilities(SpriteBatch batch, SpriteFont font, Color color)
        {
            Vector2 p = m_pOwner.Pos();

            int num = 0;

            foreach (int key in m_WeaponMap.Keys)
            {
                if (m_WeaponMap[key] != null)
                {
                    num++;
                }
            }

            int offset = 15*num;
            foreach (int key in m_WeaponMap.Keys)
            {
                Raven_Weapon curWeap = m_WeaponMap[key];
                if (curWeap != null)
                {
                    float score = curWeap.GetLastDesirabilityScore();
                    string type = Raven_ObjectEnumerations.GetNameOfType(curWeap.GetWeaponType());
                    batch.DrawString(font, score + " " + type, new Vector2(p.X + 10, p.Y - offset), color);
                }
            }
        }
    }
}