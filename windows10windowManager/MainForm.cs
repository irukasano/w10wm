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
using System.Reflection;

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

            try
            {
                this.InitializeHooks();

                this.InitializeTaskTray();

            } catch ( Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void InitializeTaskTray()
        {
            ShowInTaskbar = false;

            // メニュー項目を作成します。
            var menuItem = new ToolStripMenuItem();
            menuItem.Text = "このアプリケーションを終了(&Q)";
            menuItem.Click += new EventHandler(Exit_Click);

            // メニューを作成します。
            var menu = new ContextMenuStrip();
            menu.Items.Add(menuItem);

            // アイコンを作成します。
            // アイコンファイルは32x32の24bit Bitmap
            var icon = new NotifyIcon();
            icon.Icon = new Icon(".\\favicon.ico");
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
            this.traceWindow.ActivateEvent += TraceWindow_ActivateEvent;
            this.traceWindow.Hook();

            this.monitorManager = new MonitorManager(this.traceWindow);
        }

        private void Terminate()
        {
            this.traceWindow.UnHook();
            this.interceptKeyboard.UnHook();
            Logger.Close();
            Application.Exit();
        }

        /** ******************************************************************************************************************
         * コンテクストメニューの定義
         */
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Terminate();
        }

        private void FToolStripMenuItemTile4Window_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.FourDivided);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemTileBugn_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.Bugn);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemTileMdi_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.Mdi);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemTileFullMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.Maximize);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemTileConcentration_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.Concentration);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemTileNone_Click(object sender, EventArgs e)
        {
            try
            {
                this.monitorManager.GetCurrentMonitorWindowManager().SaveWindowTilingType(
                    this.monitorManager.GetCurrentWindowManagerIndex(),
                    WindowTilingType.None);
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemAddDenyExePath_Click(object sender, EventArgs e)
        {
            try
            {
                var activeWindowInfo = this.monitorManager.GetCurrentMonitorWindowManager().GetCurrentWindow();
                if ( activeWindowInfo == null)
                {
                    return;
                }
                Logger.DebugWindowInfo("Add Deny ExePath", activeWindowInfo);
                // アクティヴウィンドウの実行ファイル名を設定ファイルに記述
                // アクティヴウィンドウを管理下から削除して整頓し直す
                var activeWindowExePath = activeWindowInfo.ComputeWindowModuleFileName();
                this.traceWindow.denyExePaths.Add(activeWindowExePath);
                SettingManager.SaveStringList("Window_TraceWindow_Deny_ExePath", this.traceWindow.denyExePaths);
                this.monitorManager.RemoveWindowInfo(activeWindowInfo);
                this.traceWindow.RemoveWindowInfo(activeWindowInfo);
                this.ArrangeWindows();
                this.monitorManager.ActivateWindow();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemAddDenyWindowTitles_Click(object sender, EventArgs e)
        {
            try
            {
                var activeWindowInfo = this.monitorManager.GetCurrentMonitorWindowManager().GetCurrentWindow();
                if (activeWindowInfo == null)
                {
                    return;
                }
                Logger.DebugWindowInfo("Add Deny WindowTitle", activeWindowInfo);
                // アクティヴウィンドウのウィンドウタイトルを設定ファイルに記述
                // アクティヴウィンドウを管理下から削除して整頓し直す
                var activeWindowTitle = activeWindowInfo.windowTitle;
                this.traceWindow.denyWindowTitles.Add(activeWindowTitle);
                SettingManager.SaveStringList("Window_TraceWindow_Deny_WindowTitle", this.traceWindow.denyWindowTitles);
                this.monitorManager.RemoveWindowInfo(activeWindowInfo);
                this.traceWindow.RemoveWindowInfo(activeWindowInfo);
                this.ArrangeWindows();
                this.monitorManager.ActivateWindow();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemCloseMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.WriteLine("Close context menu");
                contextMenuStrip1.Close();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemWindowMaximize_Click(object sender, EventArgs e)
        {
            try
            {
                var activeWindowInfo = this.monitorManager.GetCurrentMonitorWindowManager().GetCurrentWindow();
                if (activeWindowInfo == null)
                {
                    return;
                }
                Logger.DebugWindowInfo("Window maximize", activeWindowInfo);
                activeWindowInfo.Maximize();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemWindowMinimize_Click(object sender, EventArgs e)
        {
            try
            {
                var activeWindowInfo = this.monitorManager.GetCurrentMonitorWindowManager().GetCurrentWindow();
                if (activeWindowInfo == null)
                {
                    return;
                }
                Logger.DebugWindowInfo("Window maximize", activeWindowInfo);
                activeWindowInfo.Minimize();
                this.monitorManager.GetCurrentMonitorWindowManager().MoveCurrentFocusPrevious();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        private void FToolStripMenuItemRearrangeWindow_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.WriteLine("Window rearrange");
                this.ArrangeWindows();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        /** ******************************************************************************************************************
         * <summary>
         * ホットキーの定義
         * </summary>
         */
        private bool InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            //Logger.WriteLine("InterceptKeyboard_KeyDownEvent : " + e.ToString());

            foreach (InterceptKeyboardSetting interceptKeyboardSetting in InterceptKeyboardSettingManager.GetSettings())
            {
                if ( e.Equals(interceptKeyboardSetting.key, interceptKeyboardSetting.modifiers))
                {
                    if ( interceptKeyboardSetting.needsAsync)
                    {
                        Task task = Task.Run(() => {
                            Type t = this.GetType();
                            MethodInfo methodInfo = t.GetMethod(interceptKeyboardSetting.methodName);
                            object r = methodInfo.Invoke(this, null);
                        });

                        task.ContinueWith((t) => {
                            if (t.Exception != null)
                            {
                                Logger.Exception(t.Exception);
                            }
                        });

                        return false;
                    } else
                    {
                        try
                        {
                            Type t = this.GetType();
                            MethodInfo methodInfo = t.GetMethod(interceptKeyboardSetting.methodName);
                            object r = methodInfo.Invoke(this, null);
                        }
                        catch ( Exception ex)
                        {
                            Logger.Exception(ex);
                            throw ex;
                        }
                    }
                }
            }
            return true;

        }


        /** ******************************************************************************************************************
         * ウィンドウイベントによる処理の定義
         * <summary>
         * ウィンドウ表示イベントが発生したら、
         * これを該当モニターのWindowManagerに追加し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_ShowEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            try
            {
                Logger.DebugWindowInfo("Window Show", w.WindowInfo);
                var windowManager = this.monitorManager.PushNewWindowInfo(w.WindowInfo);
                this.ArrangeWindows();
                this.monitorManager.ActivateWindow();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }


        /**
         * <summary>
         * ウィンドウHideイベントが発生したら
         * 該当モニターのWindowManagerから削除し、現在のモードで整列しなおす
         * </summary>
         */
        private void TraceWindow_HideEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            try
            {
                Logger.DebugWindowInfo("Window Hide", w.WindowInfo);
                var windowManager = this.monitorManager.RemoveWindowInfo(w.WindowInfo);
                this.ArrangeWindows();
                this.monitorManager.ActivateWindow();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
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
            try
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
                    var windowManager = this.monitorManager.PushNewWindowInfo(w.WindowInfo);
                    this.monitorManager.SetCurrentWindowManagerIndexByMonitorHandle(w.WindowInfo.monitorHandle);
                    this.ArrangeWindows();

                    // TODO フォーカスが移動先のモニターに移動してしまうが、元のモニターのほうがよいような気もする

                    this.HighlightActiveMonitor();
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        /**
         * <summary>
         * ウィンドウアクティヴ化イベントが発生したら
         * カレントモニターのカレントウィンドウと等しいか確認し
         * 異なればそのウィンドウをカレントウィンドウ、カレントモニターにする
         * </summary>
         */
        private void TraceWindow_ActivateEvent(object sender, TraceWindow.OriginalWinEventArg w)
        {
            try
            {
                Logger.DebugWindowInfo("Window Activated", w.WindowInfo);

                var currentWindowInfo = this.monitorManager.GetCurrentMonitorWindowManager().GetCurrentWindow();
                var activatedWindowInfo = w.WindowInfo;

                if ((currentWindowInfo == null) || currentWindowInfo.Equals(activatedWindowInfo))
                {
                    return;
                }

                // アクティヴ化されたウィンドウをカレントにして、そのウィンドウの所属モニターをカレントにする
                this.monitorManager.ActivateWindowInfo(activatedWindowInfo);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
        }

        /** ******************************************************************************************************************
        /**
         * <summary>
         * 現在のウィンドウを閉じる
         * </summary>
         */
        public void CloseCurrentWindow()
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
        public void MoveCurrentFocusPrevious()
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
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * ひとつ後ろのウィンドウをアクティヴにする
         * </summary>
         */
        public void MoveCurrentFocusNext()
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
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番上のウィンドウをアクティヴにする
         * </summary>
         */
        public void MoveCurrentFocusTop()
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
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 一番下のウィンドウをアクティヴにする
         * </summary>
         */
        public void MoveCurrentFocusBottom()
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
                this.ArrangeWindows();
                windowInfo.ActivateWindow();
            }
        }

        /**
         * <summary>
         * 現在のウィンドウをひとつ上に移動する
         * </summary>
         */
        public void SetWindowPrevious()
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
        public void SetWindowNext()
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
        public void SetWindowTop()
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
        public void SetWindowBottom()
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
        public void MoveCurrentFocusPreviousMonitor()
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
        public void MoveCurrentFocusNextMonitor()
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
        public void ActivateMonitorN(int monitorNumber)
        {
            Logger.WriteLine($"ActivateMonitorN : {monitorNumber}");
            var windowManager = this.monitorManager.ActivateMonitorNWindowManager(monitorNumber);
            this.monitorManager.ActivateWindow();
            this.HighlightActiveMonitor();
        }

        public void ActivateMonitor1()
        {
            this.ActivateMonitorN(0);
        }

        public void ActivateMonitor2()
        {
            this.ActivateMonitorN(1);
        }

        public void ActivateMonitor3()
        {
            this.ActivateMonitorN(2);
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
         * 現在のアクティヴモニターをその整列方法に基づいて整列しなおす
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
         * コンテキストメニューを表示する
         * </summary>
         */
        public void ShowContextMenu()
        {
            var monitorInfo = this.monitorManager.GetCurrentMonitor();

            System.Drawing.Point p = new System.Drawing.Point();
            p.X = monitorInfo.monitorRect.left + 100;
            p.Y = monitorInfo.monitorRect.top + 100;

            this.contextMenuStrip1.Show(p);
            WindowInfoWithHandle.SetWindowForground(this.contextMenuStrip1.Handle);
            //this.contextMenuStrip1.Focus();
        }

        /**
         * <summary>
         * ヘルプフォームを表示する
         * </summary>
         */
        public void ShowHelpForm()
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show();
        }

    }
}
