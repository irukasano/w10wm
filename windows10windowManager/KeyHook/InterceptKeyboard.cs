using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

using windows10windowManager.KeyHook.KeyMap;
using windows10windowManager.Util;

namespace windows10windowManager.KeyHook
{
    class InterceptKeyboard : AbstractInterceptKeyboard
    {
        #region OriginalKeyEventArg
        public class OriginalKeyEventArg : EventArgs
        {
            public int KeyCode { get; }
            public int[] Modifiers { get; }

            public OriginalKeyEventArg(int keyCode, List<int> modifiers)
            {
                this.KeyCode = keyCode;
                this.Modifiers = modifiers.ToArray();
            }

            /**
             * <summary>
             * 指定されたキーコード、修飾キーが押されたかを判別する
             * </summary>
             */
            public bool Equals(OriginalKey key, int[] modifiers)
            {
                var originalKey = KeyMapConverter.KeyCodeToKey(this.KeyCode);
                return (key == originalKey && this.EqualsModifiers(modifiers));
            }

            public bool EqualsModifiers(int[] modifiers)
            {
                if (modifiers.Length != Modifiers.Length)
                {
                    return false;
                }
                for (int i = 0; i < modifiers.Length; i++)
                {
                    if (!this.Modifiers.Contains(modifiers[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            /**
             * <summary>
             * 現在のキーイベントを文字列として戻す
             * </summary>
             */
            override public string ToString()
            {
                var key = KeyMapConverter.KeyCodeToKey(this.KeyCode);
                var modifiers = "";
                if ( this.Modifiers.Length == 0)
                {
                    return $"{key}";
                }

                for (int i = 0; i < this.Modifiers.Length; i++)
                {
                    var mk = KeyMapConverter.KeyCodeToKey(this.Modifiers[i]);
                    modifiers += mk + " ";
                }

                return $"{modifiers}+ {key}";
            }

        }
        #endregion

        #region Delegate
        public delegate void KeyEventHandler(object sender, OriginalKeyEventArg e);
        public event KeyEventHandler KeyDownEvent;
        public event KeyEventHandler KeyUpEvent;
        #endregion

        #region Fields
        public bool callNextHook { get; set; } = true;

        protected List<int> Modifiers { get; set; } = new List<int>();
        #endregion

        protected void OnKeyDownEvent(int keyCode)
        {
            KeyDownEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, Modifiers));
        }
        protected void OnKeyUpEvent(int keyCode)
        {
            KeyUpEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, Modifiers));
        }

        public override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var eventKeyDown = (wParam == (IntPtr)WM_KEYDOWN) ? "KeyDown " : "";
            var eventSysKeyDown = (wParam == (IntPtr)WM_SYSKEYDOWN) ? "SysKeyDown " : "";
            var eventKeyUp = (wParam == (IntPtr)WM_KEYUP) ? "KeyUp " : "";
            var eventSysKeyUp = (wParam == (IntPtr)WM_SYSKEYUP) ? "SysKeyUp " : "";
            var wkb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            var wvkCode = (int)wkb.vkCode;
            var vk = KeyMapConverter.KeyCodeToKey(wvkCode);

            Logger.WriteLine($"{nCode} : {vk} : {eventKeyDown}{eventSysKeyDown}{eventKeyUp}{eventSysKeyUp}");

            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                if (this.isModifier(vkCode))
                {
                    if (! this.Modifiers.Contains(vkCode))
                    {
                        this.Modifiers.Add(vkCode);
                    }
                    else
                    {
                        // KeyDown 時にすでに同じ Modifier が存在していた場合
                        // ある Modifier を二重に押していることになる
                        // なにか問題が発生しているときなので、いったん削除する
                        this.Modifiers.Remove(vkCode);
                    }
                }
                else
                {
                    this.OnKeyDownEvent(vkCode);
                    //Modifiers.Clear();
                }
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                if (this.isModifier(vkCode))
                {
                    if (this.Modifiers.Contains(vkCode))
                    {
                        this.Modifiers.Remove(vkCode);
                    }
                    //Modifiers.Clear();
                }
                else
                {
                    this.OnKeyUpEvent(vkCode);
                }
            }

            if (!this.callNextHook)
            {
                return new IntPtr(1);
            }

            return base.HookProcedure(nCode, wParam, lParam);
        }

        protected bool isModifier(int keyCode)
        {
            var key = KeyMapConverter.KeyCodeToKey(keyCode);
            return (key == OriginalKey.LeftWindows ||
                key == OriginalKey.RightWindows ||
                key == OriginalKey.LeftCtrl ||
                key == OriginalKey.RightCtrl ||
                key == OriginalKey.LeftShift ||
                key == OriginalKey.RightShift ||
                key == OriginalKey.LeftAlt ||
                key == OriginalKey.RightAlt);
        }

        public override void UnHook()
        {
            this.Modifiers.Clear();
            base.UnHook();
        }

    }
}



