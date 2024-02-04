using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace WoWTools.WDBUpdater
{
    public static class SettingsManager
    {
        public static string connectionString = "";
        static SettingsManager()
        {
            LoadSettings();
        }

        public static void LoadSettings()
        {
            var config = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).AddJsonFile("config.json", optional: false, reloadOnChange: false).Build();
            connectionString = config.GetSection("config")["connectionstring"];
        }

    }
}
