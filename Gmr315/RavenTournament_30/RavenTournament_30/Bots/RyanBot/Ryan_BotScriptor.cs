using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.RyanBot
{
    public class Ryan_BotScriptor : Scriptor
    {
        private Ryan_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\RyanBot\RyanBot.lua");
        }

        private static Ryan_BotScriptor instance;
        public static Ryan_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Ryan_BotScriptor();
            }
            return instance;
        }
    }
}