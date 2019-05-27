namespace Clarity.Engine.Visualization.Elements.RenderStates
{
    public interface IStandardRenderState : IRenderState
    {
        CullFace CullFace { get; }
        PolygonMode PolygonMode { get; }
        float ZOffset { get; }
        float PointSize { get; }
        float LineWidth { get; }

        StandardRenderStateData GetData();
        StandardRenderStateImmutabilityFlags GetImmutability();
    }
}