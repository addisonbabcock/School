#region Using

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace Common.Misc
{
    // Rewrote whole class because I didn't like what it was doing
    public class IniFileLoaderBase
    {
        private readonly StreamReader file;

        //this is set to true if the file specified by the user is valid
        protected Dictionary<string, string> parameters = new Dictionary<string, string>();

        public IniFileLoaderBase(string filename)
        {
            file = new StreamReader(File.OpenRead(filename));


            if (file == null)
            {
                return;
            }

            string line = file.ReadLine();
            while (line != null)
            {
                line = line.Trim();
                if (!line.Equals(string.Empty) && !line.StartsWith("//"))
                {
                    string[] fields = Regex.Split(line, "\\s+");
                    string parameter = fields[0];
                    string value = fields[1];
                    if (value.EndsWith(";"))
                    {
                        value = value.Remove(value.Length - 1);
                    }
                    parameters.Add(parameter, value);
                }
                line = file.ReadLine();
            }
        }
    }
}