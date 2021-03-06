﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

using keyHook.KeyMap;

namespace keyHook
{
    class InterceptKeyboard : AbstractInterceptKeyboard
    {
        #region InputEvent
        public class OriginalKeyEventArg : EventArgs
        {
            public int KeyCode { get; }
            public int[] Modifiers { get; }

            public OriginalKeyEventArg(int keyCode, List<int> modifiers)
            {
                KeyCode = keyCode;
                Modifiers = modifiers.ToArray();
            }

            public bool equalsModifiers(int[] modifiers)
            {
                if (modifiers.Length != Modifiers.Length)
                {
                    return false;
                }
                for (int i = 0; i < modifiers.Length; i++)
                {
                    if (! Modifiers.Contains(modifiers[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

        }
        public delegate void KeyEventHandler(object sender, OriginalKeyEventArg e);
        public event KeyEventHandler KeyDownEvent;
        public event KeyEventHandler KeyUpEvent;

        public bool callNextHook { get; set; } = true;

        protected List<int> Modifiers { get; set; } = new List<int>();

        protected void OnKeyDownEvent(int keyCode)
        {
            KeyDownEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, Modifiers));
        }
        protected void OnKeyUpEvent(int keyCode)
        {
            KeyUpEvent?.Invoke(this, new OriginalKeyEventArg(keyCode, Modifiers));
        }
        #endregion

        public override IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                if (isModifier(vkCode))
                {
                    if (!Modifiers.Contains(vkCode))
                    {
                        Modifiers.Add(vkCode);
                    } else
                    {
                        // KeyDown 時にすでに同じ Modifier が存在していた場合
                        // ある Modifier を二重に押していることになる
                        // なにか問題が発生しているときなので、いったん削除する
                        Modifiers.Remove(vkCode);
                    }
                }
                else
                {
                    OnKeyDownEvent(vkCode);
                }
            }
            else if (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
            {
                var kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                var vkCode = (int)kb.vkCode;
                if (isModifier(vkCode))
                {
                    if (Modifiers.Contains(vkCode))
                    {
                        Modifiers.Remove(vkCode);
                    }
                }
                else
                {
                    OnKeyUpEvent(vkCode);
                }
            }

            if (!callNextHook)
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
    }

}


