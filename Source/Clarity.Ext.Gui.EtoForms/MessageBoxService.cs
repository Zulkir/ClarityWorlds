using System;
using Clarity.Engine.Gui.MessageBoxes;
using Eto.Forms;
using MessageBoxButtons = Clarity.Engine.Gui.MessageBoxes.MessageBoxButtons;
using MessageBoxType = Clarity.Engine.Gui.MessageBoxes.MessageBoxType;

namespace Clarity.Ext.Gui.EtoForms
{
    public class MessageBoxService : IMessageBoxService
    {
        private readonly Lazy<IMainForm> mainFormLazy;

        public MessageBoxService(Lazy<IMainForm> mainFormLazy)
        {
            this.mainFormLazy = mainFormLazy;
        }

        public bool? Show(string text, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxType type = MessageBoxType.Information)
        {
            var result = MessageBox.Show(mainFormLazy.Value.Form, text, buttons.ToEto(), type.ToEto());
            switch (result)
            {
                case DialogResult.Ok:
                case DialogResult.Yes:
                case DialogResult.Retry:
                    return true;
                case DialogResult.No:
                case DialogResult.Abort:
                    return false;
                case DialogResult.None:
                case DialogResult.Cancel:
                case DialogResult.Ignore:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}