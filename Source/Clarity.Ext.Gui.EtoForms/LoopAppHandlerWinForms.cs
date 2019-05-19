using System;
using Clarity.Native.Win32;

namespace Clarity.Ext.Gui.EtoForms
{
    public class LoopAppHandlerWinForms : Eto.WinForms.Forms.ApplicationHandler, ILoopAppHandler
    {
        public event Action NewFrame;

        public LoopAppHandlerWinForms()
        {
            System.Windows.Forms.Application.Idle += OnIdle;

            // Workaround for http://stackoverflow.com/questions/7572995/how-can-i-get-winforms-to-stop-silently-ignoring-unhandled-exceptions
            System.Windows.Forms.Application.ThreadException += PrimitiveExceptionHandler.HandleThread;
        }

        static bool AppStillIdle()
        {
            MSG msg;
            return !WinApi.PeekMessage(out msg, IntPtr.Zero, 0, 0, (PM)0);
        }

        void OnIdle(object sender, EventArgs e)
        {
            while (AppStillIdle())
                if (NewFrame != null)
                    NewFrame();
                else
                    System.Threading.Thread.Sleep(1);
        }
    }
}