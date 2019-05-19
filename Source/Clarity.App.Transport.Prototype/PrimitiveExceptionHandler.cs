using System;
using System.Windows.Forms;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.App.Transport.Prototype
{
    public static class PrimitiveExceptionHandler
    {
        public static void Handle(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            var message = ex.GetCompleteMessage();
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
    }
}