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

        public override string ToString()
        {
            return $"top={top},left={left},bottom={bottom},right={right}";
        }
    }

    public class AbstractWindowTiler
    {
        protected List<WindowRect> windowRects = new List<WindowRect>();

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
         * もしwindowRectの数より大きな数が指定された場合は、一番最後を戻す
         * </summary>
         */
        public WindowRect GetWindowRectOf(int windowIndex)
        {
            if ( windowIndex < 0 )
            {
                if ( this.windowRects.Count > 0)
                {
                    return this.windowRects.ElementAt(0);
                } 
                return new WindowRect(0, 0, 0, 0);
            }
            if ( windowIndex < this.windowRects.Count)
            {
                return this.windowRects.ElementAt(windowIndex);
            }
            var maxWindowRectIndex = this.windowRects.Count - 1;
            return this.windowRects.ElementAt(maxWindowRectIndex);
        }


    }

}
