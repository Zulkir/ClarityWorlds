using Clarity.Engine.Objects.Caching;
using JetBrains.Annotations;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Text
{
    public interface IBorderCurveLocalCache : ICache
    {
        int NumPoints { get; }
        [CanBeNull]
        IVertexArray GetVao();
    }
}