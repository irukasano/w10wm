using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        HotKey hotKey;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hotKey = new HotKey(MOD_KEY.ALT | MOD_KEY.CONTROL | MOD_KEY.SHIFT, Keys.F);
            hotKey.HotKeyPush += new EventHandler(hotKey_HotKeyPush1);
            hotKey = new HotKey(MOD_KEY.WIN, Keys.V);
            hotKey.HotKeyPush += new EventHandler(hotKey_HotKeyPush2);
        }

        void hotKey_HotKeyPush1(object sender, EventArgs e)
        {
            MessageBox.Show("ホットキーが押されました(CTL+SHIFT+ALT+F)");
        }
        void hotKey_HotKeyPush2(object sender, EventArgs e)
        {
            // これは動かない。WIN+Vホットキーはシステムで予約済みだから
            MessageBox.Show("ホットキーが押されました(WINキー+V)");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hotKey.Dispose();
        }
    }
}
