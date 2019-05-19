using JetBrains.Annotations;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IRenderStage
    {
        string Name { get; }
        [CanBeNull] IRenderQueue Queue { get; }
    }
}