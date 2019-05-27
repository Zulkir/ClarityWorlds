namespace Clarity.App.Worlds.Interaction.Tools
{
    public interface IToolMenuItem
    {
        void OnActivate();
        string Text { get; }
    }
}