﻿using System;
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
using windows10windowManager.Window;

namespace windows10windowManager
{
    public partial class MonitorInformationForm : Form
    {
        #region Field
        protected MonitorInfoWithHandle MonitorInfo { get; private set; }

        private readonly object formLock = new object();
        #endregion


        public MonitorInformationForm(MonitorInfoWithHandle monitorInfoWithHandle)
        {
            InitializeComponent();

            ShowInTaskbar = false;

            // このフォームはモニターに全画面表示するのでタイトルなし、境界なし
            this.FormBorderStyle = FormBorderStyle.None;
            var hilightColorArgb = System.Drawing.SystemColors.Highlight.ToArgb();
            var backColorArgb = hilightColorArgb | 666666;
            this.BackColor = System.Drawing.Color.FromArgb(backColorArgb);

            this.MonitorInfo = monitorInfoWithHandle;

            // このモニターで最大化表示する
            var monitorRect = this.MonitorInfo.monitorRect;

            this.Top = monitorRect.top;
            this.Left = monitorRect.left;
            this.Width = monitorRect.right - monitorRect.left;
            this.Height = monitorRect.bottom - monitorRect.top;

            this.Location = new Point(monitorRect.left, monitorRect.top);

        }

        public void Highlight()
        {
            lock (this.formLock)
            {
                this.Opacity = 0.8D;
            }

            //this.BringToFront();
            //this.TopMost = true;
            this.Show();
            WindowInfoWithHandle.SetWindowForground(this.Handle);

            for (int i = 20; i >= 0; i--)
            {
                //フォームの不透明度を変更する
                lock (this.formLock)
                {
                    this.Opacity = 0.05 * i;
                }
                //一時停止
                System.Threading.Thread.Sleep(1);
                //await Task.Delay(30);
            }

            this.Hide();
            this.Close();
        }


    }
}
