namespace Clarity.Core.AppCore.Tools
{
    public interface IToolMenuItem
    {
        void OnActivate();
        string Text { get; }
    }
}