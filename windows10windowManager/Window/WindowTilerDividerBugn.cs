using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    public class WindowTilerDividerBugn : AbstractWindowTiler
    {
        /**
         * <summary>
         * 左側列の横幅％
         * </summary>
         */
        public double percentOfLeftColumn = 0.65;

        /**
         * <summary>
         * 右側列に縦に並べる最大ウィンドウ数
         * </summary>
         */
        public int maxCountOfWindowOfRightColumn = 6;

        /**
         * <summary>
         * 左は１ウィンドウ、右は複数ウィンドウ
         * </summary>
         */
        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {
            if ( windowCount == 0)
            {
                return;
            }
            
            // 定義すべきウィンドウ位置の最大数は、
            // 右側の複数ウィンドウ＋１（左側）だが
            // 右側はウィンドウ数によって可変
            var countOfWindowOfRightColumn = this.maxCountOfWindowOfRightColumn;
            if ( windowCount -1 < countOfWindowOfRightColumn)
            {
                countOfWindowOfRightColumn = windowCount - 1;
            }

            // 左側列の定義
            var monitorWidth = monitorRight - monitorLeft;
            var monitorHeight = monitorBottom - monitorTop;
            var windowWidthOfLeftColumn = (int)Math.Floor(monitorWidth * this.percentOfLeftColumn);
            this.windowRects.Add(new WindowRect(
                /* top     = */ monitorTop,
                /* bottom  = */ monitorBottom,
                /* left    = */ monitorLeft,
                /* right   = */ monitorLeft + windowWidthOfLeftColumn
            ));

            if ( countOfWindowOfRightColumn == 0 )
            {
                // 右側列にウィンドウがなければここで計算終了
                return;
            }

            // 右側列の定義
            var windowWidthOfRightColumn = monitorWidth - windowWidthOfLeftColumn;
            var windowHeightOfRightColumn = monitorHeight / countOfWindowOfRightColumn;
            var windowTop = monitorTop;
            var windowLeft = monitorLeft + windowWidthOfLeftColumn;
            var windowRight = windowLeft + windowWidthOfRightColumn;
            for (int i = 0; i < countOfWindowOfRightColumn; i++)
            {
                var windowBottom = windowTop + windowHeightOfRightColumn;
                var newWindowRect = new WindowRect(
                    /* top     = */ windowTop,
                    /* bottom  = */ windowBottom,
                    /* left    = */ windowLeft,
                    /* right   = */ windowRight
                );

                Logger.WriteLine(newWindowRect.ToString());

                this.windowRects.Add(newWindowRect);

                windowTop = windowBottom;
            }

        }
    }
}
