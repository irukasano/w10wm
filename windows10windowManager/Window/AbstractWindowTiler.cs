using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    public class WindowRect : IEquatable<WindowRect>
    {
        #region Field
        public int left;
        public int top;
        public int right;
        public int bottom;
        #endregion

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

        public WindowRect Clone()
        {
            return (WindowRect)MemberwiseClone();
        }

        public int GetX()
        {
            return this.left;
        }

        public int GetY()
        {
            return this.top;
        }

        /**
         * <summary>
         * このRectの横幅を戻す
         * </summary>
         */
        public int GetWidth()
        {
            return this.right - this.left;
        }

        /**
         * <summary>
         * このRectの高さを戻す
         * </summary>
         */
        public int GetHeight()
        {
            return this.bottom - this.top;
        }

        public override string ToString()
        {
            return $"top={top},left={left},bottom={bottom},right={right}";
        }
    }

    public class AbstractWindowTiler
    {
        #region Field
        protected List<WindowRect> windowRects = new List<WindowRect>();

        public WindowRect defaultWindowRect = new WindowRect(0, 500, 0 ,500);
        #endregion

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
        public virtual WindowRect GetWindowRectOf(int windowIndex)
        {
            if ( this.windowRects.Count == 0)
            {
                return this.defaultWindowRect;
            }
            if ( windowIndex < 0 )
            {
                // インデックスが負の数の場合は 0 番目を戻す
                return this.windowRects.ElementAt(0);
            }
            if ( windowIndex < this.windowRects.Count)
            {
                // 範囲内のインデックスが指定された場合
                return this.windowRects.ElementAt(windowIndex);
            }
            // 範囲外の場合は最後のものを戻す
            var maxWindowRectIndex = this.windowRects.Count - 1;
            return this.windowRects.ElementAt(maxWindowRectIndex);
        }

        public virtual int PushNewWindowInfo(List<WindowInfoWithHandle> windowInfos, WindowInfoWithHandle windowInfoWithHandle)
        {
            windowInfos.Insert(0, windowInfoWithHandle);
            return 0;
        }

    }

}
