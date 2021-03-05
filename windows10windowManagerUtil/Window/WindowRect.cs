using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManagerUtil.Window
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
}
