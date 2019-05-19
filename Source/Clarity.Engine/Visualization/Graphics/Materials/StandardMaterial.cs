using Clarity.Engine.Media.Images;

namespace Clarity.Engine.Visualization.Graphics.Materials
{
    public class StandardMaterial : IStandardMaterial
    {
        public IPixelSource DiffuseTextureSource { get; set; }
        public bool IgnoreLighting { get; set; }
        public bool NoSpecular { get; set; }
        public float PointSize { get; set; }
        public float LineWidth { get; set; }
        public bool Hide { get; set; }

        public bool HasTransparency => DiffuseTextureSource.HasTransparency;

        public StandardMaterial(IPixelSource diffuseTextureSource)
        {
            DiffuseTextureSource = diffuseTextureSource;
            PointSize = 1;
            LineWidth = 1;
        }
    }
}