using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven.Bots.TravisBot
{
    public class Travis_WeaponSystem : AbstractWeaponSystem
    {
        public Travis_WeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance) : base(owner, ReactionTime, AimAccuracy, AimPersistance)
        {
        }
    }
}