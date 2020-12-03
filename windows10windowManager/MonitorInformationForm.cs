using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace windows10windowManager
{
    public partial class MonitorInformationForm : Form
    {
        public MonitorInformationForm()
        {
            InitializeComponent();
            
            // このフォームはモニターに全画面表示するのでタイトルなし、境界なし
            this.FormBorderStyle = FormBorderStyle.None;
            var hilightColorArgb = System.Drawing.SystemColors.Highlight.ToArgb();
            var backColorArgb = hilightColorArgb | 666666;
            this.BackColor = System.Drawing.Color.FromArgb(backColorArgb);
            this.Opacity = 0.8D;
        }

        private void MonitorInformationForm_Load(object sender, EventArgs e)
        {


            for (int i = 10; i >= 0; i--)
            {
                //フォームの不透明度を変更する
                this.Opacity = 0.05 * i;
                //一時停止
                System.Threading.Thread.Sleep(50);
            }

            this.Close();
        }
    }
}
