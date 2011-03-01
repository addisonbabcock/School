using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Misc
{
    public static class Debug
    {
        public static bool debug = false;
        public static void WriteLine(string line)
        {
#if DEBUG
            if(debug)
            Console.WriteLine(line);
#endif
        }
    }
}
