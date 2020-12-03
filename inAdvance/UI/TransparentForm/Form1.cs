using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransparentForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 10; i >= 0; i--)
            {
                //フォームの不透明度を変更する
                this.Opacity = 0.1 * i;
                //一時停止
                System.Threading.Thread.Sleep(100);
            }

            for (int i = 0; i <= 10; i++)
            {
                //フォームの不透明度を変更する
                this.Opacity = 0.1 * i;
                //一時停止
                System.Threading.Thread.Sleep(100);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
