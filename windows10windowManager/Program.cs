using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using windows10windowManager.Util;

namespace windows10windowManager
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 二重起動を禁止する
            bool createdNew;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "windows10windowManager", out createdNew);
            if ( createdNew == false)
            {
                mutex.Close();
                return;
            }

            try {
                Util.Logger.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new MainForm());
                var f = new MainForm();
                Application.Run();
            }
            catch ( Exception ex)
            {
                Logger.Exception(ex);
                throw ex;
            }
            finally
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
        }
    }
}
