using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.CraigBot
{
    public class Craig_BotScriptor : Scriptor
    {
        private Craig_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\CraigBot\CraigBot.lua");
        }

        private static Craig_BotScriptor instance;
        public static Craig_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Craig_BotScriptor();
            }
            return instance;
        }
    }
}