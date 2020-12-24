using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.KeyHook.KeyMap;

namespace windows10windowManager.KeyHook
{
    public class InterceptKeyboardSettingManager
    {
        public static List<InterceptKeyboardSetting> GetSettings()
        {
            int[] modifierLWindows = new int[] { (int)OriginalKey.LeftWindows };
            int[] modifierLShiftLWindows = new int[] { (int)OriginalKey.LeftWindows, (int)OriginalKey.LeftShift };
            int[] modifierRShiftLWindows = new int[] { (int)OriginalKey.LeftWindows, (int)OriginalKey.LeftShift };

            return new List<InterceptKeyboardSetting>
            {
                new InterceptKeyboardSetting("MoveCurrentFocusNext",            true,   OriginalKey.J,          modifierLWindows),
                new InterceptKeyboardSetting("MoveCurrentFocusPrevious",        true,   OriginalKey.K,          modifierLWindows),
                new InterceptKeyboardSetting("MoveCurrentFocusTop",             true,   OriginalKey.U,          modifierLWindows),
                new InterceptKeyboardSetting("MoveCurrentFocusBottom",          true,   OriginalKey.M,          modifierLWindows),
                new InterceptKeyboardSetting("SetWindowNext",                   true,   OriginalKey.J,          modifierLShiftLWindows),
                new InterceptKeyboardSetting("SetWindowNext",                   true,   OriginalKey.J,          modifierRShiftLWindows),
                new InterceptKeyboardSetting("SetWindowPrevious",               true,   OriginalKey.K,          modifierLShiftLWindows),
                new InterceptKeyboardSetting("SetWindowPrevious",               true,   OriginalKey.K,          modifierRShiftLWindows),
                new InterceptKeyboardSetting("SetWindowTop",                    true,   OriginalKey.U,          modifierLShiftLWindows),
                new InterceptKeyboardSetting("SetWindowTop",                    true,   OriginalKey.U,          modifierRShiftLWindows),
                new InterceptKeyboardSetting("SetWindowBottom",                 true,   OriginalKey.M,          modifierLShiftLWindows),
                new InterceptKeyboardSetting("SetWindowBottom",                 true,   OriginalKey.M,          modifierRShiftLWindows),
                new InterceptKeyboardSetting("HighlightActiveMonitor",          true,   OriginalKey.C,          modifierLWindows),
                new InterceptKeyboardSetting("MoveCurrentFocusPreviousMonitor", true,   OriginalKey.Period,     modifierLWindows),
                new InterceptKeyboardSetting("MoveCurrentFocusNextMonitor",     true,   OriginalKey.Comma,      modifierLWindows),
                new InterceptKeyboardSetting("ActivateMonitor1",                true,   OriginalKey.F1,         modifierLWindows),
                new InterceptKeyboardSetting("ActivateMonitor2",                true,   OriginalKey.F2,         modifierLWindows),
                new InterceptKeyboardSetting("ActivateMonitor3",                true,   OriginalKey.F3,         modifierLWindows),
                new InterceptKeyboardSetting("ShowHelpForm",                    false,  OriginalKey.Question,   modifierLWindows),
                new InterceptKeyboardSetting("ShowContextMenu",                 false,  OriginalKey.O,          modifierLWindows)
            };
        }
    }
}
