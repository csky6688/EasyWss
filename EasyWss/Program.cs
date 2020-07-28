using System;
using System.Windows.Forms;

namespace EasyWss
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new InitForm());
            }
            else
            {
                Application.Run(new MainForm(args[0]));
            }

           


        }
    }
}
