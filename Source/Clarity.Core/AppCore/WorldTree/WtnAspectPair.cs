using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public struct WtnAspectPair<TAspect>
    {
        public ISceneNode Node;
        public TAspect Aspect;

        public WtnAspectPair(ISceneNode node, TAspect aspect)
        {
            Node = node;
            Aspect = aspect;
        }
    }
}