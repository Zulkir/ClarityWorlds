using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public static class PrimitiveExceptionHandler
    {
        public static void HandleUnhandled(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            var message = BuildMessage(ex);
            WriteToConsole(message);
            DisplayWinFormsMessageBox(message);
        }

        public static void HandleThread(object sender, ThreadExceptionEventArgs args)
        {
            var ex = args.Exception;
            var message = BuildMessage(ex);
            WriteToConsole(message);
            DisplayWinFormsMessageBox(message);
        }

        private static void WriteToConsole(string message)
        {
            Console.Error.WriteLine(message);
            Console.Out.WriteLine(message);
        }

        private static void DisplayWinFormsMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static string BuildMessage(Exception ex)
        {
            var builder = new StringBuilder();
            var currentEx = ex;
            while (currentEx != null)
            {
                builder.AppendLine(currentEx.Message);
                builder.AppendLine();
                currentEx = currentEx.InnerException;
            }
            builder.AppendLine(ex.StackTrace);
            return builder.ToString();
        }
    }
}