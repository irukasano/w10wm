using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace windows10windowManager.Util
{
    public static class SettingManager
    {
        public static string GetString(string key)
        {
            return (string)Properties.Settings.Default[key];
        }

        public static int GetInt(string key)
        {
            return (int)Properties.Settings.Default[key];
        }

        public static double GetDouble(string key)
        {
            return (double)Properties.Settings.Default[key];
        }

        public static void SaveString(string key, string value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }

        public static void SaveInt(string key, int value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }

        public static void SaveDouble(string key, double value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }

    }
}
