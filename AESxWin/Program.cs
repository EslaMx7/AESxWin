using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AESxWin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                if (args[0] == "/reg")
                {
                    RegisterApp();
                    return;
                }
                else if (args[0] == "/unreg")
                {
                    UnRegisterApp();
                    return;
                }
                Application.Run(new MainWindow(args));
            }
            else
            {
                Application.Run(new MainWindow());
            }
        }

        public static void RegisterApp()
        {
            try
            {
                var key = Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true).CreateSubKey("AESxWin");
                key.SetValue("", "Encrypt\\Decrypt with AESxWin", RegistryValueKind.String);
                key.SetValue("icon",Application.ExecutablePath,RegistryValueKind.String);
                key = key.CreateSubKey("command");
                key.SetValue("", Application.ExecutablePath + " \"%V\"");

                key = Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("shell", true).CreateSubKey("AESxWin");
                key.SetValue("", "Encrypt\\Decrypt with AESxWin", RegistryValueKind.String);
                key.SetValue("icon", Application.ExecutablePath, RegistryValueKind.String);
                key = key.CreateSubKey("command");
                key.SetValue("", Application.ExecutablePath + " \"%1\"");

                //key = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true).CreateSubKey("AESxWin");
                //key.SetValue("", "Encrypt\\Decrypt with AESxWin", RegistryValueKind.String);
                //key.SetValue("icon", Application.ExecutablePath, RegistryValueKind.String);
                //key = key.CreateSubKey("command");
                //key.SetValue("", Application.ExecutablePath + " \"%1\"");

                MessageBox.Show("Application has been registered with your system", "AESxWin", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                //throw;
                MessageBox.Show(ex.Message + "\nPlease run the application as Administrator !", "AESxWin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void UnRegisterApp()
        {
            try
            {
                var key = Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true);
                key.DeleteSubKeyTree("AESxWin");

                //key = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true);
                //key.DeleteSubKeyTree("AESxWin");

                key = Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("shell", true);
                key.DeleteSubKeyTree("AESxWin");

                MessageBox.Show("Application has been Unregistered from your system", "AESxWin", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease run the application as Administrator !", "AESxWin", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


    }


}
