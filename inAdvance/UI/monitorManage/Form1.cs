using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monitorManage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MonitorManager monitorManager = new MonitorManager();
            MonitorInfoWithHandle[] monitorInfoWithHandle = monitorManager.GetMonitors();

            for (int i = 0; i < monitorInfoWithHandle.Length; i++)
            {
                Debug.WriteLine("---");
                //Debug.WriteLine(monitorInfoWithHandle[i].MonitorHandle);
                Debug.WriteLine(new String(monitorInfoWithHandle[i].MonitorInfo.szDevice));
                Debug.WriteLine("");
                Debug.WriteLine(monitorInfoWithHandle[i].MonitorInfo.work.left.ToString());
                Debug.WriteLine(monitorInfoWithHandle[i].MonitorInfo.work.top.ToString());
                Debug.WriteLine(monitorInfoWithHandle[i].MonitorInfo.work.right.ToString());
                Debug.WriteLine(monitorInfoWithHandle[i].MonitorInfo.work.bottom.ToString());
            }

            List<MonitorInfoWithHandle> monitors = monitorManager.MonitorInfos;

            foreach (var monitor in monitors)
            {
                Debug.WriteLine(new String(monitor.MonitorInfo.szDevice));
                Debug.WriteLine("");
            }

        }
    }
}
