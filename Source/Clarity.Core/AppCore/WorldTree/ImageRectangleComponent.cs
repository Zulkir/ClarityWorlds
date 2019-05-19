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
    public abstract class ImageRectangleComponent : SceneNodeComponentBase<ImageRectangleComponent>,
        IVisualComponent
    {
        public abstract IImage Image { get; set; }

        private readonly IVisualElement visualElement;
        private readonly IPixelSource fallbackPixelSource;

        private AaRectangle2 Rectangle => Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2();

        protected ImageRectangleComponent(IEmbeddedResources embeddedResources)
        {
            fallbackPixelSource = new SingleColorPixelSource(Color4.Black);
            var material = new StandardMaterialProxy<ImageRectangleComponent>(this)
            {
                GetDiffuseTextureSource = x => x.Image ?? x.fallbackPixelSource,
                GetIgnoreLighting = x => true,
            };
            visualElement = new CgModelVisualElement<ImageRectangleComponent>(this)
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