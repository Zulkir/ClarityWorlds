namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextPixelSource : IRichTextPixelSource
    {
        public IRichTextBox TextBox { get; }

        public bool HasTransparency => TextBox.Text.Style.HasTransparency;

        public RichTextPixelSource(IRichTextBox textBox)
        {
            TextBox = textBox;
        }
    }
}