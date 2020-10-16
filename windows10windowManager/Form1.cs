using System;
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

            // MonitorManager にWindowHandles を渡し、モニターごとにWindowManagerで管理する


        }

        /*
         * ウィンドウ表示イベントが発生したら、
         * これを該当モニターのWindowManagerに追加し、現在のモードで整列しなおす
         */

        /*
         * ウィンドウHideイベントが発生したら
         * 該当モニターのWindowManagerから削除し、現在のモードで整列しなおす
         */

        /*
         * ウィンドウ移動イベントが発生したら
         * 移動前モニターのWindowManagerから削除し
         * 移動先モニターのWindowManagerに追加する
         * 移動前、移動先両方を現在のモードで整列しなおす
         */

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
