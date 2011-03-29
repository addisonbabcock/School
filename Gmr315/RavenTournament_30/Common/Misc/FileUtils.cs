using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class FileUtils
    {
        public static string GetTemporaryFile(string extn)
        {
            string response = string.Empty;

            if (!extn.StartsWith("."))
                extn = "." + extn;

            response = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + extn;

            return response;
        }
    }
}
