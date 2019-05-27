using Clarity.Engine.Visualization.Elements.Samplers;
using ObjectGL.Api.Objects.Samplers;

namespace Clarity.Ext.Rendering.Ogl3.Caches 
{
    public interface ISamplerCache
    {
        ISampler GetGlSampler(IImageSampler cSampler);
    }
}