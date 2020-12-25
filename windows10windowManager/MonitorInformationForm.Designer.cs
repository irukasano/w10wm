
namespace windows10windowManager
{
    partial class MonitorInformationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MonitorInformationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MonitorInformationLabel
            // 
            this.MonitorInformationLabel.AutoSize = true;
            this.MonitorInformationLabel.Font = new System.Drawing.Font("MS UI Gothic", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MonitorInformationLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MonitorInformationLabel.Location = new System.Drawing.Point(28, 28);
            this.MonitorInformationLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.MonitorInformationLabel.Name = "MonitorInformationLabel";
            this.MonitorInformationLabel.Size = new System.Drawing.Size(232, 22);
            this.MonitorInformationLabel.TabIndex = 0;
            this.MonitorInformationLabel.Text = "MonitorInformationLabel";
            this.MonitorInformationLabel.Visible = false;
            // 
            // MonitorInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MonitorInformationLabel);
            this.Name = "MonitorInformationForm";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MonitorInformationForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MonitorInformationLabel;
    }
}