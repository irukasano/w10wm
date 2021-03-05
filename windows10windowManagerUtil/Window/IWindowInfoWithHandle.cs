using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace windows10windowManagerUtil.Window
{

    public interface IWindowInfoWithHandle
    {
        IntPtr windowHandle { get; }

        String windowTitle { get; }

        RECT position { get; }

        IntPtr monitorHandle { get; }
    }


}
