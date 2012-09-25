using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.AddisonBot
{
    public class Addison_BotScriptor : Scriptor
    {
        private Addison_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\AddisonBot\AddisonBot.lua");
        }

        private static Addison_BotScriptor instance;
        public static Addison_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Addison_BotScriptor();
            }
            return instance;
        }
    }
}