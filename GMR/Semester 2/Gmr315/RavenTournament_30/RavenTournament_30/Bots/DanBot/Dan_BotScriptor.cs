using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.DanBot
{
    public class Dan_BotScriptor : Scriptor
    {
        private Dan_BotScriptor() : base()
        {
            RunScriptFile(@"Bots\DanBot\DanBot.lua");
        }

        private static Dan_BotScriptor instance;
        public static Dan_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Dan_BotScriptor();
            }
            return instance;
        }
    }
}
