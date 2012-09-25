using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven.Bots.RyanBot
{
    public class Ryan_WeaponSystem : AbstractWeaponSystem
    {
        public Ryan_WeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance) : base(owner, ReactionTime, AimAccuracy, AimPersistance)
        {
        }
    }
}