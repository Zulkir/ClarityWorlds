using JetBrains.Annotations;

namespace Clarity.Core.AppCore.Tools
{
    public interface IToolService
    {
        [CanBeNull]
        ITool CurrentTool { get; set; }
    }
}