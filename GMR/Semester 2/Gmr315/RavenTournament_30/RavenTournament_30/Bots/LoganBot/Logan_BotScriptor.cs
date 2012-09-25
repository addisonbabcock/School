using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.LoganBot
{
    public class Logan_BotScriptor : Scriptor
    {
        private Logan_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\LoganBot\LoganBot.lua");
        }

        private static Logan_BotScriptor instance;
        public static Logan_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Logan_BotScriptor();
            }
            return instance;
        }
    }
}
