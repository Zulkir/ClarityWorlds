namespace Clarity.Ext.Rendering.Ogl3
{
    public class RenderStage : IRenderStage
    {
        public string Name { get; }
        public IRenderQueue Queue { get; }

        public RenderStage(string name, IRenderQueue queue)
        {
            Name = name;
            Queue = queue;
        }
    }
}