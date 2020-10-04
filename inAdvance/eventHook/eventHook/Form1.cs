using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eventHook
{
    public partial class Form1 : Form
    {
        private TraceProcess traceProcess { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            traceProcess = new TraceProcess();
            traceProcess.Hook();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            traceProcess.UnHook();
        }
    }
}
