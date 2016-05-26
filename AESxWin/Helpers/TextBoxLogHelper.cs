using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AESxWin.Helpers
{
    public static class TextBoxLogHelper
    {
        private static TextBox LogViewer { get; set; }

        public static void SetLogViewer(this Form form, TextBox textbox)
        {
            if (textbox == null)
                throw new NullReferenceException("SetLogViewer cannot accept null, where the log will be displayed then!, please provide a TextBox Control object.");
            LogViewer = textbox;
            
        }

        public static void Log(string message) // For calling outside the Form control
        {
            AppendMessage(FormatMessage(String.Empty, message, null));
        }
        public static void Log(string level, string message)  // For calling outside the Form control
        {
            AppendMessage(FormatMessage(level, message, null));
        }
        public static void Log(string level, string message, Exception exception)  // For calling outside the Form control
        {
            AppendMessage(FormatMessage(level, message, exception));
        }

        public static void Log(this Form form, string message)
        {
            AppendMessage(FormatMessage(String.Empty, message, null));
        }
        public static void Log(this Form form, string level, string message)
        {
            AppendMessage(FormatMessage(level, message, null));
        }
        public static void Log(this Form form, string level, string message, Exception exception)
        {
            AppendMessage(FormatMessage(level, message, exception));
        }


        private static void AppendMessage(string message)
        {
            if (LogViewer == null)
                throw new NullReferenceException("Please call the SetLogViewer method first with a valid TextBox Control.");


            if (LogViewer.InvokeRequired) // Thread-safe invoking check
            {
                LogViewer.Invoke(new Action(() =>
                {
                    LogViewer.Text += message;
                }));
            }
            else
            {
                LogViewer.Text += message;
                LogViewer.SelectionStart = LogViewer.Text.Length;
                LogViewer.ScrollToCaret();
            }
        }

        private static string FormatMessage(string level, string message, Exception exception)
        {
            if (!String.IsNullOrEmpty(level))
                level = " | " + level + " |";

            return String.Format("\r\n{0} :{1} {2} {3}", DateTime.Now, level.ToUpper(), message, exception);
        }




    }
}
