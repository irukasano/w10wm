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
using windows10windowManager.Window;
using windows10windowManager.KeyHook;
using windows10windowManager.KeyHook.KeyMap;
using System.Diagnostics;

namespace windows10windowManager
{
    public partial class Form1 : Form
    {
        private MonitorManager monitorManager { get; set; }
        private TraceWindow traceWindow { get; set; }
        private InterceptKeyboard interceptKeyboard { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.traceWindow = new TraceWindow();
            this.traceWindow.ShowEvent += TraceWindow_ShowEvent;
            this.traceWindow.HideEvent += TraceWindow_HideEvent;
            this.traceWindow.Hook();

            this.monitorManager = new MonitorManager(this.traceWindow);

            this.interceptKeyboard = new InterceptKeyboard();
            this.interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            this.interceptKeyboard.KeyUpEvent += InterceptKeyboard_KeyUpEvent;
            this.interceptKeyboard.Hook();
        }

        private void InterceptKeyboard_KeyUpEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {

        }

        /**
         * <summary>
         * ホットキーの定義
         * </summary>
         */
        private void InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            this.interceptKeyboard.callNextHook = true;


            if (e.equals(OriginalKey.J, new int[] { (int)OriginalKey.LeftWindows }))
            {
                Debug.WriteLine("With LeftWindows + J");
                this.monitorManager.GetCurrentMonitorWindowManager().MoveCurrentFocusPrevious().ActivateWindow();
                this.interceptKeyboard.callNextHook = false;
            }
            else if (e.equals(OriginalKey.K, new int[] { (int)OriginalKey.LeftWindows }))
            {
                Debug.WriteLine("With LeftWindows + K");
                this.monitorManager.GetCurrentMonitorWindowManager().MoveCurrentFocusNext().ActivateWindow();
                this.interceptKeyboard.callNextHook = false;
            }
            else
            {
                Debug.WriteLine("Else key");
            }
        }


        /**
         * <summary>
         * ウィンドウ表示イベントが発生したら、
         * これを該当モニターのWindowManagerに追加し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_ShowEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            Debug.WriteLine("Window Show : " + w.WindowInfo.WindowTitle);
            var windowManager = this.monitorManager.AddWindowInfo(w.WindowInfo);
            windowManager.ArrangeWindows();
        }


        /**
         * <summary>
         * ウィンドウHideイベントが発生したら
         * 該当モニターのWindowManagerから削除し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_HideEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            Debug.WriteLine("Window Show : " + w.WindowInfo.WindowTitle);
            var windowManager = this.monitorManager.RemoveWindowInfo(w.WindowInfo);
            windowManager.ArrangeWindows();
        }

        /**
         * <summary>
         * ウィンドウ移動イベントが発生したら
         * 移動前モニターのWindowManagerから削除し
         * 移動先モニターのWindowManagerに追加する
         * 移動前、移動先両方を現在のモードで整列しなおす
         * </summary>
         */



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.traceWindow.UnHook();
            this.interceptKeyboard.UnHook();
        }
    }
}
