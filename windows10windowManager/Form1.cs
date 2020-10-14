﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Monitor;
using Window;

namespace windows10windowManager
{
    public partial class Form1 : Form
    {
        private MonitorManager monitorManager { get; set; }
        private TraceProcess traceProcess { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            monitorManager = new MonitorManager();
            //List<MonitorInfoWithHandle> monitors = monitorManager.MonitorInfos;

            traceProcess = new TraceProcess();
            // List<WindowInfoWithHandle> windowHandles = TraceProcess.WindowHandles;

            // MonitorManager にWindowHandles を渡し、モニターごとに管理する

        }

        /*
         * TODO KeyHook で Win + j が押されたとき
         * 
         * 現在のアクティヴモニター内のアクティヴウィンドウで、ひとつ上のウィンドウをアクティヴにする
         * 
         * アクティヴモニターのウィンドウリストを取得する( monitorManager)
         * ウィンドウリスト内の現在ウィンドウのひとつ上を取得する
         * そのウィンドウをアクティヴにする
         * 
         */
        // 

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            traceProcess.UnHook();
        }
    }
}
