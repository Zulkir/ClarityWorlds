using System;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Images
{
    public class SingleColorPixelSourceProxy<TMaster> : ISingleColorPixelSource
    {
        private readonly TMaster master;

        public SingleColorPixelSourceProxy(TMaster master)
        {
            this.master = master;
        }

        public Func<TMaster, Color4> GetColor { get; set; }
        public Func<TMaster, bool> GetHasTransparency { get; set; }

        public Color4 Color => (GetColor ?? (x => Color4.Red))(master);
        public bool HasTransparency => GetHasTransparency?.Invoke(master) ?? Color.A < 1;
    }
}