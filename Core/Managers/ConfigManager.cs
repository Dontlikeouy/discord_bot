using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

//TOKEN и prefix
namespace discord_bot.Core.Managers
{
    public static class ConfigManager
    {
        private static string ConfigFolder = "Resources";
        private static string ConfigFile = "config.json";
        private static string ConfigPath = ConfigFolder + "/" + ConfigFile;
        public static BotConfig Config { get; private set; }
        static ConfigManager()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigPath))
            {
                Config = new BotConfig();
                var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigPath);
                Config = JsonConvert.DeserializeObject<BotConfig>(json);
            }

        }
    }
    public class BotConfig
    {
        private string token = "";

        [JsonProperty("token")]
        public string Token { get { return token; } private set { token = value; } }
        private string prefix = "!";

        [JsonProperty("prefix")]
        public string Prefix { get { return prefix; } private set { prefix = value; } }
    }
}