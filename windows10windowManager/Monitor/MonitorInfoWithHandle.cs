using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Monitor
{
    /// <summary>
    /// Rectangle
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    /// <summary>
    /// Monitor information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORINFO
    {
        public uint size;
        public RECT monitor;
        public RECT work;
        public uint flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szDevice;
    }

    /// <summary>
    /// Monitor information with handle interface.
    /// </summary>
    public interface IMonitorInfoWithHandle
    {
        IntPtr MonitorHandle { get; }
        MONITORINFO MonitorInfo { get; }
    }

    /// <summary>
    /// Monitor information with handle.
    /// </summary>
    public class MonitorInfoWithHandle : IMonitorInfoWithHandle
    {
        /// <summary>
        /// Gets the monitor handle.
        /// </summary>
        /// <value>
        /// The monitor handle.
        /// </value>
        public IntPtr MonitorHandle { get; private set; }

        /// <summary>
        /// Gets the monitor information.
        /// </summary>
        /// <value>
        /// The monitor information.
        /// </value>
        public MONITORINFO MonitorInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorInfoWithHandle"/> class.
        /// </summary>
        /// <param name="monitorHandle">The monitor handle.</param>
        /// <param name="monitorInfo">The monitor information.</param>
        public MonitorInfoWithHandle(IntPtr monitorHandle, MONITORINFO monitorInfo)
        {
            MonitorHandle = monitorHandle;
            MonitorInfo = monitorInfo;
        }
    }


}
