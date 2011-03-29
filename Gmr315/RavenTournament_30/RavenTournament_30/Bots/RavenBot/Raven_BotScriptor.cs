using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.RavenBot
{
    public class Raven_BotScriptor : Scriptor
    {
        private Raven_BotScriptor() : base()
        {
            RunScriptFile(@"Bots\RavenBot\RavenBot.lua");
        }

        private static Raven_BotScriptor instance;
        public static Raven_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Raven_BotScriptor();
            }
            return instance;
        }
    }
}