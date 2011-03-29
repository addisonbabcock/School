using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven.Bots.AddisonBot
{
    public class Addison_WeaponSystem : AbstractWeaponSystem
    {
        public Addison_WeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance) : base(owner, ReactionTime, AimAccuracy, AimPersistance)
        {
        }
    }
}