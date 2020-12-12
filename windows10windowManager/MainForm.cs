using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Diagnostics;

using windows10windowManager.Monitor;
using windows10windowManager.Window;
using windows10windowManager.KeyHook;
using windows10windowManager.KeyHook.KeyMap;
using windows10windowManager.Util;

namespace windows10windowManager
{
    public partial class MainForm : Form
    {
        #region Field
        private MonitorManager monitorManager { get; set; }
        private TraceWindow traceWindow { get; set; }
        private InterceptKeyboard interceptKeyboard { get; set; }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            this.InitializeHooks();

            this.InitializeTaskTray();
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
            this.interceptKeyboard = new InterceptKeyboard();
            this.interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            //this.interceptKeyboard.KeyUpEvent += InterceptKeyboard_KeyUpEvent;
            this.interceptKeyboard.Hook();

            this.traceWindow = new TraceWindow();
            this.traceWindow.ShowEvent += TraceWindow_ShowEvent;
            this.traceWindow.HideEvent += TraceWindow_HideEvent;
            this.traceWindow.LocationChangeEvent += TraceWindow_LocationChangeEvent;
            this.traceWindow.Hook();

            this.monitorManager = new MonitorManager(this.traceWindow);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.traceWindow.UnHook();
            this.interceptKeyboard.UnHook();
            Logger.Close();
            Application.Exit();
        }

