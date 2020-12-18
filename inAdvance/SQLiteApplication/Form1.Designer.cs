
namespace SQLiteApplication
{
    partial class Form1
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
            this.buttonCreateDatabase = new System.Windows.Forms.Button();
            this.buttonCreateTable = new System.Windows.Forms.Button();
            this.buttonInsertData = new System.Windows.Forms.Button();
            this.buttonSelectData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCreateDatabase
            // 
            this.buttonCreateDatabase.Location = new System.Drawing.Point(108, 60);
            this.buttonCreateDatabase.Name = "buttonCreateDatabase";
            this.buttonCreateDatabase.Size = new System.Drawing.Size(186, 23);
            this.buttonCreateDatabase.TabIndex = 0;
            this.buttonCreateDatabase.Text = "CreateDatabase";
            this.buttonCreateDatabase.UseVisualStyleBackColor = true;
            this.buttonCreateDatabase.Click += new System.EventHandler(this.buttonCreateDatabase_Click);
            // 
            // buttonCreateTable
            // 
            this.buttonCreateTable.Location = new System.Drawing.Point(108, 103);
            this.buttonCreateTable.Name = "buttonCreateTable";
            this.buttonCreateTable.Size = new System.Drawing.Size(186, 23);
            this.buttonCreateTable.TabIndex = 1;
            this.buttonCreateTable.Text = "CreateTable";
            this.buttonCreateTable.UseVisualStyleBackColor = true;
            this.buttonCreateTable.Click += new System.EventHandler(this.buttonCreateTable_Click);
            // 
            // buttonInsertData
            // 
            this.buttonInsertData.Location = new System.Drawing.Point(108, 148);
            this.buttonInsertData.Name = "buttonInsertData";
            this.buttonInsertData.Size = new System.Drawing.Size(186, 23);
            this.buttonInsertData.TabIndex = 2;
            this.buttonInsertData.Text = "InsertData";
            this.buttonInsertData.UseVisualStyleBackColor = true;
            this.buttonInsertData.Click += new System.EventHandler(this.buttonInsertData_Click);
            // 
            // buttonSelectData
            // 
            this.buttonSelectData.Location = new System.Drawing.Point(108, 195);
            this.buttonSelectData.Name = "buttonSelectData";
            this.buttonSelectData.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectData.TabIndex = 3;
            this.buttonSelectData.Text = "SelectData";
            this.buttonSelectData.UseVisualStyleBackColor = true;
            this.buttonSelectData.Click += new System.EventHandler(this.buttonSelectData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSelectData);
            this.Controls.Add(this.buttonInsertData);
            this.Controls.Add(this.buttonCreateTable);
            this.Controls.Add(this.buttonCreateDatabase);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreateDatabase;
        private System.Windows.Forms.Button buttonCreateTable;
        private System.Windows.Forms.Button buttonInsertData;
        private System.Windows.Forms.Button buttonSelectData;
    }
}

