using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.ToreyBot
{
    public class Torey_BotScriptor : Scriptor
    {
        private Torey_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\ToreyBot\ToreyBot.lua");
        }

        private static Torey_BotScriptor instance;
        public static Torey_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Torey_BotScriptor();
            }
            return instance;
        }
    }
}
