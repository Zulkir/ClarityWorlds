using JetBrains.Annotations;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public interface IToolService
    {
        [CanBeNull]
        ITool CurrentTool { get; set; }
    }
}