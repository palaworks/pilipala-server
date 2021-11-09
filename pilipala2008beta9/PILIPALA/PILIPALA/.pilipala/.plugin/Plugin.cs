using System.IO;
using System.Collections.Generic;


namespace PILIPALA.pilipala.plugin
{
    using WaterLibrary.pilipala.Component;
    using Newtonsoft.Json.Linq;


    public class PluginInstance
    {
        private Pluginer pluginer;
        private string pluginUUID;

        internal PluginInstance(string pluginUUID, Pluginer pluginer)
        {
            this.pluginUUID = pluginUUID;
            this.pluginer = pluginer;
        }
        public object Invoke(string methodName, params object[] args)
        {
            return pluginer.Invoke(pluginUUID, methodName, args);
        }
    }

    public class PluginManager
    {
        private Pluginer pluginer;
        private Dictionary<string, string> nameUUIDpool = new();

        public Dictionary<string, PluginInstance> PluginInstancePool = new();

        public PluginManager(ComponentFactory compoFty)
        {
            pluginer = compoFty.GenPluginer();

            string path = "./.pilipala/.plugin";
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(path + "/config.json", System.Text.Encoding.UTF8);
            }
            catch//找不到配置文件则创建
            {
                var fileStream = new FileStream(path + "/config.json", FileMode.Create, FileAccess.Write);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("{\"plugins\":{}}");
                streamWriter.Close();
                fileStream.Close();

                jsonString = File.ReadAllText(path + "/config.json", System.Text.Encoding.UTF8);
            }
            var jObject = JObject.Parse(jsonString);

            foreach (var el in jObject["plugins"])
            {
                var pluginName = ((JProperty)el).Name;
                var pluginPath = path + el.First["path"];
                var pluginJson = path + el.First["json"];

                var pluginUUID = pluginer.LoadPlugin(pluginPath, pluginName, pluginJson);
                nameUUIDpool.Add(pluginName, pluginUUID);
                PluginInstancePool[pluginName] = new PluginInstance(nameUUIDpool[pluginName], pluginer);
            }//初始化缓存
        }
    }
}
