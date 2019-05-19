using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Engine.Visualization.Viewports
{
    public abstract class Viewport : AmObjectBase<Viewport>, IViewport
    {
        public abstract int Left { get; set; }
        public abstract int Top { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }

        public float AspectRatio => GraphicsHelper.AspectRatio(Width, Height);
        public IView View { get; set; }

        public void OnResized(float left, float top, float width, float height)
        {
            Left = (int)Math.Round(left);
            Top = (int)Math.Round(top);
            Width = (int)Math.Round(width);
            Height = (int)Math.Round(height);
        }

        public static Viewport Create(IntVector2 topLeft, IntSize2 size, IView view)
        {
            var viewport = AmFactory.Create<Viewport>();
            viewport.Left = topLeft.X;
            viewport.Top = topLeft.Y;
            viewport.Width = size.Width;
            viewport.Height = size.Height;
            viewport.View = view;
            return viewport;
        }
    }
}