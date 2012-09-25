using WeaponMap = System.Collections.Generic.Dictionary<int, Raven.armory.Raven_Weapon>;

namespace Raven.Bots.LoganBot
{
    public class Logan_WeaponSystem : AbstractWeaponSystem
    {
        public Logan_WeaponSystem(AbstractBot owner,
                                  float ReactionTime,
                                  float AimAccuracy,
                                  float AimPersistance) : base(owner, ReactionTime, AimAccuracy, AimPersistance)
        {
        }
    }
}