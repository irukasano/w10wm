﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using windows10windowManagerUtil.Window;

namespace windows10windowManager.Window
{
    public class WindowTilerNone : AbstractWindowTiler
    {
        public override WindowRect GetWindowRectOf(int windowIndex)
        {
            return null;
        }

    }
}
