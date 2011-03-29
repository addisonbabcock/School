using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.TravisBot
{
    public class Travis_BotScriptor : Scriptor
    {
        private Travis_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\TravisBot\TravisBot.lua");
        }

        private static Travis_BotScriptor instance;
        public static Travis_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Travis_BotScriptor();
            }
            return instance;
        }
    }
}