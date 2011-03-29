using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.Bots.GrahameBot
{
    public class Grahame_BotScriptor : Scriptor
    {
        private Grahame_BotScriptor()
            : base()
        {
            RunScriptFile(@"Bots\GrahameBot\GrahameBot.lua");
        }

        private static Grahame_BotScriptor instance;
        public static Grahame_BotScriptor Instance()
        {
            if(instance == null)
            {
                instance = new Grahame_BotScriptor();
            }
            return instance;
        }
    }
}