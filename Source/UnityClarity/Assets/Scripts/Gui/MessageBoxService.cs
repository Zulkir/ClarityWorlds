using System;
using Clarity.Engine.Gui.MessageBoxes;

namespace Assets.Scripts.Gui
{
    public class MessageBoxService : IMessageBoxService
    {
        public bool? Show(string text, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxType type = MessageBoxType.Information)
        {
            throw new NotImplementedException();
        }
    }
}