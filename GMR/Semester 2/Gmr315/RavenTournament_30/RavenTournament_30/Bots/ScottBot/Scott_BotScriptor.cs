using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.ScottBot
{
    public class Scott_BotScriptor : Scriptor
    {
        private Scott_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\ScottBot\ScottBot.lua");
        }

        private static Scott_BotScriptor instance;
        public static Scott_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Scott_BotScriptor();
            }
            return instance;
        }
    }
}
