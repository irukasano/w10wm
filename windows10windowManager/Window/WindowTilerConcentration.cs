using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    public class WindowTilerConcentration : AbstractWindowTiler
    {
        /**
         * <summary>
         * 右下ウィンドウの幅を画面幅に対する％で指定する
         * </summary>
         */
        public double percentOfWidthOfSubWindow;/* = 0.45; */

        /**
         * <summary>
         * 右下ウィンドウの高さを画面高さに対する％で指定する
         * </summary>
         */
        public double percentOfHeightOfSubWindow;/* = 0.85; */

        /**
         * <summary>
         * 右下ウィンドウの下マージン
         * </summary>
         */
        public int marginRightOfSubWindow;/* = 50; */

        /**
         * <summary>
         * 右下ウィンドウの右マージン
         * </summary>
         */
        public int marginBottomOfSubWindow;/* = 50; */

        public WindowTilerConcentration()
        {
            this.percentOfWidthOfSubWindow =
                SettingManager.GetDouble("Window_WindowTilerConcentration_percentOfWidthOfSubWindow");
            this.percentOfHeightOfSubWindow =
                SettingManager.GetDouble("Window_WindowTilerConcentration_percentOfHeightOfSubWindow");
            this.marginRightOfSubWindow =
                SettingManager.GetInt("Window_WindowTilerConcentration_marginRightOfSubWindow");
            this.marginBottomOfSubWindow =
                SettingManager.GetInt("Window_WindowTilerConcentration_marginBottomOfSubWindow");
        }

        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {
            // 主ウィンドウは最大化
            this.windowRects.Add(new WindowRect(
                /* top     = */ monitorTop,
                /* bottom  = */ monitorBottom,
                /* left    = */ monitorLeft,
                /* right   = */ monitorRight
            ));

            // ２番目以降のウィンドウはすべてサブウィンドウ
            var monitorWidth = monitorRight - monitorLeft;
            var monitorHeight = monitorBottom - monitorTop;
            var windowWidth = (int)Math.Floor(monitorWidth * this.percentOfWidthOfSubWindow);
            var windowHeight = (int)Math.Floor(monitorHeight * this.percentOfHeightOfSubWindow);

            var subwindowTop = monitorBottom - this.marginBottomOfSubWindow - windowHeight;
            var subwindowBottom = monitorBottom - this.marginBottomOfSubWindow;
            var subwindowLeft = monitorRight - this.marginRightOfSubWindow - windowWidth;
            var subwindowRight = monitorRight - this.marginRightOfSubWindow;
 
            this.windowRects.Add(new WindowRect(
                /* top     = */ subwindowTop,
                /* bottom  = */ subwindowBottom,
                /* left    = */ subwindowLeft,
                /* right   = */ subwindowRight
            ));

        }

        public override int PushNewWindowInfo(List<WindowInfoWithHandle> windowInfos, WindowInfoWithHandle windowInfoWithHandle)
        {
            // 集中モードの場合は、新規ウィンドウは先頭ではない（先頭は集中対象のウィンドウだから）
            // なのでその次の位置に新規ウィンドウを挿入する
            int targetIndex = 0;
            if ( windowInfos.Count > 0)
            {
                targetIndex = 1;
            }
            windowInfos.Insert(targetIndex, windowInfoWithHandle);
            return targetIndex;
        }

    }
}
