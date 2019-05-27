namespace Clarity.Engine.Visualization.Elements.Samplers
{
    public interface IImageSampler
    {
        ImageSamplerFilter MagFilter { get; }
        ImageSamplerFilter MinFilter { get; }
        ImageSamplerFilter MipFilter { get; }
        ImageSamplerAddressMode AddressModeU { get; }
        ImageSamplerAddressMode AddressModeV { get; }
        ImageSamplerAddressMode AddressModeW { get; }
        float MaxAnisotropy { get; }
    }
}