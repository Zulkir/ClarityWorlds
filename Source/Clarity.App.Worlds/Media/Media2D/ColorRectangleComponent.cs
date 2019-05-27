using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Worlds.Media.Media2D
{
    public abstract class ColorRectangleComponent : SceneNodeComponentBase<ColorRectangleComponent>, 
        IVisualComponent
    {
        public abstract Color4 Color { get; set; }

        private readonly IVisualElement visualElement;

        private AaRectangle2 Rectangle => Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2();

        protected ColorRectangleComponent(IEmbeddedResources embeddedResources)
        {
            var material = StandardMaterial.New(this)
                .SetDiffuseColor(x => x.Color)
                .SetIgnoreLighting(true);
            visualElement = new ModelVisualElement<ColorRectangleComponent>(this)
                .SetModel(embeddedResources.SimplePlaneXyModel())
                .SetMaterial(material)
                .SetTransform(x => Transform.Translation(new Vector3(x.Rectangle.Center, 0)))
                .SetNonUniformScale(x => new Vector3(x.Rectangle.HalfWidth, x.Rectangle.HalfHeight, 1));
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            yield return visualElement;
        }

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;
    }
}