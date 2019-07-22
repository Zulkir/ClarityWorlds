using System;
using Clarity.Engine.Gui.WindowQueries;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class WindowQueryService : IWindowQueryService
    {
        public bool TryQueryText(string windowTitle, string initialText, out string text)
        {
            var screen = Screen.FromPoint(Mouse.Position);
            var size = new Size(400, 100);
            var dialog = new Dialog
            {
                Title = windowTitle,
                Size = size,
                Location = new Point(new PointF(
                    screen.Bounds.Left + (screen.Bounds.Width - size.Width) / 2,
                    screen.Bounds.Top + (screen.Bounds.Height - size.Height) / 2))
            };
            dialog.LostFocus += (s, a) => { dialog.Close(); };

            var textBox = new TextBox
            {
                Text = initialText
            };
            textBox.Font = new Font(textBox.Font.Family, 12);

            var ok = false;

            var okButton = new Button { Text = "OK" };
            okButton.Click += (s, a) => { ok = true; dialog.Close(); };

            var cancelButton = new Button { Text = "Cancel" };
            cancelButton.Click += (s, a) => { dialog.Close(); };

            dialog.Content = new TableLayout(
                new TableRow(textBox),
                new TableRow(new TableLayout(
                    new TableRow(okButton, cancelButton)
                    )
                )
            );

            dialog.ShowModal();
            text = textBox.Text;
            return ok;
        }

        public void QueryTextMutable(string windowTitle, string initialText, Action<string> onTextChanged)
        {
            var screen = Screen.FromPoint(Mouse.Position);
            var size = new Size(400, 100);
            var form = new Form
            {
                Title = windowTitle,
                Size = size,
                Location = new Point(new PointF(
                    screen.Bounds.Left + (screen.Bounds.Width - size.Width) / 2, 
                    screen.Bounds.Top + (screen.Bounds.Height - size.Height) / 2))
            };

            var textBox = new TextBox
            {
                Text = initialText
            };
            textBox.Font = new Font(textBox.Font.Family, 12);
            textBox.TextChanged += (s, a) => onTextChanged?.Invoke(textBox.Text);
            form.Content = textBox;

            form.LostFocus += (s, a) =>
            {
                onTextChanged = null;
                form.Close();
            };
            
            form.Show();
        }
    }
}