using System;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.Core.AppCore.WorldTree
{
    public class WorldTreeService : IWorldTreeService
    {
        private WorldHolder WorldHolder { get; }
        public IScene ParentScene { get; set; }

        public IWorld ParentWorld { get; set; }
        public IWorld World { get => WorldHolder.World; set => WorldHolder.World = value; }
        public ISceneNode MainRoot => World.Scenes.FirstOrDefault()?.Root;

        public event Action<IAmEventMessage> UpdatedHigh;
        public event Action<IAmEventMessage> UpdatedMed;

        public WorldTreeService(IAmDiBasedObjectFactory objectFactory, IRenderLoopDispatcher renderLoopDispatcher)
        {
            WorldHolder = AmFactory.Create<WorldHolder>();
            WorldHolder.World = objectFactory.Create<World>();
            WorldHolder.World.Scenes.Add(objectFactory.Create<Scene>());
            WorldHolder.Updated += OnSceneUpdated;
            renderLoopDispatcher.Update += OnUpdate;
        }

        private void OnUpdate(FrameTime frameTime)
        {
            World.Update(frameTime);
        }

        private void OnSceneUpdated(IAmEventMessage message)
        {
            UpdatedHigh?.Invoke(message);
            UpdatedMed?.Invoke(message);
        }

        public ISceneNode GetById(int id) => World.GetNodeById(id);
        public bool TryGetById(int id, out ISceneNode node) => World.TryGetNodeById(id, out node);
    }
}