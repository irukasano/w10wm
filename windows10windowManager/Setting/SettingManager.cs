using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManager.Setting
{
    public class SettingManager
    {

        public static Dictionary<string, string> MainSetting { get; set; }

        static SettingManager()
        {
            MainSetting = new Dictionary<string, string>()
            {
                { "", "" }
            };
        }


        /**
         * <summary>
         * 設定情報を文字列で取得する
         * section には "Main" を指定する
         * </summary>
         */
        public static string GetString(string section, string key, string defaultValue = "")
        {
            Type typeOfSetting = typeof(SettingManager);
            PropertyInfo propertyOfSection = typeOfSetting.GetProperty(section+"Setting");
            if ( propertyOfSection == null)
            {
                return defaultValue;
            }
            var dictionayOfSection = 
                (Dictionary<string, string>)propertyOfSection.GetValue(typeOfSetting, null);
            if (! dictionayOfSection.ContainsKey(key))
            {
                return defaultValue;
            }
            return dictionayOfSection[key];
        }

        /**
         * <summary>
         * 設定情報を数値型で取得する
         * </summary>
         */
        public static int GetInt(string section, string key, int defaultValue = 0)
        {
            string value = GetString(section, key, null);
            if ( value == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt32(value);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
            catch (OverflowException)
            {
                return defaultValue;
            }
        }
    }
}
