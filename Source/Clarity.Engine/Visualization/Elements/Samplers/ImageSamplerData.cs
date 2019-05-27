using System;

namespace Clarity.Engine.Visualization.Elements.Samplers 
{
    public struct ImageSamplerData : IEquatable<ImageSamplerData>
    {
        public ImageSamplerFilter MagFilter;
        public ImageSamplerFilter MinFilter;
        public ImageSamplerFilter MipFilter;
        public ImageSamplerAddressMode AddressModeU;
        public ImageSamplerAddressMode AddressModeV;
        public ImageSamplerAddressMode AddressModeW;
        public float MaxAnisotropy;

        public ImageSamplerData(IImageSampler sampler)
        {
            MinFilter = sampler.MinFilter;
            MagFilter = sampler.MagFilter;
            MipFilter = sampler.MipFilter;
            AddressModeU = sampler.AddressModeU;
            AddressModeV = sampler.AddressModeV;
            AddressModeW = sampler.AddressModeW;
            MaxAnisotropy = sampler.MaxAnisotropy;
        }

        public bool Equals(ImageSamplerData other)
        {
            return MagFilter == other.MagFilter && 
                   MinFilter == other.MinFilter && 
                   MipFilter == other.MipFilter && 
                   AddressModeU == other.AddressModeU && 
                   AddressModeV == other.AddressModeV && 
                   AddressModeW == other.AddressModeW && 
                   MaxAnisotropy.Equals(other.MaxAnisotropy);
        }

        public override bool Equals(object obj)
        {
            return obj is ImageSamplerData data && Equals(data);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)MagFilter;
                hashCode = (hashCode * 397) ^ (int)MinFilter;
                hashCode = (hashCode * 397) ^ (int)MipFilter;
                hashCode = (hashCode * 397) ^ (int)AddressModeU;
                hashCode = (hashCode * 397) ^ (int)AddressModeV;
                hashCode = (hashCode * 397) ^ (int)AddressModeW;
                hashCode = (hashCode * 397) ^ MaxAnisotropy.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ImageSamplerData left, ImageSamplerData right) => left.Equals(right);
        public static bool operator !=(ImageSamplerData left, ImageSamplerData right) => !left.Equals(right);
    }
}