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
        IntPtr monitorHandle { get; }

        RECT monitorRect { get; }

        MONITORINFO monitorInfo { get; }

    }
    #endregion

    /**
     * <summary>
     * Monitor information with handle.
     * </summary>
     */
    public class MonitorInfoWithHandle : IMonitorInfoWithHandle, IEquatable<MonitorInfoWithHandle>
    {
        #region Field
        /**
         * <summary>
         * Gets the monitor handle.
         * </summary>
         * <value>
         * The monitor handle.
         * </value>
         */
        public IntPtr monitorHandle { get; private set; }

        /**
         * <summary>
         * Gets the monitor rect
         * </summary>
         */
        public RECT monitorRect { get; private set; }

        /**
         * <summary>
         * Gets the monitor information.
         * </summary>
         * <value>
         * The monitor information.
         * </value>
         */
        public MONITORINFO monitorInfo { get; private set; }

        //protected MonitorInformationForm monitorInformationForm;

        private readonly object formLock = new object();

        #endregion


        /**
         * <summary>
         * Initializes a new instance of the <see cref="MonitorInfoWithHandle"/> class.
         * </summary>
         * <param name="monitorHandle">The monitor handle.</param>
         * <param name="monitorInfo">The monitor information.</param>
         */
        public MonitorInfoWithHandle(IntPtr monitorHandle, RECT monitorRect, MONITORINFO monitorInfo)
        {
            this.monitorHandle = monitorHandle;
            this.monitorRect = monitorRect;
            this.monitorInfo = monitorInfo;

            //this.monitorInformationForm = new MonitorInformationForm(this);
        }

        public bool Equals(MonitorInfoWithHandle other)
        {
            return this.monitorHandle == other.monitorHandle;
        }

        /**
         * <summary>
         * このモニターをハイライト表示する
         * </summary>
         */
        public void Highlight()
        {
            lock(this.formLock){
                //this.monitorInformationForm.Highlight();
                var monitorInformationForm = new MonitorInformationForm(this);
                monitorInformationForm.Highlight();
            }
        }

    }


}
