using System;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.Editing;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public class ToolFactory : IToolFactory
    {
        private readonly Lazy<IToolService> toolServiceLazy;
        private readonly Lazy<IViewService> viewServiceLazy;
        private readonly Lazy<IAssetService> assetService;
        private readonly Lazy<IRayHitIndex> worldClickIndex;
        private readonly Lazy<IUndoRedoService> undoRedo;
        private readonly Lazy<IMouseInputProvider> mouseInputProvider;
        private readonly Lazy<ICommonNodeFactory> commonNodeFactoryLazy;
        private readonly Lazy<IAmDiBasedObjectFactory> objectFactoryLazy;
        private readonly Lazy<IStoryService> storyGraphServiceLazy;

        private IToolService ToolService => toolServiceLazy.Value;
        private IViewService ViewService => viewServiceLazy.Value;
        private IAssetService AssetService => assetService.Value;
        private IRayHitIndex RayHitIndex => worldClickIndex.Value;
        private IUndoRedoService UndoRedo => undoRedo.Value;
        private IMouseInputProvider MouseInputProvider => mouseInputProvider.Value;
        private ICommonNodeFactory CommonNodeFactory => commonNodeFactoryLazy.Value;
        private IAmDiBasedObjectFactory ObjectFactory => objectFactoryLazy.Value;
        private IStoryService StoryService => storyGraphServiceLazy.Value;

        public ToolFactory(Lazy<IToolService> toolServiceLazy, Lazy<IViewService> viewServiceLazy, 
            Lazy<IAssetService> assetService, Lazy<IRayHitIndex> worldClickIndex, 
            Lazy<IUndoRedoService> undoRedo, Lazy<IMouseInputProvider> mouseInputProvider, 
            Lazy<ICommonNodeFactory> commonNodeFactoryLazy, Lazy<IAmDiBasedObjectFactory> objectFactoryLazy, 
            Lazy<IStoryService> storyGraphServiceLazy)
        {
            this.toolServiceLazy = toolServiceLazy;
            this.viewServiceLazy = viewServiceLazy;
            this.assetService = assetService;
            this.worldClickIndex = worldClickIndex;
            this.undoRedo = undoRedo;
            this.mouseInputProvider = mouseInputProvider;
            this.commonNodeFactoryLazy = commonNodeFactoryLazy;
            this.objectFactoryLazy = objectFactoryLazy;
            this.storyGraphServiceLazy = storyGraphServiceLazy;
        }

        //public ITool Interact() => new InteractTool(ToolService, this, RayHitIndex, ViewService);
        public ITool MoveEntity(ISceneNode entity, bool isNew) => new MoveEntityTool(entity, isNew, ToolService, UndoRedo);
        public ITool RotateEntity(ISceneNode entity) => new RotateEntityTool(entity, ToolService, UndoRedo);
        public ITool ScaleEntity(ISceneNode entity) => new ScaleEntityTool(entity, ToolService, UndoRedo);

        //public ITool AddCube() => new AddCubeTool(ToolService, ViewService, AssetService, UndoRedo);
        public ITool AddRectangle() => new AddRectangleTool(ToolService, UndoRedo, CommonNodeFactory, ObjectFactory, null, null, false);

        public ITool AddImage(IImage image)
        {
            return new AddRectangleTool(ToolService, UndoRedo, CommonNodeFactory, ObjectFactory, image, null, false);
        }

        public ITool AddMovie(IMovie movie)
        {
            return new AddRectangleTool(ToolService, UndoRedo, CommonNodeFactory, ObjectFactory, null, movie, false);
        }

        public ITool AddText() => new AddRectangleTool(ToolService, UndoRedo, CommonNodeFactory, ObjectFactory, null, null, true);
        public ITool DrawBorderCurve(ISceneNode entity) => new DrawBorderCurveTool(entity, UndoRedo);
        public ITool AddExplicitStoryGraphEdge(int from) => new AddExplicitStoryGraphEdgeTool(from, RayHitIndex, ToolService, StoryService);
        public ITool StoryBranchInto(int from) => new StoryBranchIntoTool(from, RayHitIndex, ToolService, StoryService, CommonNodeFactory);
    }
}