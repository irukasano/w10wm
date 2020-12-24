using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.KeyHook.KeyMap;

namespace windows10windowManager.KeyHook
{
    public class InterceptKeyboardSetting
    {
        #region Field
        /**
         * <summary>
         * 補足するキー
         * </summary>
         */
        public OriginalKey key;

        /**
         * <summary>
         * 補足する修飾キー
         * </summary>
         */
        public int[] modifiers;

        /**
         * <summary>
         * 補足したときに処理するメソッド
         * </summary>
         */
        public string methodName;

        /**
         * <summary>
         * 処理するメソッドが非同期処理が必要かどうか
         * </summary>
         */
        public bool needsAsync;
        #endregion

        public InterceptKeyboardSetting(string methodName, bool needsAsync, OriginalKey key, int[] modifiers)
        {
            this.methodName = methodName;
            this.needsAsync = needsAsync;
            this.key = key;
            this.modifiers = modifiers;
        }
    }
}
