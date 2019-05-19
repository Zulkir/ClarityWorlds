using System;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextPixelSourceProxy<TMaster> : IRichTextPixelSource
    {
        private readonly TMaster master;

        public RichTextPixelSourceProxy(TMaster master)
        {
            this.master = master;
        }

        public Func<TMaster, IRichTextBox> GetTextBox { get; set; }
        public Func<TMaster, bool> GetHasTransparency { get; set; }

        public IRichTextBox TextBox => (GetTextBox ?? (x => null))(master);
        public bool HasTransparency => GetHasTransparency?.Invoke(master) ?? TextBox.Text.Style.HasTransparency;
    }
}