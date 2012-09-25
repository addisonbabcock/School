using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Script;

namespace Raven.lua
{
    public class Raven_Scriptor : Scriptor
    {
        private Raven_Scriptor() : base()
        {
            RunScriptFile("Params.lua");
        }

        private static Raven_Scriptor instance;
        public static Raven_Scriptor Instance()
        {
            if(instance == null)
            {
                instance = new Raven_Scriptor();
            }
            return instance;
        }
    }
}
