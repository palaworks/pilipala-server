using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using WaterLibrary.Util;

namespace PILIPALA.system
{
    public class ThemeReader
    {
        public static string GetConfigString(string Path)
        {
            return System.IO.File.ReadAllText(Path);
        }
        public static JObject GetConfigJson(string Path)
        {
            return JObject.Parse(GetConfigString(Path));
        }
    }
}
