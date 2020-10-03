using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using keyHook.KeyMap;

namespace keyHook
{
    public partial class Form1 : Form
    {
        private InterceptKeyboard interceptKeyboard { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            interceptKeyboard = new InterceptKeyboard();
            interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            interceptKeyboard.KeyUpEvent += InterceptKeyboard_KeyUpEvent;
            interceptKeyboard.Hook();

        }

        private  void InterceptKeyboard_KeyUpEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            Debug.WriteLine("Keyup KeyCode {0}", KeyMapConverter.KeyCodeToKey(e.KeyCode));
        }

        private  void InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            var key = KeyMapConverter.KeyCodeToKey(e.KeyCode);
            Debug.WriteLine("Keydown KeyCode {0}", key);

            if (e.equalsModifiers(new int[] { (int)OriginalKey.LeftWindows }))
            {
                Debug.WriteLine("With LeftWindows");
            }

            /*
            if ( key == OriginalKey.LeftWindows)
            {
                interceptKeyboard.callNextHook = false;
            } else
            {
                interceptKeyboard.callNextHook = true;
            }
            */
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            interceptKeyboard.UnHook();
        }
    }
}

