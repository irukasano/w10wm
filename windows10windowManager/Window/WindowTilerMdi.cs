using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using windows10windowManager.Util;
using windows10windowManagerUtil.Util;
using windows10windowManagerUtil.Window;

namespace windows10windowManager.Window
{
    public class WindowTilerMdi : AbstractWindowTiler
    {
        /**
         * <summary>
         * MDI形式のウィンドウのモニター横幅に対する割合
         * </summary>
         */
        public double percentOfWidthOfWindow;/* = 0.6; */

        /**
         * <summary>
         * MDI形式のウィンドウのモニター縦長に対する割合
         * </summary>
         */
        public double percentOfHeightOfWindow;/* = 0.6; */

        /**
         * <summary>
         * MDI形式のウィンドウの次の位置を求めるとき
         * 指定された分だけTop/Leftの位置をずらす
         * </summary>
         */
        public int shiftWidthToNextWindow;/* = 50; */
        public int shiftHeightToNextWindow;/* = 50; */

        /**
         * <summary>
         * MDI形式のウィンドウのモニター端までのマージン
         * </summary>
         */
        public int marginLeftToMonitor;/* = 20; */
        public int marginTopToMonitor;/* = 20; */
        public int marginRightToMonitor;/* = 20; */
        public int marginBottomToMonitor;/* = 20; */

        public WindowTilerMdi()
        {
            this.percentOfWidthOfWindow =
                SettingManager.GetDouble("Window_WindowTilerMdi_percentOfWidthOfWindow");
            this.percentOfHeightOfWindow =
                SettingManager.GetDouble("Window_WindowTilerMdi_percentOfHeightOfWindow");
            this.shiftWidthToNextWindow =
                SettingManager.GetInt("Window_WindowTilerMdi_shiftWidthToNextWindow");
            this.shiftHeightToNextWindow =
                SettingManager.GetInt("Window_WindowTilerMdi_shiftHeightToNextWindow");
            this.marginLeftToMonitor =
                SettingManager.GetInt("Window_WindowTilerMdi_marginLeftToMonitor");
            this.marginLeftToMonitor =
                SettingManager.GetInt("Window_WindowTilerMdi_marginTopToMonitor");
            this.marginRightToMonitor =
                SettingManager.GetInt("Window_WindowTilerMdi_marginRightToMonitor");
            this.marginBottomToMonitor =
                SettingManager.GetInt("Window_WindowTilerMdi_marginBottomToMonitor");
        }

        /**
         * <summary>
         * MDI形式に並べるためにウィンドウ位置を計算する
         * 左上から右下へ shiftWidht, shiftHeight 分ずつずらしながら求める
         * マージンをはみ出ないようにする。はみでる場合は
         * </summary>
         */
        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {
            var monitorWidth = monitorRight - monitorLeft;
            var monitorHeight = monitorBottom - monitorTop;
            var windowWidth = (int)Math.Floor(monitorWidth * this.percentOfWidthOfWindow);
            var windowHeight = (int)Math.Floor(monitorHeight * this.percentOfHeightOfWindow);

            var marginedMonitorTop = monitorTop + this.marginTopToMonitor;
            var marginedMonitorBottom = monitorBottom - this.marginBottomToMonitor;
            var marginedMonitorLeft = monitorLeft + this.marginLeftToMonitor;
            var marginedMonitorRight = monitorRight - this.marginRightToMonitor;
            WindowRect marginedMonitorRect = new WindowRect(
                marginedMonitorTop,
                marginedMonitorBottom,
                marginedMonitorLeft,
                marginedMonitorRight
            );

            var windowTop = marginedMonitorTop;
            var windowLeft = marginedMonitorLeft;
            var shiftWidthCount = 0;
            for (int i = 0; i < windowCount; i++)
            {
                var newWindowRect = new WindowRect(
                    /* top     = */ windowTop,
                    /* bottom  = */ windowTop + windowHeight,
                    /* left    = */ windowLeft,
                    /* right   = */ windowLeft + windowWidth
                );

                if (! this.IsWindowInMonitorRect(marginedMonitorRect, newWindowRect))
                {
                    // もしモニターからはみ出る場合は位置を調整して再計算する
                    // 縦にはみ出る場合は縦位置をトップに戻し、横位置は初期位置+shiftWidth*shiftWidthCount分ずらす
                    // 横にはみ出る場合は縦位置、横位置ともに初期位置に戻す
                    var windowRight = newWindowRect.right;
                    if ( marginedMonitorRect.bottom < newWindowRect.bottom)
                    {
                        shiftWidthCount ++;
                        windowTop = marginedMonitorTop;
                        windowLeft = marginedMonitorLeft + this.shiftWidthToNextWindow * shiftWidthCount;
                        windowRight = windowLeft + windowWidth;
                    }
                    if ( marginedMonitorRect.right < windowRight )
                    {
                        windowTop = marginedMonitorTop;
                        windowLeft = marginedMonitorLeft;
                        windowRight = windowLeft + windowWidth;
                    }

                    newWindowRect = new WindowRect(
                        /* top     = */ windowTop,
                        /* bottom  = */ windowTop + windowHeight,
                        /* left    = */ windowLeft,
                        /* right   = */ windowRight
                    );
                }

                Logger.WriteLine(newWindowRect.ToString());

                this.windowRects.Add(newWindowRect);
                windowTop += this.shiftHeightToNextWindow;
                windowLeft += this.shiftWidthToNextWindow;
            }

        }

        /**
         * <summary>
         * windowRect が monitorRect からはみ出ていなければ True 
         * </summary>
         */
        protected bool IsWindowInMonitorRect(WindowRect monitorRect, WindowRect windowRect)
        {
            return monitorRect.left <= windowRect.left &&
                monitorRect.top <= windowRect.top &&
                monitorRect.right >= windowRect.right &&
                monitorRect.bottom >= windowRect.bottom
            ;
        }

    }
}
