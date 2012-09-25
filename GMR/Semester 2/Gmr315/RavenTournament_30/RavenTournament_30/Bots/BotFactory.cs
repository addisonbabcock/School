using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;

namespace Raven.Bots
{
    public class BotFactory
    {
         #region Singleton pattern

        private static BotFactory instance;
        public static BotFactory Instance()
        {
            if(instance == null)
            {
                instance = new BotFactory();
            }
            return instance;
        }
        #endregion

        private Dictionary<string, Type> teams;

        private static Assembly callingAssembly;
        private BotFactory()
        {
            teams = new Dictionary<string, Type>();
            Initialize();
        }

        private void Initialize()
        {
            // Get the assembly that contains this code

            callingAssembly = Assembly.GetCallingAssembly();

            // Get a list of all the types in the assembly

            Type[] allTypes = callingAssembly.GetTypes();
            foreach (Type type in allTypes)
            {
                // Only scan classes that arn't abstract

                if (type.IsClass && !type.IsAbstract)
                {
                    // If a class implements the IFactoryProduct interface,

                    // which allows retrieval of the product class key...

                    Type iBotMaker = type.GetInterface("IBot");
                    if (iBotMaker != null)
                    {
                        teams.Add(type.Name, type);
                    }
                }
            }
        }

        public AbstractBot CreateTeam(string className, Raven_Game world, Vector2 position)
        {
            object[] args = {world, position};
            Type bot = teams[className];
            object botInstance = callingAssembly.CreateInstance(bot.FullName, false,
                                                                       BindingFlags.CreateInstance, null, args, null,
                                                                       null);
            return (AbstractBot) botInstance;
        }
    }
}
