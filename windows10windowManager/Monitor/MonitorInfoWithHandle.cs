using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using windows10windowManagerUtil;
using windows10windowManagerUtil.Monitor;

namespace windows10windowManager.Monitor
{

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
