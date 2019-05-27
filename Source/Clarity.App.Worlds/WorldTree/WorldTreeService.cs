using System;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.WorldTree
{
    public class WorldTreeService : IWorldTreeService
    {
        private readonly IEventRoutingService eventRoutingService;
        private readonly IPresentationWorldBuilder presentationWorldBuilder;
        
        private WorldHolder WorldHolder { get; }

        public IWorld ParentWorld { get; set; }
        public IWorld World { get => WorldHolder.World; set => WorldHolder.World = value; }

        // todo: make these two main props
        public IWorld EditingWorld => ParentWorld ?? World;
        public IWorld PresentationWorld => ParentWorld == null ? null : World;

        public ISceneNode MainRoot => World.Scenes.FirstOrDefault()?.Root;

        public WorldTreeService(IEventRoutingService eventRoutingService, IPresentationWorldBuilder presentationWorldBuilder)
        {
            this.eventRoutingService = eventRoutingService;
            this.presentationWorldBuilder = presentationWorldBuilder;
            WorldHolder = AmFactory.Create<WorldHolder>();
            WorldHolder.World = AmFactory.Create<World>();
            WorldHolder.World.Scenes.Add(AmFactory.Create<Scene>());
            WorldHolder.Updated += OnWorldUpdated;
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IWorldTreeService), nameof(OnNewFrame), OnNewFrame);
            eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(IWorldTreeService), nameof(OnAppModeChanged), OnAppModeChanged);
            eventRoutingService.SubscribeToAllAfter($"{typeof(IWorldTreeService).FullName}.{nameof(OnRoutedEvent)}", OnRoutedEvent, true);
        }

        private void OnNewFrame(INewFrameEvent newFrameEvent)
        {
            World.Update(newFrameEvent.FrameTime);
        }

        private void OnAppModeChanged(IAppModeChangedEvent appModeChangedEvent)
        {
            switch (appModeChangedEvent.NewAppMode)
            {
                case AppMode.Editing:
                    WorldHolder.World = ParentWorld;
                    ParentWorld = null;
                    break;
                case AppMode.Presentation:
                    ParentWorld = WorldHolder.World;
                    WorldHolder.World = presentationWorldBuilder.BuildPreesentationWorld(ParentWorld);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRoutedEvent(IRoutedEvent evnt)
        {
            World.OnRoutedEvent(evnt);
        }

        private void OnWorldUpdated(IAmEventMessage message)
        {
            eventRoutingService.FireEvent<IWorldTreeUpdatedEvent>(new WorldTreeUpdatedEvent(message));
        }

        public ISceneNode GetById(int id) => World.GetNodeById(id);
        public bool TryGetById(int id, out ISceneNode node) => World.TryGetNodeById(id, out node);
    }
}