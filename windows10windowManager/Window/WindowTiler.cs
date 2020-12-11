using System;
using System.Collections.Generic;
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
        protected AbstractWindowTiler windowTiler;

        public WindowTiler(WindowTilingType windowTilingType, int windowCount, Monitor.RECT monitorRect)
        {
            Logger.WriteLine($"WindowTiler(WindowTilingType={windowTilingType}, int={windowCount}");

            switch ( windowTilingType)
            {
                case WindowTilingType.None:
                    this.windowTiler = new WindowTilerNone();
                    break;
                case WindowTilingType.Bugn:
                    this.windowTiler = new WindowTilerDividerBugn();
                    break;
                case WindowTilingType.FourDivided:
                    this.windowTiler = new WindowTilerDivider();
                    var wt = (WindowTilerDivider)this.windowTiler;
                    wt.columnCount = 2;
                    wt.rowCount = 2;
                    break;
                case WindowTilingType.Maximize:
                    this.windowTiler = new WindowTilerMaximize();
                    break;
                case WindowTilingType.Mdi:
                    this.windowTiler = new WindowTilerMdi();
                    break;
                case WindowTilingType.Concentration:
                    this.windowTiler = new WindowTilerConcentration();
                    break;
            }

            this.windowTiler.CalcuratePosition(windowCount, 
                monitorRect.top, monitorRect.bottom, monitorRect.left, monitorRect.right);
        }

        public WindowRect GetWindowRectOf(int windowIndex)
        {
            return this.windowTiler.GetWindowRectOf(windowIndex);
        }
    }
}
