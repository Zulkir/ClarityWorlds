using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Skyboxes
{
    public class Skybox : ResourceBase, ISkybox
    {
        public int Width { get; }
        private readonly IImage[] images;

        public Skybox(ResourceVolatility volatility, int width, IImage[] images) 
            : base(volatility)
        {
            Width = width;
            this.images = images;
        }

        public byte[] GetRawData(SkyboxFace skyboxFace)
        {
            return images[(int)skyboxFace].GetRawData();
        }
    }
}