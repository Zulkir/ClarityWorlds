using System.Collections.Concurrent;

namespace Clarity.Engine.Visualization.Elements.Samplers 
{
    public class ImageSampler : IImageSampler 
    {
        public static ImageSampler Default { get; } = new ImageSampler();
        private static ConcurrentDictionary<ImageSamplerData, ImageSampler> GlobalCache { get; } = 
            new ConcurrentDictionary<ImageSamplerData, ImageSampler>();

        public static ImageSampler FromGlobalCache(ImageSamplerData data) =>
            GlobalCache.GetOrAdd(data, x => new ImageSampler(x));

        public ImageSamplerFilter MagFilter { get; set; }
        public ImageSamplerFilter MinFilter { get; set; }
        public ImageSamplerFilter MipFilter { get; set; }
        public ImageSamplerAddressMode AddressModeU { get; set; }
        public ImageSamplerAddressMode AddressModeV { get; set; }
        public ImageSamplerAddressMode AddressModeW { get; set; }
        public float MaxAnisotropy { get; set; }

        public ImageSampler()
        {
            MinFilter = ImageSamplerFilter.Linear;
            MagFilter = ImageSamplerFilter.Linear;
            MipFilter = ImageSamplerFilter.Linear;
            MaxAnisotropy = 16;
        }

        public ImageSampler(ImageSamplerData data)
        {
            MinFilter = data.MinFilter;
            MagFilter = data.MagFilter;
            MipFilter = data.MipFilter;
            AddressModeU = data.AddressModeU;
            AddressModeV = data.AddressModeV;
            AddressModeW = data.AddressModeW;
            MaxAnisotropy = data.MaxAnisotropy;
        }

        public ImageSamplerData GetData() => new ImageSamplerData(this);
        public ImageSampler FromGlobalCache() => FromGlobalCache(GetData());
    }
}