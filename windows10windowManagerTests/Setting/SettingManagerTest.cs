using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using windows10windowManager.Setting;

namespace windows10windowManagerTests.Setting
{
    [TestClass]
    public class SettingManagerTest
    {
        [TestMethod]
        public void Test_GetStringNotExistSection()
        {
            var value = SettingManager.GetString("NotExistsSection", "NotExistKey", "DefaultValue");
            Assert.AreEqual("DefaultValue", value);
        }

        [TestMethod]
        public void Test_GetStringNotExistKey()
        {
            var value = SettingManager.GetString("Main", "NotExistKey", "DefaultValue");
            Assert.AreEqual("DefaultValue", value);
        }

        [TestMethod]
        public void Test_GetString()
        {
            SettingManager.MainSetting.Clear();
            SettingManager.MainSetting.Add("TestKey1", "TestValue1");
            SettingManager.MainSetting.Add("TestKey2", "TestValue2");
            var value = SettingManager.GetString("Main", "TestKey2", "DefaultValue");
            Assert.AreEqual("TestValue2", value);
        }

        [TestMethod]
        public void Test_GetInt()
        {
            SettingManager.MainSetting.Clear();
            SettingManager.MainSetting.Add("TestKey1", "TestValue1");
            SettingManager.MainSetting.Add("TestKey2", "10");
            var value = SettingManager.GetInt("Main", "TestKey2", 200);
            Assert.AreEqual(10, value);
        }

        [TestMethod]
        public void Test_GetIntCantParse()
        {
            SettingManager.MainSetting.Clear();
            SettingManager.MainSetting.Add("TestKey1", "TestValue1");
            SettingManager.MainSetting.Add("TestKey2", "10x");
            var value = SettingManager.GetInt("Main", "TestKey2", 200);
            Assert.AreEqual(200, value);
        }
    }
}
