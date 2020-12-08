using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManager.Window
{
    public class WindowTilerMdi : AbstractWindowTiler
    {
        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {

        }

        public override WindowRect GetWindowRectOf(int windowIndex)
        {
            return base.GetWindowRectOf(windowIndex);
        }
    }
}
