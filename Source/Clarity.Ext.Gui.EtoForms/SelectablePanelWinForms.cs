using System;
using System.Drawing;
using System.Windows.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class SelectablePanelWinForms : Panel
    {
        private Form fullscreenHost;
        public event Action FullscreenEnded;

        public SelectablePanelWinForms()
        {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                return true;
            return base.IsInputKey(keyData);
        }

        public void GoFullscreen()
        {
            fullscreenHost = new Form();
            fullscreenHost.FormBorderStyle = FormBorderStyle.None;
            //fullscreenHost.ShowInTaskbar = false;
            
            var origianlLocation = Location;
            var originalDock = Dock;
            var originalParent = Parent;
            var originalForm = originalParent;
            while (!(originalForm is Form))
                originalForm = originalForm.Parent;

            Parent = fullscreenHost;
            Location = Point.Empty;
            Dock = DockStyle.Fill;

            fullscreenHost.FormClosing += (s, e) => {
                Parent = originalParent;
                Dock = originalDock;
                Location = origianlLocation;
                originalForm.Show();
            };

            fullscreenHost.KeyPreview = true;
            //fullscreenHost.KeyDown += (s, e) =>
            //{
            //    if (e.KeyCode == Keys.Escape)
            //        fullscreenHost.Close();
            //};
            fullscreenHost.Closed += (s, e) =>
            {
                FullscreenEnded?.Invoke();
            };
            fullscreenHost.Show();
            fullscreenHost.Location = originalForm.Location;
            fullscreenHost.WindowState = FormWindowState.Maximized;
            originalForm.Hide();
        }

        public void EndFullscreen()
        {
            fullscreenHost.Close();
            fullscreenHost = null;
        }
    }
}