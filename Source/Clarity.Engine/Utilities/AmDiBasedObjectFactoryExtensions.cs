using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Utilities
{
    public static class AmDiBasedObjectFactoryExtensions
    {
        public static ISceneNode CreateWorldNodeWithComponent<TComponent>(this IAmDiBasedObjectFactory factory, out TComponent component) 
            where TComponent : ISceneNodeComponent
        {
            var node = factory.Create<SceneNode>();
            component = factory.Create<TComponent>();
            node.Components.Add(component);
            return node;
        }
    }
}