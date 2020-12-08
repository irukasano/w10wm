using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManager.Window
{
    public class WindowRect : IEquatable<WindowRect>
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public WindowRect(int top, int bottom, int left, int right)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public bool Equals(WindowRect other)
        {
            return this.left == other.left &&
                this.top == other.top &&
                this.right == other.right &&
                this.bottom == other.bottom
            ;
        }
    }

    public class AbstractWindowTiler
    {
        /**
         * <summary>
         * 指定された  モニターサイズの中で windowCount (ウィンドウの数) で整列した場合の位置を求める
         * </summary>
         */
        public virtual void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {

        }

        /**
         * <summary>
         * 整列済みの位置情報のうち、指定された順番のウィンドウの位置をRECTで戻す
         * </summary>
         */
        public virtual WindowRect GetWindowRectOf(int windowIndex)
        {
            return new WindowRect(0, 0, 0, 0);
        }


    }

}
