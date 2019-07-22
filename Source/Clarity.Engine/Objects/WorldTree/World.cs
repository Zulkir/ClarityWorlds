using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree
{
    public abstract class World : AmObjectBase<World>, IWorld
    {
        public abstract IList<IScene> Scenes { get; }
        public abstract IList<string> Tags { get; }
        public abstract IPropertyBag Properties { get; set; }
        public abstract int NextId { get; set; }

        private readonly Dictionary<int, ISceneNode> idIndex;

        protected World()
        {
            idIndex = new Dictionary<int, ISceneNode>();
            Properties = new PropertyBag();
            NextId = 1;
        }

        public ISceneNode GetNodeById(int id) => idIndex[id];
        public bool TryGetNodeById(int id, out ISceneNode node) => idIndex.TryGetValue(id, out node);

        public void Update(FrameTime frameTime)
        {
            foreach (var scene in Scenes)
                scene.Update(frameTime);
        }

        public void OnRoutedEvent(IRoutedEvent evnt)
        {
            foreach (var scene in Scenes)
                scene.OnRoutedEvent(evnt);
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            if (message.Obj<IWorld>().ItemAddedOrRemoved(x => x.Scenes, out _) ||
                message.Obj<ISceneNode>().ItemAddedOrRemoved(x => x.ChildNodes, out _) ||
                message.Obj<IScene>().ValueChanged(x => x.Root, out _))
                UpdateIds();
        }

        private void UpdateIds()
        {
            idIndex.Clear();
            foreach (var scene in Scenes)
                if (scene.Root != null)
                    UpdateIdsForSubtree(scene.Root);
        }

        private void UpdateIdsForSubtree(ISceneNode subtreeRoot)
        {
            if (subtreeRoot.Id == 0)
                subtreeRoot.Id = NextId++;
            else if (subtreeRoot.Id > NextId)
                NextId = subtreeRoot.Id + 1;
            idIndex.Add(subtreeRoot.Id, subtreeRoot);
            foreach (var child in subtreeRoot.ChildNodes)
                UpdateIdsForSubtree(child);
        }
    }
}