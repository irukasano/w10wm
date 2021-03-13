using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace windows10windowManagerUtil.Util
{
    public static class SettingManager
    {
        public static Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static string GetConfigrationFileName()
        {
            return configuration.FilePath;
        }

        public static string GetString(string key)
        {
            return (string)ConfigurationManager.AppSettings[key];
        }

        public static int GetInt(string key)
        {
            return int.Parse(SettingManager.GetString(key));
        }

        public static double GetDouble(string key)
        {
            return double.Parse(SettingManager.GetString(key));
        }

        /**
         * key = example_key の場合
         * example_key_0 から連番でキーが存在する限り取得し List<string> で戻す
         */
        public static List<string> GetStringList(string key)
        {
            List<string> settingList = new List<string>();
            var indexOfKey = 0;
            while (true)
            {
                var indexedKey = $"{key}_{indexOfKey}";
                if ( ! SettingManager.ContainsKey(indexedKey))
                {
                    break;
                }
                settingList.Add(SettingManager.GetString(indexedKey));
                indexOfKey++;
            }
            return settingList;
        }

        public static void SaveString(string key, string value)
        {
            if (SettingManager.ContainsKey(key))
            {
                configuration.AppSettings.Settings[key].Value = value;
            } else
            {
                configuration.AppSettings.Settings.Add(key, value);
            }
            configuration.Save();
        }

        public static void SaveInt(string key, int value)
        {
            SettingManager.SaveString(key, value.ToString());
        }

        public static void SaveDouble(string key, double value)
        {
            SettingManager.SaveString(key, value.ToString());
        }

        public static void SaveStringList(string key, List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                var indexedKey = $"{key}_{i}";
                SettingManager.SaveString(indexedKey, values[i]);
            }
        }

        public static bool ContainsKey(string key)
        {
            return ConfigurationManager.AppSettings.AllKeys.Contains(key);
        }


    }
}
