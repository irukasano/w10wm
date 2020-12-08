using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManager.Window
{
    /**
     * <summary>
     * ウィンドウを整列する種類を定義
     * </summary>
     */
    public enum WindowTilingType : ushort
    {
        Divider = 1,
        FourDivided = 2,
        Maximize = 4,
        Mdi = 8 
    }

    public class WindowTiler
    {
        protected AbstractWindowTiler windowTiler;

        public WindowTiler(WindowTilingType windowTilingType, int windowCount, RECT monitorRect)
        {
            switch ( windowTilingType)
            {
                case WindowTilingType.Divider:
                    this.windowTiler = new WindowTilerDivider();
                    break;
                case WindowTilingType.FourDivided:
                    this.windowTiler = new WindowTilerFourDivided();
                    break;
                case WindowTilingType.Maximize:
                    this.windowTiler = new WindowTilerMaximize();
                    break;
                case WindowTilingType.Mdi:
                    this.windowTiler = new WindowTilerMdi();
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
