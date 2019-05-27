using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Visualization.Elements.Samplers;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Samplers;

namespace Clarity.Ext.Rendering.Ogl3.Caches
{
    public class SamplerCache : ISamplerCache
    {
        private readonly IContext glContext;
        private readonly Dictionary<IImageSampler, ISampler> dict;

        public SamplerCache(IContext context)
        {
            glContext = context;
            dict = new Dictionary<IImageSampler, ISampler>();
        }

        public ISampler GetGlSampler(IImageSampler cSampler) => 
            dict.GetOrAdd(cSampler, CreateGlSampler);

        private ISampler CreateGlSampler(IImageSampler cSampler)
        {
            var glSampler = glContext.Create.Sampler();
            glSampler.SetMagFilter(cSampler.MagFilter.ToOglMag());
            glSampler.SetMinFilter(Converters.ToOglMin(cSampler.MinFilter, cSampler.MipFilter));
            glSampler.SetWrapR(cSampler.AddressModeU.ToOgl());
            glSampler.SetWrapS(cSampler.AddressModeV.ToOgl());
            glSampler.SetWrapT(cSampler.AddressModeW.ToOgl());
            glSampler.SetMaxAnisotropy(cSampler.MaxAnisotropy);
            return glSampler;
        }
    }
}