        private void FToolStripMenuItemTile4Window_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.FourDivided);
            this.ArrangeWindows();
        }

        private void FToolStripMenuItemTileBugn_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.Bugn);
            this.ArrangeWindows();
        }

        private void FToolStripMenuItemTileMdi_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.Mdi);
            this.ArrangeWindows();
        }

        private void FToolStripMenuItemTileFullMonitor_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.Maximize);
            this.ArrangeWindows();
        }

        private void FToolStripMenuItemTileConcentration_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.Concentration);
            this.ArrangeWindows();
        }

        private void FToolStripMenuItemTileNone_Click(object sender, EventArgs e)
        {
            this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                this.monitorManager.GetCurrentWindowManagerIndex(),
                WindowTilingType.None);
            this.ArrangeWindows();
        }

        /**
         * <summary>
         * ホットキーの定義
         * </summary>
         */
        private bool InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            //Logger.WriteLine("InterceptKeyboard_KeyDownEvent : " + e.ToString());

            int[] modifierLWindows = new int[] { (int)OriginalKey.LeftWindows };
            int[] modifierLShiftLWindows = new int[] { (int)OriginalKey.LeftWindows, (int)OriginalKey.LeftShift };
            int[] modifierRShiftLWindows = new int[] { (int)OriginalKey.LeftWindows, (int)OriginalKey.LeftShift };

            if (e.Equals(OriginalKey.J, modifierLWindows))
            {
                this.MoveCurrentFocusNext();
                return false;
            }
            else if (e.Equals(OriginalKey.K, modifierLWindows))
            {
                this.MoveCurrentFocusPrevious();
                return false;
            }
            else if (e.Equals(OriginalKey.U, modifierLWindows))
            {
                this.MoveCurrentFocusTop();
                return false;
            }
            else if (e.Equals(OriginalKey.M, modifierLWindows))
            {
                this.MoveCurrentFocusBottom();
                return false;
            }
            else if (e.Equals(OriginalKey.J, modifierLShiftLWindows) || e.Equals(OriginalKey.J, modifierRShiftLWindows))
            {
                this.SetWindowNext();
                return false;
            }
            else if (e.Equals(OriginalKey.K, modifierLShiftLWindows) || e.Equals(OriginalKey.K, modifierRShiftLWindows))
            {
                this.SetWindowPrevious();
                return false;
            }
            else if (e.Equals(OriginalKey.U, modifierLShiftLWindows) || e.Equals(OriginalKey.U, modifierRShiftLWindows))
            {
                this.SetWindowTop();
                return false;
            }
            else if (e.Equals(OriginalKey.M, modifierLShiftLWindows) || e.Equals(OriginalKey.M, modifierRShiftLWindows))
            {
                this.SetWindowBottom();
                return false;
            }
            else if (e.Equals(OriginalKey.X, modifierLWindows))
            {
                this.CloseCurrentWindow();
                return false;
            }
            else if (e.Equals(OriginalKey.C, modifierLWindows))
            {
                this.HighlightActiveMonitor();
                return false;
            }
            else if (e.Equals(OriginalKey.Period, modifierLWindows))
            {
                this.MoveCurrentFocusPreviousMonitor();
                return false;
            }
            else if (e.Equals(OriginalKey.Comma, modifierLWindows))
            {
                this.MoveCurrentFocusNextMonitor();
                return false;
            }
            else if (e.Equals(OriginalKey.F1, modifierLWindows))
            {
                this.ActivateMonitorN(0);
                return false;
            }
            else if (e.Equals(OriginalKey.F2, modifierLWindows))
            {
                this.ActivateMonitorN(1);
                return false;
            }
            else if (e.Equals(OriginalKey.F3, modifierLWindows))
            {
                this.ActivateMonitorN(2);
                return false;
            }
            else if (e.Equals(OriginalKey.O, modifierLWindows))
            {
                // 右クリックメニューを表示する
                var monitorInfo = this.monitorManager.GetCurrentMonitor();

                System.Drawing.Point p = new System.Drawing.Point();
                p.X = monitorInfo.monitorRect.left + 100;
                p.Y = monitorInfo.monitorRect.top + 100;

                /*
                var mainForm = new MainForm();
                mainForm.StartPosition = FormStartPosition.Manual;
                mainForm.DesktopLocation = p;
                mainForm.Show();
                */

                this.contextMenuStrip1.Show(p);
                this.contextMenuStrip1.Focus();
                return false;
            }

            return true;
        }


        /**
         * <summary>
         * ウィンドウ表示イベントが発生したら、
         * これを該当モニターのWindowManagerに追加し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_ShowEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            Logger.DebugWindowInfo("Window Show", w.WindowInfo);
            var windowManager = this.monitorManager.PushWindowInfo(w.WindowInfo);
            this.ArrangeWindows();
        }


        /**
         * <summary>
         * ウィンドウHideイベントが発生したら
         * 該当モニターのWindowManagerから削除し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_HideEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            Logger.DebugWindowInfo("Window Hide", w.WindowInfo);
            var windowManager = this.monitorManager.RemoveWindowInfo(w.WindowInfo);
            this.ArrangeWindows();
        }

        /**
         * <summary>
         * ウィンドウ移動イベントが発生したら
         * 該当モニターがモニター移動したかどうかを判断し
         * 移動していたら以下を行う
         * * 移動前モニターの整列しなおし、移動前WindowManagerからの削除
         * * 移動後モニターの整列しなおし、移動後WindowManagerへの追加
         * </summary>
         */
        private void TraceWindow_LocationChangeEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            Logger.DebugWindowInfo("Window LocationChange", w.WindowInfo);
            if (w.WindowInfo.MovedMonitor())
            {
                Logger.DebugWindowInfo("Window MonitorChange", w.WindowInfo);
                var beforeMovedMonitorHandle = w.WindowInfo.GetMonitorHandle();
                var beforeMovedWindowManager = this.monitorManager.FindWindowManagerByMonitorHandle(beforeMovedMonitorHandle);

                Logger.DebugWindowInfo("Remove From BeforeMovedWindowManager", w.WindowInfo);
                beforeMovedWindowManager.Remove(w.WindowInfo);
                this.ArrangeWindows();

                w.WindowInfo.ComputeMonitorHandle();
                Logger.DebugWindowInfo("Add To NewWindowManager", w.WindowInfo);
                var windowManager = this.monitorManager.PushWindowInfo(w.WindowInfo);
                this.monitorManager.SetCurrentWindowManagerIndexByMonitorHandle(w.WindowInfo.monitorHandle);
                this.ArrangeWindows();

                // TODO フォーカスが移動先のモニターに移動してしまうが、元のモニターのほうがよいような気もする

                this.HighlightActiveMonitor();
            }
        }

        /**
         * <summary>
         * 現在のウィンドウを閉じる
         * </summary>
         */
        private void CloseCurrentWindow()
        {
            Logger.WriteLine("CloseCurrentWindow");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.GetCurrentWindow();
            if (windowInfo != null)
            {
                windowInfo.WindowClose();
            }
        }

        /**
         * <summary>
         * ひとつ上のウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusPrevious()
        {
            Logger.WriteLine("MoveCurrentFocusPrevious");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusPrevious();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * ひとつ後ろのウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusNext()
        {
            Logger.WriteLine("MoveCurrentFocusNext");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusNext();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番上のウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusTop()
        {
            Logger.WriteLine("MoveCurrentFocusTop");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusTop();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番下のウィンドウをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusBottom()
        {
            Logger.WriteLine("MoveCurrentFocusBottom");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.MoveCurrentFocusBottom();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 現在のウィンドウをひとつ上に移動する
         * </summary>
         */
        private void SetWindowPrevious()
        {
            Logger.WriteLine("SetWindowPrevious");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.SetWindowPrevious();
            if (windowInfo != null)
            {
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 現在のウィンドウをひとつ下に移動する
         * </summary>
         */
        private void SetWindowNext()
        {
            Logger.WriteLine("SetWindowPrevious");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.SetWindowNext();
            if (windowInfo != null)
            {
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番上のウィンドウをひとつ下に移動する
         * </summary>
         */
        private void SetWindowTop()
        {
            Logger.WriteLine("SetWindowTop");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.SetWindowTop();
            if (windowInfo != null)
            {
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番上のウィンドウをひとつ下に移動する
         * </summary>
         */
        private void SetWindowBottom()
        {
            Logger.WriteLine("SetWindowBottom");
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.SetWindowBottom();
            if (windowInfo != null)
            {
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * ひとつ前のモニターをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusPreviousMonitor()
        {
            Logger.WriteLine("MoveCurrentFocusPreviousMonitor");
            var windowManager = this.monitorManager.MoveCurrentFocusPrevious();
            var windowInfo = windowManager.GetCurrentWindow();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
            this.HighlightActiveMonitor();
        }

        /**
         * <summary>
         * ひとつ後ろのモニターをアクティヴにする
         * </summary>
         */
        private void MoveCurrentFocusNextMonitor()
        {
            Logger.WriteLine("MoveCurrentFocusNextMonitor");
            var windowManager = this.monitorManager.MoveCurrentFocusNext();
            var windowInfo = windowManager.GetCurrentWindow();
            if (windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
            this.HighlightActiveMonitor();
        }


        /**
         * <summary>
         * モニターNのカレントウィンドウをアクティヴにする
         * </summary>
         */
        private void ActivateMonitorN(int monitorNumber)
        {
            Logger.WriteLine($"ActivateMonitorN : {monitorNumber}");
            var windowManager = this.monitorManager.ActivateMonitorNWindowManager(monitorNumber);
            if (windowManager is null)
            {
                return;
            }
            var windowInfo = windowManager.GetCurrentWindow();
            if ( windowInfo != null)
            {
                windowInfo.ActivateWindow();
            }
            this.HighlightActiveMonitor();
        }

        /**
         * <summary>
         * アクティヴモニターをハイライト表示する
         * </summary>
         */
        public void HighlightActiveMonitor()
        {
            this.monitorManager.HighlightCurrentMonitor();
        }

        /**
         * <summary>
         * </summary>
         */
        public void ArrangeWindows()
        {
            var windowManager = this.monitorManager.GetCurrentMonitorWindowManager();
            windowManager.RemoveInvisibleWindows();
            var monitorInfoWithHandle = this.monitorManager.GetCurrentMonitor();
            var windowTiler = new WindowTiler(
                /* windowTilingType =  */ windowManager.windowTilingType,
                /* windowCount =  */ windowManager.WindowCount(),
                /* monitorRect = */ monitorInfoWithHandle.monitorInfo.work);
            windowManager.ArrangeWindows(windowTiler);
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
