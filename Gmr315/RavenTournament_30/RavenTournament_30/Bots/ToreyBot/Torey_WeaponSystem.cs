﻿using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven.Bots.ToreyBot
{
    public class Torey_WeaponSystem : AbstractWeaponSystem
    {
        public Torey_WeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance) : base(owner, ReactionTime, AimAccuracy, AimPersistance)
        {
        }
    }
}