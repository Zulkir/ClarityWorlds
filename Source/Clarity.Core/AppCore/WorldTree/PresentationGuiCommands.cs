using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.CopyPaste;
using Clarity.Core.AppCore.Interaction.Manipulation3D;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.Tools;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.Views;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Core.AppCore.WorldTree
{
    public class PresentationGuiCommands : IPresentationGuiCommands
    {
        private readonly IAmDiBasedObjectFactory objectFactory;
        private readonly Lazy<IViewService> viewServiceLazy;
        private readonly Lazy<IWorldCopyPasteService> copyPasteServiceLazy;
        private readonly IToolService toolService;
        private readonly IToolFactory toolFactory;
        private readonly IUndoRedoService undoRedoService;

        private IViewService ViewService => viewServiceLazy.Value;
        private IWorldCopyPasteService WorldCopyPaste => copyPasteServiceLazy.Value;

        //public Command LayoutHere { get; }
        public IGuiCommand Move { get; }
        public IGuiCommand Move3D { get; }
        public IGuiCommand Rotate { get; }
        public IGuiCommand Scale { get; }
        public IGuiCommand Cut { get; }
        public IGuiCommand Copy { get; }
        public IGuiCommand Paste { get; }
        public IGuiCommand Duplicate { get; }
        public IGuiCommand Delete { get; }
        public IGuiCommand FocusView { get; }
        public IGuiCommand AddNewAdaptiveLayout { get; }
        public IGuiCommand MoveUp { get; }
        public IGuiCommand MoveDown { get; }
        public IGuiCommand SetBorderCurve { get; }
        public IGuiCommand MakeScenePortal { get; }

        public PresentationGuiCommands(Lazy<IViewService> viewServiceLazy,
            Lazy<IWorldCopyPasteService> copyPasteServiceLazy,
            IToolService toolService, IToolFactory toolFactory, IUndoRedoService undoRedoService, IAmDiBasedObjectFactory objectFactory)
        {
            this.copyPasteServiceLazy = copyPasteServiceLazy;
            this.toolService = toolService;
            this.toolFactory = toolFactory;
            this.undoRedoService = undoRedoService;
            this.objectFactory = objectFactory;
            this.viewServiceLazy = viewServiceLazy;

            //LayoutHere = GuiCommandsHelper.Create("Layout Here", ExecLayoutHere);
            Move = new GuiCommand("Move", KeyModifyers.Control, Key.T, ExecMove);
            Move3D = new GuiCommand("Move3D", KeyModifyers.Control | KeyModifyers.Shift , Key.T, ExecMove3D);
            Rotate = new GuiCommand("Rotate", KeyModifyers.Control, Key.R, ExecRotate);
            Scale = new GuiCommand("Scale", KeyModifyers.Control, Key.E, ExecScale);
            Cut = new GuiCommand("Cut", KeyModifyers.Control, Key.X, ExecCut);
            Copy = new GuiCommand("Copy", KeyModifyers.Control, Key.C, ExecCopy);
            Paste = new GuiCommand("Paste", KeyModifyers.Control, Key.V, ExecPaste);
            Duplicate = new GuiCommand("Duplicate", KeyModifyers.Control, Key.D, ExecDuplicate);
            Delete = new GuiCommand("Delete", Key.Delete, ExecDelete);
            FocusView = new GuiCommand("Focus View", KeyModifyers.Control, Key.Enter, ExecFocusView);
            AddNewAdaptiveLayout = new GuiCommand("Adaptive Layout", ExecNewAdaptiveLayout);
            MoveUp = new GuiCommand("Move Up", KeyModifyers.Control, Key.Up, ExecMoveUp);
            MoveDown = new GuiCommand("Move Down", KeyModifyers.Control, Key.Down, ExecMoveDown);
            SetBorderCurve = new GuiCommand("Set Border Curve", ExecSetBorderCurve);
            MakeScenePortal = new GuiCommand("Make Scene Portal", ExecMakeScenePortal);
        }

        private IGuiCommand Create(string text, Action action, KeyModifyers shortcutModifiers, Key shortcutKey)
        {
            return new GuiCommand(text, shortcutModifiers, shortcutKey, action);
        }

        /*
        private void ExecLayoutHere(object sender, EventArgs args)
        {
            var focusedLayout = ViewService.Viewport.FocusedLayout;
            var newLayout = new ScreenPlaneLayout();
            var parentGlobalTransform = focusedLayout.ChildPlacementComponent.GetParentGlobalTransformForNewChild3D();
            var globalFrame = ViewService.Viewport.Camera.GetGlobalFrame();
            var scale = parentGlobalTransform.Scale;
            var rotation = Quaternion.RotationToFrame(globalFrame.Right, globalFrame.Forward);
            var pos = globalFrame.Eye;
            var globalTransform = new Transform(scale, rotation, pos);
            var localTransform = globalTransform * parentGlobalTransform.Invert();
            newLayout.Name = "FloatingLayout";
            newLayout.LocalTransform = localTransform;
            focusedLayout.ChildNodes.Add(newLayout);
        }*/

        private void ExecMove()
        {
            var entity = ViewService.SelectedNode;
            if (!entity?.HasComponent<ITransformable3DComponent>() ?? false)
                return;
            var tool = toolFactory.MoveEntity(entity, false);
            toolService.CurrentTool = tool;
        }

        private void ExecMove3D()
        {
            throw new NotImplementedException();
            var entity = ViewService.SelectedNode;
            if (!entity?.HasComponent<ITransformable3DComponent>() ?? false)
                return;
            var gizmo = AmFactory.Create<SceneNode>();
            var gizmoComponent = AmFactory.Create<Translate3DGizmoComponent>();
            //var auxComponent = AmFactory.Create<AuxiliaryNodeComponent>();
            //auxComponent.SuppoesedParent = entity;
            //gizmo.Components.Add(auxComponent);
            gizmo.Components.Add(gizmoComponent);
            entity.ChildNodes.Add(gizmo);
            //WorldTreeService.AuxiliaryNodes.Add(gizmo);
        }

        private void ExecRotate()
        {
            var entity = ViewService.SelectedNode;
            if (!entity?.HasComponent<ITransformable3DComponent>() ?? false)
                return;
            var tool = toolFactory.RotateEntity(entity);
            toolService.CurrentTool = tool;
        }

        private void ExecScale()
        {
            var entity = ViewService.SelectedNode;
            if (!entity?.HasComponent<ITransformable3DComponent>() ?? false)
                return;
            var tool = toolFactory.ScaleEntity(entity);
            toolService.CurrentTool = tool;
        }

        private void ExecCut()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.Cut(node);
        }

        private void ExecCopy()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.Copy(node);
        }

        private void ExecDuplicate()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.Duplicate(node);
            ViewService.SelectedNode = node.ParentNode.ChildNodes[node.ParentNode.ChildNodes.IndexOf(node) + 1];
        }

        private void ExecPaste()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.PasteTo(node);
            ViewService.SelectedNode = node;
        }

        private void ExecDelete()
        {
            var node = ViewService.SelectedNode;
            var parent = node.ParentNode;
            var index = Math.Max(node.ParentNode.ChildNodes.IndexOf(node) - 1, 0);

            undoRedoService.Common.Remove(parent.ChildNodes, node);
            ViewService.SelectedNode = parent.ChildNodes.Count > 0 ? parent.ChildNodes[index] : parent;
        }

        private void ExecFocusView()
        {
            ViewService.MainView.FocusOn(ViewService.SelectedNode.PresentationInfra().ClosestFocusNode.GetComponent<IFocusNodeComponent>());
        }

        private void ExecNewAdaptiveLayout()
        {
            var newNode = objectFactory.Create<SceneNode>();
            newNode.Name = "AbstractNode";
            newNode.Components.Add(objectFactory.Create<StoryComponent>());
            undoRedoService.Common.Add(ViewService.SelectedNode.ChildNodes, newNode);
        }

        private void ExecMoveUp()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.MoveUp(node);
            ViewService.SelectedNode = node;
        }

        private void ExecMoveDown()
        {
            var node = ViewService.SelectedNode;
            WorldCopyPaste.MoveDown(node);
            ViewService.SelectedNode = node;
        }

        private void ExecSetBorderCurve()
        {
            var node = ViewService.SelectedNode;
            if (node != null)
                toolService.CurrentTool = toolFactory.DrawBorderCurve(node);
        }

        private void ExecMakeScenePortal()
        {
            throw new NotImplementedException();
            //var node = SceneTreeGui.SelectedNode;
            //if (node == null)
            //    return;
            //var sceneComponent = objectFactory.Create<SceneComponent>();
            //undoRedoService.Common.Add(node.Components, sceneComponent);
        }
    }
}