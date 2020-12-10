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
            this.FToolStripMenuItemTile4Window = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileBugn = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileMdi = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileFullMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.FToolStripMenuItemTileNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FToolStripMenuItemTile4Window,
            this.FToolStripMenuItemTileBugn,
            this.FToolStripMenuItemTileMdi,
            this.FToolStripMenuItemTileFullMonitor,
            this.FToolStripMenuItemTileNone,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 164);
            // 
            // FToolStripMenuItemTile4Window
            // 
            this.FToolStripMenuItemTile4Window.Name = "FToolStripMenuItemTile4Window";
            this.FToolStripMenuItemTile4Window.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTile4Window.Text = "ウィンドウを４分割表示する(&F)";
            this.FToolStripMenuItemTile4Window.Click += new System.EventHandler(this.FToolStripMenuItemTile4Window_Click);
            // 
            // FToolStripMenuItemTileBugn
            // 
            this.FToolStripMenuItemTileBugn.Name = "FToolStripMenuItemTileBugn";
            this.FToolStripMenuItemTileBugn.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileBugn.Text = "ウィンドウをBug.n風タイル表示する(&B)";
            this.FToolStripMenuItemTileBugn.Click += new System.EventHandler(this.FToolStripMenuItemTileBugn_Click);
            // 
            // FToolStripMenuItemTileMdi
            // 
            this.FToolStripMenuItemTileMdi.Name = "FToolStripMenuItemTileMdi";
            this.FToolStripMenuItemTileMdi.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileMdi.Text = "ウィンドウをMDI表示する(&M)";
            this.FToolStripMenuItemTileMdi.Click += new System.EventHandler(this.FToolStripMenuItemTileMdi_Click);
            // 
            // FToolStripMenuItemTileFullMonitor
            // 
            this.FToolStripMenuItemTileFullMonitor.Name = "FToolStripMenuItemTileFullMonitor";
            this.FToolStripMenuItemTileFullMonitor.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileFullMonitor.Text = "ウィンドウを全画面表示する(&X)";
            this.FToolStripMenuItemTileFullMonitor.Click += new System.EventHandler(this.FToolStripMenuItemTileFullMonitor_Click);
            // 
            // FToolStripMenuItemTileNone
            // 
            this.FToolStripMenuItemTileNone.Name = "FToolStripMenuItemTileNone";
            this.FToolStripMenuItemTileNone.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileNone.Text = "ウィンドウをタイル表示しない(&N)";
            this.FToolStripMenuItemTileNone.Click += new System.EventHandler(this.FToolStripMenuItemTileNone_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(254, 22);
            this.ToolStripMenuItemExit.Text = "終了";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTile4Window;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileBugn;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileMdi;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileFullMonitor;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileNone;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
    }
}

