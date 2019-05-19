namespace Clarity.Engine.Media.Text.Common
{
    public interface ITextLineBreaker
    {
        bool CanBreakAt(string paragraphText, int breakPosition);
    }
}