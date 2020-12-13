using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace windows10windowManager.Util
{
    public static class SettingManager
    {
        public static Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static string GetString(string key)
        {
            return (string)ConfigurationManager.AppSettings[key];
        }

        public static int GetInt(string key)
        {
            return int.Parse(ConfigurationManager.AppSettings[key]);
        }

        public static double GetDouble(string key)
        {
            return double.Parse(ConfigurationManager.AppSettings[key]);
        }

        public static void SaveString(string key, string value)
        {
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();
        }

        public static void SaveInt(string key, int value)
        {
            configuration.AppSettings.Settings[key].Value = value.ToString();
            configuration.Save();
        }

        public static void SaveDouble(string key, double value)
        {
            configuration.AppSettings.Settings[key].Value = value.ToString();
            configuration.Save();
        }

    }
}
