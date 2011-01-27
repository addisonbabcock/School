#region Using

using System;

#endregion

namespace WestWorldWithWoman
{
    public class EntityNames
    {
        public static string GetNameOfEntity(EntityName n)
        {
            switch (n)
            {
                case EntityName.ent_Miner_Bob:

                    return "Miner Bob";

                case EntityName.ent_Elsa:

                    return "Elsa";

                default:

                    return "UNKNOWN!";
            }
        }

        public static string GetNameOfEntity(int n)
        {
            EntityName name = (EntityName) Enum.Parse(typeof (EntityName), n.ToString());
            return GetNameOfEntity(name);
        }
    }
}