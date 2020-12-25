namespace windows10windowManager
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TToolStripMenuItemSetupTilingType = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileBugn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileMdi = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileFullMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileConcentration = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.FToolStripMenuItemWindowTo = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemWindowMaximize = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemWindowMinimize = new System.Windows.Forms.ToolStripMenuItem();
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemAddDenyExePath = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemAddDenyWindowTitles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemCloseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemRearrangeWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TToolStripMenuItemSetupTilingType,
            this.FToolStripMenuItemRearrangeWindow,
            this.toolStripMenuItem3,
            this.FToolStripMenuItemWindowTo,
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemExit,
            this.FToolStripMenuItemCloseMenu});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(316, 170);
            // 
            // TToolStripMenuItemSetupTilingType
            // 
            this.TToolStripMenuItemSetupTilingType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FToolStripMenuItemTileBugn,
            this.toolStripMenuItem2,
            this.FToolStripMenuItemTileMdi,
            this.FToolStripMenuItemTileFullMonitor,
            this.FToolStripMenuItemTileConcentration,
            this.FToolStripMenuItemTileNone});
            this.TToolStripMenuItemSetupTilingType.Name = "TToolStripMenuItemSetupTilingType";
            this.TToolStripMenuItemSetupTilingType.Size = new System.Drawing.Size(315, 22);
            this.TToolStripMenuItemSetupTilingType.Text = "このモニターのウィンドウの整理方法を変更する(&T)";
            // 
            // FToolStripMenuItemTileBugn
            // 
            this.FToolStripMenuItemTileBugn.Name = "FToolStripMenuItemTileBugn";
            this.FToolStripMenuItemTileBugn.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileBugn.Text = "ウィンドウをBug.n風タイル表示する(&B)";
            this.FToolStripMenuItemTileBugn.ToolTipText = "左側にメインウィンドウを大きく表示し、右側に複数のウィンドウを小さく表示します。";
            this.FToolStripMenuItemTileBugn.Click += new System.EventHandler(this.FToolStripMenuItemTileBugn_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(254, 22);
            this.toolStripMenuItem2.Text = "ウィンドウを４分割表示する(&F)";
            this.toolStripMenuItem2.ToolTipText = "ウィンドウを上下左右に同サイズで４分割表示します。";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.FToolStripMenuItemTile4Window_Click);
            // 
            // FToolStripMenuItemTileMdi
            // 
            this.FToolStripMenuItemTileMdi.Name = "FToolStripMenuItemTileMdi";
            this.FToolStripMenuItemTileMdi.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileMdi.Text = "ウィンドウをMDI表示する(&M)";
            this.FToolStripMenuItemTileMdi.ToolTipText = "MDIのようにすべてのウィンドウを一定サイズにして重ねて表示します。";
            this.FToolStripMenuItemTileMdi.Click += new System.EventHandler(this.FToolStripMenuItemTileMdi_Click);
            // 
            // FToolStripMenuItemTileFullMonitor
            // 
            this.FToolStripMenuItemTileFullMonitor.Name = "FToolStripMenuItemTileFullMonitor";
            this.FToolStripMenuItemTileFullMonitor.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileFullMonitor.Text = "ウィンドウを全画面表示する(&X)";
            this.FToolStripMenuItemTileFullMonitor.ToolTipText = "すべてのウィンドウを全画面表示します。";
            this.FToolStripMenuItemTileFullMonitor.Click += new System.EventHandler(this.FToolStripMenuItemTileFullMonitor_Click);
            // 
            // FToolStripMenuItemTileConcentration
            // 
            this.FToolStripMenuItemTileConcentration.Name = "FToolStripMenuItemTileConcentration";
            this.FToolStripMenuItemTileConcentration.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileConcentration.Text = "ウィンドウを集中モード表示する(&C)";
            this.FToolStripMenuItemTileConcentration.ToolTipText = "１番目のウィンドウを全画面表示し、それ以外は右下に寄せてある程度の大きさのウィンドウで表示します。";
            this.FToolStripMenuItemTileConcentration.Click += new System.EventHandler(this.FToolStripMenuItemTileConcentration_Click);
            // 
            // FToolStripMenuItemTileNone
            // 
            this.FToolStripMenuItemTileNone.Name = "FToolStripMenuItemTileNone";
            this.FToolStripMenuItemTileNone.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileNone.Text = "ウィンドウをタイル表示しない(&N)";
            this.FToolStripMenuItemTileNone.Visible = false;
            this.FToolStripMenuItemTileNone.Click += new System.EventHandler(this.FToolStripMenuItemTileNone_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(312, 6);
            // 
            // FToolStripMenuItemWindowTo
            // 
            this.FToolStripMenuItemWindowTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FToolStripMenuItemWindowMaximize,
            this.FToolStripMenuItemWindowMinimize});
            this.FToolStripMenuItemWindowTo.Name = "FToolStripMenuItemWindowTo";
            this.FToolStripMenuItemWindowTo.Size = new System.Drawing.Size(315, 22);
            this.FToolStripMenuItemWindowTo.Text = "現在アクティヴなウィンドウを操作する(&W)";
            // 
            // FToolStripMenuItemWindowMaximize
            // 
            this.FToolStripMenuItemWindowMaximize.Name = "FToolStripMenuItemWindowMaximize";
            this.FToolStripMenuItemWindowMaximize.Size = new System.Drawing.Size(127, 22);
            this.FToolStripMenuItemWindowMaximize.Text = "最大化(&X)";
            this.FToolStripMenuItemWindowMaximize.Click += new System.EventHandler(this.FToolStripMenuItemWindowMaximize_Click);
            // 
            // FToolStripMenuItemWindowMinimize
            // 
            this.FToolStripMenuItemWindowMinimize.Name = "FToolStripMenuItemWindowMinimize";
            this.FToolStripMenuItemWindowMinimize.Size = new System.Drawing.Size(127, 22);
            this.FToolStripMenuItemWindowMinimize.Text = "最小化(&N)";
            this.FToolStripMenuItemWindowMinimize.Click += new System.EventHandler(this.FToolStripMenuItemWindowMinimize_Click);
            // 
            // 現在のウィンドウを管理下から常に除外ToolStripMenuItem
            // 
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FToolStripMenuItemAddDenyExePath,
            this.FToolStripMenuItemAddDenyWindowTitles});
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem.Name = "現在のウィンドウを管理下から常に除外ToolStripMenuItem";
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem.Size = new System.Drawing.Size(315, 22);
            this.現在のウィンドウを管理下から常に除外ToolStripMenuItem.Text = "現在アクティヴなウィンドウを管理下から常に除外(&-)";
            // 
            // FToolStripMenuItemAddDenyExePath
            // 
            this.FToolStripMenuItemAddDenyExePath.Name = "FToolStripMenuItemAddDenyExePath";
            this.FToolStripMenuItemAddDenyExePath.Size = new System.Drawing.Size(200, 22);
            this.FToolStripMenuItemAddDenyExePath.Text = "実行ファイル名で除外(&E)";
            this.FToolStripMenuItemAddDenyExePath.Click += new System.EventHandler(this.FToolStripMenuItemAddDenyExePath_Click);
            // 
            // FToolStripMenuItemAddDenyWindowTitles
            // 
            this.FToolStripMenuItemAddDenyWindowTitles.Name = "FToolStripMenuItemAddDenyWindowTitles";
            this.FToolStripMenuItemAddDenyWindowTitles.Size = new System.Drawing.Size(200, 22);
            this.FToolStripMenuItemAddDenyWindowTitles.Text = "ウィンドウタイトルで除外(&T)";
            this.FToolStripMenuItemAddDenyWindowTitles.Click += new System.EventHandler(this.FToolStripMenuItemAddDenyWindowTitles_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(312, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(315, 22);
            this.ToolStripMenuItemExit.Text = "アプリケーションを終了(&Q)";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // FToolStripMenuItemCloseMenu
            // 
            this.FToolStripMenuItemCloseMenu.Name = "FToolStripMenuItemCloseMenu";
            this.FToolStripMenuItemCloseMenu.Size = new System.Drawing.Size(315, 22);
            this.FToolStripMenuItemCloseMenu.Text = "このメニューを閉じる(&X)";
            this.FToolStripMenuItemCloseMenu.Click += new System.EventHandler(this.FToolStripMenuItemCloseMenu_Click);
            // 
            // FToolStripMenuItemRearrangeWindow
            // 
            this.FToolStripMenuItemRearrangeWindow.Name = "FToolStripMenuItemRearrangeWindow";
            this.FToolStripMenuItemRearrangeWindow.Size = new System.Drawing.Size(315, 22);
            this.FToolStripMenuItemRearrangeWindow.Text = "このモニターのウィンドウを整理しなおす(&R)";
            this.FToolStripMenuItemRearrangeWindow.Click += new System.EventHandler(this.FToolStripMenuItemRearrangeWindow_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 20);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.TopMost = true;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 現在のウィンドウを管理下から常に除外ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemAddDenyExePath;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemAddDenyWindowTitles;
        private System.Windows.Forms.ToolStripMenuItem TToolStripMenuItemSetupTilingType;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileBugn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileMdi;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileFullMonitor;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileConcentration;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileNone;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemWindowTo;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemWindowMaximize;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemWindowMinimize;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemCloseMenu;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemRearrangeWindow;
    }
}

