using System;
using Clarity.Engine.Gui.WindowQueries;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class WindowQueryService : IWindowQueryService
    {
        public void QueryTextMutable(string windowTitle, string initialText, Action<string> onTextChanged)
        {
            var screen = Screen.FromPoint(Mouse.Position);
            var size = new Size(400, 100);
            var textMutableForm = new Form
            {
                Title = windowTitle,
                Size = size,
                Location = new Point(new PointF(
                    screen.Bounds.Left + (screen.Bounds.Width - size.Width) / 2, 
                    screen.Bounds.Top + (screen.Bounds.Height - size.Height) / 2))
            };

            var textMutableTextBox = new TextBox
            {
                Text = initialText
            };
            textMutableTextBox.Font = new Font(textMutableTextBox.Font.Family, 12);
            textMutableTextBox.TextChanged += (s, a) => onTextChanged?.Invoke(textMutableTextBox.Text);
            textMutableForm.Content = textMutableTextBox;

            textMutableForm.LostFocus += (s, a) =>
            {
                onTextChanged = null;
                textMutableForm.Close();
            };
            
            textMutableForm.Show();
        }
    }
}