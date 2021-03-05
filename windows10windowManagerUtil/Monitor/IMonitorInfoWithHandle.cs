using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace windows10windowManagerUtil.Monitor
{

    /**
     * <summary>
     * Monitor information.
     * </summary>
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public uint size;

        /**
         * <summary>
         * A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
         * </summary>
         */
        public RECT monitor;

        /**
         * <summary>
         * A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
         * </summary>
         */
        public RECT work;

        /**
         * <summary>
         * A set of flags that represent attributes of the display monitor.
         * MONITORINFOF_PRIMARY = This is the primary display monitor.
         * </summary>
         */
        public uint flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szDevice;
    }

    /**
     * <summary>
     * Monitor information with handle interface.
     * </summary>
     */
    public interface IMonitorInfoWithHandle
    {
        IntPtr monitorHandle { get; }

        RECT monitorRect { get; }

        MONITORINFO monitorInfo { get; }

    }
}
