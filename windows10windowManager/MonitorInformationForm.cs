using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using windows10windowManager.Monitor;
using windows10windowManager.Util;

namespace windows10windowManager
{
    public partial class MonitorInformationForm : Form
    {
        public MonitorInformationForm(MonitorInfoWithHandle monitorInfoWithHandle)
        {
            InitializeComponent();

            ShowInTaskbar = false;

            // このフォームはモニターに全画面表示するのでタイトルなし、境界なし
            this.FormBorderStyle = FormBorderStyle.None;
            var hilightColorArgb = System.Drawing.SystemColors.Highlight.ToArgb();
            var backColorArgb = hilightColorArgb | 666666;
            this.BackColor = System.Drawing.Color.FromArgb(backColorArgb);
            this.Opacity = 0.8D;

            // 指定のモニターで最大化表示する
            var monitorRect = monitorInfoWithHandle.MonitorInfo.monitor;

            this.Top = monitorRect.top;
            this.Left = monitorRect.left;
            this.Width = monitorRect.right - monitorRect.left;
            this.Height = monitorRect.bottom - monitorRect.top;
        }

        private void MonitorInformationForm_Shown(object sender, EventArgs e)
        {
            for (int i = 20; i >= 0; i--)
            {
                //フォームの不透明度を変更する
                this.Opacity = 0.05 * i;
                //一時停止
                System.Threading.Thread.Sleep(50);
            }

            this.Close();
        }
    }
}
