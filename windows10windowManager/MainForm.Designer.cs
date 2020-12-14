﻿namespace windows10windowManager
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
            this.FToolStripMenuItemTileConcentration = new System.Windows.Forms.ToolStripMenuItem();
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
            this.FToolStripMenuItemTileConcentration,
            this.FToolStripMenuItemTileNone,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 186);
            // 
            // FToolStripMenuItemTile4Window
            // 
            this.FToolStripMenuItemTile4Window.Name = "FToolStripMenuItemTile4Window";
            this.FToolStripMenuItemTile4Window.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTile4Window.Text = "ウィンドウを４分割表示する(&F)";
            this.FToolStripMenuItemTile4Window.ToolTipText = "ウィンドウを上下左右に同サイズで４分割表示します。";
            this.FToolStripMenuItemTile4Window.Click += new System.EventHandler(this.FToolStripMenuItemTile4Window_Click);
            // 
            // FToolStripMenuItemTileBugn
            // 
            this.FToolStripMenuItemTileBugn.Name = "FToolStripMenuItemTileBugn";
            this.FToolStripMenuItemTileBugn.Size = new System.Drawing.Size(254, 22);
            this.FToolStripMenuItemTileBugn.Text = "ウィンドウをBug.n風タイル表示する(&B)";
            this.FToolStripMenuItemTileBugn.ToolTipText = "左側にメインウィンドウを大きく表示し、右側に複数のウィンドウを小さく表示します。";
            this.FToolStripMenuItemTileBugn.Click += new System.EventHandler(this.FToolStripMenuItemTileBugn_Click);
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(251, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(254, 22);
            this.ToolStripMenuItemExit.Text = "終了(&Q)";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.Exit_Click);
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
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTile4Window;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileBugn;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileMdi;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileFullMonitor;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileNone;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem FToolStripMenuItemTileConcentration;
    }
}

