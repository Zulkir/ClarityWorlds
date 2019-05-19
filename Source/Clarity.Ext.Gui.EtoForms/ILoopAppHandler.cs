using System;

namespace Clarity.Ext.Gui.EtoForms
{
    public interface ILoopAppHandler : Eto.Forms.Application.IHandler
    {
        event Action NewFrame;
    }
}