using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams
{
    public class SoccerTeamFactory
    {

        #region Singleton pattern

        private static SoccerTeamFactory instance;
        public static SoccerTeamFactory Instance()
        {
            if(instance == null)
            {
                instance = new SoccerTeamFactory();
            }
            return instance;
        }
        #endregion

        private Dictionary<string, Type> teams;

        private static Assembly callingAssembly;
        private SoccerTeamFactory()
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

                    Type iTeamMaker = type.GetInterface("ITeam");
                    if (iTeamMaker != null)
                    {
                        teams.Add(type.Name, type);
                    }
                }
            }
        }

        public AbstractSoccerTeam CreateTeam(string className, Game game, Goal        home_goal,
             Goal        opponents_goal,
             SoccerPitch pitch,
			 AbstractSoccerTeam.team_color   color)
        {
            object[] args = {game, home_goal, opponents_goal, pitch, color};
            Type soccerTeam = teams[className];
            object soccerTeamInstance = callingAssembly.CreateInstance(soccerTeam.FullName, false,
                                                                       BindingFlags.CreateInstance, null, args, null,
                                                                       null);
            return (AbstractSoccerTeam) soccerTeamInstance;
        }

    }
}
