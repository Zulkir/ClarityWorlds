using System;

namespace Clarity.Ext.Gui.EtoForms
{
    public class LoopAppHandlerGtk : Eto.GtkSharp.Forms.ApplicationHandler, ILoopAppHandler
    {
        public event Action NewFrame;

        public LoopAppHandlerGtk()
        {
            GLib.Idle.Add(OnIdle);
        }

        private bool OnIdle()
        {
            if (NewFrame != null)
                NewFrame();
            else
                System.Threading.Thread.Sleep(1);
            return true;
        }
    }
}