using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows10windowManager.Window
{
    public class WindowTilerMaximize : AbstractWindowTiler
    {
        public override void CalcuratePosition(int windowCount, int monitorTop, int monitorBottom, int monitorLeft, int monitorRight)
        {
            this.windowRects.Add(new WindowRect(
                /* top     = */ monitorTop,
                /* bottom  = */ monitorBottom,
                /* left    = */ monitorLeft,
                /* right   = */ monitorRight
            ));
        }

    }
}
