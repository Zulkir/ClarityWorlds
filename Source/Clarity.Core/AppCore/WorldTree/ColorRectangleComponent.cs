using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class ColorRectangleComponent : SceneNodeComponentBase<ColorRectangleComponent>, 
        IVisualComponent
    {
        public abstract Color4 Color { get; set; }

        private readonly IVisualElement visualElement;
        private readonly IPixelSource pixelSource;

        private AaRectangle2 Rectangle => Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2();

        protected ColorRectangleComponent(IEmbeddedResources embeddedResources)
        {
            pixelSource = new SingleColorPixelSourceProxy<ColorRectangleComponent>(this)
            {
                GetColor = x => x.Color,
                GetHasTransparency = x => x.Color.A < 1f
            };
            var material = new StandardMaterialProxy<ColorRectangleComponent>(this)
            {
                GetDiffuseTextureSource = x => x.pixelSource,
                GetIgnoreLighting = x => true,
            };
            visualElement = new CgModelVisualElement<ColorRectangleComponent>(this)
                .SetModel(embeddedResources.SimplePlaneXyModel())
                .SetMaterial(material)
                .SetTransform(x => Transform.Translation(new Vector3(x.Rectangle.Center, 0)))
                .SetNonUniformScale(x => new Vector3(x.Rectangle.HalfWidth, x.Rectangle.HalfHeight, 1));
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            yield return visualElement;
        }
    }
}