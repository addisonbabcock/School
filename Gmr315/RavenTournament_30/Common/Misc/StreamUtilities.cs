using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Misc
{
    public static class StreamUtilities
    {
        public static int GetIntFromStream(StreamReader stream)
        {
            string line = stream.ReadLine();
            return int.Parse(line);
        }
    }
}
