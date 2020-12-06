using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace windows10windowManager.Monitor
{
    #region Structure
    /**
     * <summary>
     * Rectangle
     * </summary>
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

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
    #endregion

    #region IMonitorInfoWithHandle
    /**
     * <summary>
     * Monitor information with handle interface.
     * </summary>
     */
    public interface IMonitorInfoWithHandle
    {
        IntPtr MonitorHandle { get; }

        RECT MonitorRect { get; }

        MONITORINFO MonitorInfo { get; }
    }
    #endregion

    /**
     * <summary>
     * Monitor information with handle.
     * </summary>
     */
    public class MonitorInfoWithHandle : IMonitorInfoWithHandle, IEquatable<MonitorInfoWithHandle>
    {
        /**
         * <summary>
         * Gets the monitor handle.
         * </summary>
         * <value>
         * The monitor handle.
         * </value>
         */
        public IntPtr MonitorHandle { get; private set; }

        /**
         * <summary>
         * Gets the monitor rect
         * </summary>
         */
        public RECT MonitorRect { get; private set; }

        /**
         * <summary>
         * Gets the monitor information.
         * </summary>
         * <value>
         * The monitor information.
         * </value>
         */
        public MONITORINFO MonitorInfo { get; private set; }

        /**
         * <summary>
         * Initializes a new instance of the <see cref="MonitorInfoWithHandle"/> class.
         * </summary>
         * <param name="monitorHandle">The monitor handle.</param>
         * <param name="monitorInfo">The monitor information.</param>
         */
        public MonitorInfoWithHandle(IntPtr monitorHandle, RECT monitorRect, MONITORINFO monitorInfo)
        {
            this.MonitorHandle = monitorHandle;
            this.MonitorRect = monitorRect;
            this.MonitorInfo = monitorInfo;
        }

        public bool Equals(MonitorInfoWithHandle other)
        {
            return this.MonitorHandle == other.MonitorHandle;
        }

    }


}
