using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    /**
     * <summary>
     * ウィンドウを整列する種類を定義
     * </summary>
     */
    public enum WindowTilingType : ushort
    {
        None = 0,
        Bugn = 1,
        FourDivided = 2,
        Maximize = 4,
        Mdi = 8,
        Concentration = 16
    }

    public class WindowTiler
    {
        #region Field
        protected AbstractWindowTiler windowTiler;

        protected static Dictionary<int, string> WindowTilerClass = new Dictionary<int, string>()
        {
            { (int)WindowTilingType.None,  "WindowTilerNone"},
            { (int)WindowTilingType.Bugn,  "WindowTilerBugn"},
            { (int)WindowTilingType.FourDivided,  "WindowTilerDividerFourDivided"},
            { (int)WindowTilingType.Maximize,  "WindowTilerMaximize"},
            { (int)WindowTilingType.Mdi,  "WindowTilerMdi"},
            { (int)WindowTilingType.Concentration,  "WindowTilerConcentration"}
        };
        #endregion


        public WindowTiler(WindowTilingType windowTilingType, int windowCount, Monitor.RECT monitorRect)
        {
            Logger.WriteLine($"WindowTiler(WindowTilingType={windowTilingType}, int={windowCount}");

            this.windowTiler = WindowTiler.CreateWindowTilerInstance(windowTilingType);

            this.windowTiler.CalcuratePosition(windowCount, 
                monitorRect.top, monitorRect.bottom, monitorRect.left, monitorRect.right);
        }

        public WindowRect GetWindowRectOf(int windowIndex)
        {
            return this.windowTiler.GetWindowRectOf(windowIndex);
        }

        public static int PushNewWindowInfo(WindowTilingType windowTilingType, 
            List<WindowInfoWithHandle> windowInfos, WindowInfoWithHandle windowInfoWithHandle)
        {
            var windowTiler = WindowTiler.CreateWindowTilerInstance(windowTilingType);
            return windowTiler.PushNewWindowInfo(windowInfos, windowInfoWithHandle);
        }

        /**
         * <summary>
         * 指定された windowTilingType に紐付く WindowTiler クラスインスタンスを作成する
         * </summary>
         */
        public static AbstractWindowTiler CreateWindowTilerInstance( WindowTilingType windowTilingType)
        {
            // 指定された windowTilingType に紐付く WindowTiler クラス名を取得し
            // そのインスタンスによって位置情報を計算する
            var classNameOfWindowTiler = WindowTilerClass[(int)windowTilingType];
            Type typeOfWindowTiler = Type.GetType("windows10windowManager.Window." + classNameOfWindowTiler);
            return (AbstractWindowTiler)Activator.CreateInstance(typeOfWindowTiler);
        }

    }
}
