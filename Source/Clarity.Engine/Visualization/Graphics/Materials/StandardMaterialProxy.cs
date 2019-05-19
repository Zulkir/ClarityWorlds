using System;
using Clarity.Engine.Media.Images;

namespace Clarity.Engine.Visualization.Graphics.Materials
{
    public class StandardMaterialProxy<TMaster> : IStandardMaterial
    {
        private readonly TMaster master;

        public StandardMaterialProxy(TMaster master)
        {
            this.master = master;
        }

        public StandardMaterialProxy(TMaster master, Func<TMaster, IStandardMaterial> getMaterial)
        {
            this.master = master;
            GetDiffuseTextureSource = x => getMaterial(x).DiffuseTextureSource;
            GetLineWidth = x => getMaterial(x).LineWidth;
            GetPointSize = x => getMaterial(x).PointSize;
            GetIgnoreLighting = x => getMaterial(x).IgnoreLighting;
            GetNoSpecular = x => getMaterial(x).NoSpecular;
            GetHide = x => getMaterial(x).Hide;
            GetIsTransparent = x => getMaterial(x).HasTransparency;
        }

        public Func<TMaster, IPixelSource> GetDiffuseTextureSource { get; set; }
        public Func<TMaster, float> GetLineWidth { get; set; }
        public Func<TMaster, float> GetPointSize { get; set; }
        public Func<TMaster, bool> GetIgnoreLighting { get; set; }
        public Func<TMaster, bool> GetNoSpecular { get; set; }
        public Func<TMaster, bool> GetHide { get; set; }
        public Func<TMaster, bool> GetIsTransparent { get; set; }

        public IPixelSource DiffuseTextureSource => (GetDiffuseTextureSource ?? (x => null))(master);
        public float LineWidth => (GetLineWidth ?? (x => 1))(master);
        public bool Hide => (GetHide ?? (x => false))(master);
        public float PointSize => (GetPointSize ?? (x => 1))(master);
        public bool IgnoreLighting => (GetIgnoreLighting ?? (x => false))(master);
        public bool NoSpecular => (GetNoSpecular ?? (x => false))(master);
        public bool HasTransparency => GetIsTransparent?.Invoke(master) ?? DiffuseTextureSource.HasTransparency;
    }
}