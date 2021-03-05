using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManagerUtil.Window;

namespace windows10windowManager.Window
{
    public class WindowTilerDivider : AbstractWindowTiler
    {
        /**
         * <summary>
         * 縦に何列作成するかを指定する
         * </summary>
         */
        public int columnCount;

        /**
         * <summary>
         * 横に何列作成するかを指定する
         * </summary>
         */
        public int rowCount;


        /**
         * <summary>
         * 横 columnCount, 縦 rowCount に分割したウィンドウサイズを計算する
         * 左上、その下、右上、その下、の順に位置を計算し、this.windowRects に格納する
         * columnCount * rowCount を超えた数の windowCount の場合、超えた分の位置はすべて左下のそれと同じにする
         * </summary>
         */
        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {
            var monitorWidth = monitorRight - monitorLeft;
            var monitorHeight = monitorBottom - monitorTop;
            var windowWidth = monitorWidth / this.columnCount;
            var windowHeight = monitorHeight / this.rowCount;

            var currentWindowIndex = 0;
            this.windowRects.Clear();
            for (int icolumn = 0; icolumn < this.columnCount; icolumn++)
            {
                for ( int irow = 0; irow < this.rowCount; irow++)
                {
                    if ( windowCount <= currentWindowIndex)
                    {
                        break;
                    }

                    var windowLeft = icolumn * windowWidth;
                    var windowTop = irow * windowHeight;
                    this.windowRects.Add(new WindowRect(
                        /* top     = */ windowTop,
                        /* bottom  = */ windowTop + windowHeight,
                        /* left    = */ windowLeft,
                        /* right   = */ windowLeft + windowWidth
                    ));

                    currentWindowIndex++;
                }
            }
        }
    }
}
