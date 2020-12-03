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
    public partial class MainForm : Form
    {
        private MonitorManager monitorManager { get; set; }
        private TraceWindow traceWindow { get; set; }
        private InterceptKeyboard interceptKeyboard { get; set; }

        public MainForm()
        {
            InitializeComponent();

            this.InitializeTaskTray();

            this.InitializeHooks();
        }

        private void InitializeTaskTray()
        {
            ShowInTaskbar = false;

            // メニュー項目を作成します。
            var menuItem = new ToolStripMenuItem();
            menuItem.Text = "&Exit";
            menuItem.Click += new EventHandler(Exit_Click);

            // メニューを作成します。
            var menu = new ContextMenuStrip();
            menu.Items.Add(menuItem);

            // アイコンを作成します。
            // アイコンファイルは32x32の24bit Bitmap
            var icon = new NotifyIcon();
            icon.Icon = new Icon("..\\..\\favicon.ico");
            icon.Visible = true;
            icon.Text = "w10wm";
            icon.ContextMenuStrip = menu;
        }

        private void InitializeHooks()
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

        private void Exit_Click(object sender, EventArgs e)
        {
            this.traceWindow.UnHook();
            this.interceptKeyboard.UnHook();
            Application.Exit();
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
            int[] modifierLeftWindows = new int[] { (int)OriginalKey.LeftWindows };

            if (e.equals(OriginalKey.J, modifierLeftWindows))
            {
                Debug.WriteLine("With LeftWindows + J");
                this.MoveCurrentFocusPrevious();
                this.interceptKeyboard.callNextHook = false;
            }
            else if (e.equals(OriginalKey.K, modifierLeftWindows))
            {
                Debug.WriteLine("With LeftWindows + K");
                this.MoveCurrentFocusNext();
                this.interceptKeyboard.callNextHook = false;
            }
            else if (e.equals(OriginalKey.F1, modifierLeftWindows))
            {
                Debug.WriteLine("With LeftWindows + F1");
                this.ActivateMonitorN(0);
                this.interceptKeyboard.callNextHook = false;
            }
            else if (e.equals(OriginalKey.F2, modifierLeftWindows))
            {
                Debug.WriteLine("With LeftWindows + F2");
                this.ActivateMonitorN(1);
                this.interceptKeyboard.callNextHook = false;
            }
            else if (e.equals(OriginalKey.F3, modifierLeftWindows))
            {
                Debug.WriteLine("With LeftWindows + F3");
                this.ActivateMonitorN(2);
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
         * ひとつ上のウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusPrevious()
        {
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusPrevious();
            if (windowInfo is null)
            {
                return;
            }
            windowInfo.ActivateWindow();
        }

        /**
         * <summary>
         * ひとつ後ろのウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusNext()
        {
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusNext();
            if (windowInfo is null)
            {
                return;
            }
            windowInfo.ActivateWindow();
        }

        /**
         * <summary>
         * モニター１のカレントウィンドウをアクティヴにする
         * </summary>
         */
        private void ActivateMonitorN(int monitorNumber)
        {
            Debug.WriteLine($"Change Monitor : {monitorNumber}");
            var windowManager = this.monitorManager.GetMonitorNWindowManager(monitorNumber);
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.GetCurrentWindow();
            if ( windowInfo is null)
            {
                return;
            }
            windowInfo.ActivateWindow();
        }

        /**
         * <summary>
         * ウィンドウ移動イベントが発生したら
         * 移動前モニターのWindowManagerから削除し
         * 移動先モニターのWindowManagerに追加する
         * 移動前、移動先両方を現在のモードで整列しなおす
         * </summary>
         */



    }
}
