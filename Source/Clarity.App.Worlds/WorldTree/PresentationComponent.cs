﻿using System;
using System.Collections.Generic;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.CopyPaste;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;

namespace Clarity.App.Worlds.WorldTree
{
    // todo: IPresentationComponent
    public abstract class PresentationComponent : SceneNodeComponentBase<PresentationComponent>,
        IGuiComponent, ICopyPasteComponent, IVisualComponent
    {
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly IPresentationGuiCommands commands;
        private readonly IWorldCopyPasteService worldCopyPaste;
        private readonly IViewService viewService;

        private IAppModeService AppModeService => appModeServiceLazy.Value;
        private IList<ISceneNode> Siblings => Node.ParentNode?.ChildNodes;

        protected PresentationComponent(Lazy<IAppModeService> appModeServiceLazy, IPresentationGuiCommands commands, IWorldCopyPasteService worldCopyPaste, IViewService viewService)
        {
            this.appModeServiceLazy = appModeServiceLazy;
            this.commands = commands;
            this.worldCopyPaste = worldCopyPaste;
            this.viewService = viewService;
        }

        public static PresentationComponent Create() => AmFactory.Create<PresentationComponent>();

        public void BuildMenuSection(IGuiMenuBuilder menuBuilder)
        {
            if (AppModeService.Mode != AppMode.Editing)
                return;

            var node = Node;

            if (node.HasComponent<ITransformable3DComponent>())
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.Move);
                menuBuilder.AddCommand(commands.Move3D);
                menuBuilder.AddCommand(commands.Rotate);
                menuBuilder.AddCommand(commands.Scale);
            }
            if (node.HasComponent<IFocusNodeComponent>())
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.FocusView);
            }
            if (node.HasComponent<IRichTextComponent>())
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.SetBorderCurve);
            }

            var cCopyPaste = node.SearchComponent<ICopyPasteComponent>();
            if (cCopyPaste != null)
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.Cut, cCopyPaste.CanExecute(CopyPasteCommand.Cut));
                menuBuilder.AddCommand(commands.Copy, cCopyPaste.CanExecute(CopyPasteCommand.Copy));
                menuBuilder.AddCommand(commands.Duplicate, cCopyPaste.CanExecute(CopyPasteCommand.Duplicate));
                menuBuilder.AddCommand(commands.Paste, cCopyPaste.CanExecute(CopyPasteCommand.Paste));
                menuBuilder.AddCommand(commands.Delete, cCopyPaste.CanExecute(CopyPasteCommand.Delete));
                menuBuilder.StartSection();
                // todo: top, bottom
                menuBuilder.AddCommand(commands.MoveUp, cCopyPaste.CanExecute(CopyPasteCommand.MoveUp));
                menuBuilder.AddCommand(commands.MoveDown, cCopyPaste.CanExecute(CopyPasteCommand.MoveDown));
            }
            
            if (node.HasComponent<ITransformable3DComponent>())
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.MakeScenePortal);
            }
        }

        // CopyPaste
        public bool CanExecute(CopyPasteCommand command)
        {
            switch (command)
            {
                case CopyPasteCommand.Cut:
                case CopyPasteCommand.Copy:
                case CopyPasteCommand.Duplicate:
                case CopyPasteCommand.Delete:
                    return true;
                case CopyPasteCommand.Paste:
                    return worldCopyPaste.Node != null;
                case CopyPasteCommand.MoveTop:
                case CopyPasteCommand.MoveUp:
                    return Siblings?.IndexOf(Node) > 0;
                case CopyPasteCommand.MoveDown:
                case CopyPasteCommand.MoveBottom:
                    var index = Siblings?.IndexOf(Node);
                    return 0 <= index && index < Siblings.Count - 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        public void Execute(CopyPasteCommand command)
        {
            switch (command)
            {
                case CopyPasteCommand.Cut:
                    worldCopyPaste.Node = Node;
                    Node.Deparent();
                    break;
                case CopyPasteCommand.Copy:
                    worldCopyPaste.Node = Node;
                    break;
                case CopyPasteCommand.Duplicate:
                    var nodeCopy = Node.CloneTyped();
                    nodeCopy.Id = 0;
                    Siblings.Insert(Siblings.IndexOf(Node), nodeCopy);
                    break;
                case CopyPasteCommand.Paste:
                    if (worldCopyPaste.Node == null)
                        return;
                    var focusNode = Node.PresentationInfra().ClosestFocusNode;
                    if (focusNode == null)
                        return;
                    var copy = worldCopyPaste.Node.CloneTyped();
                    copy.Id = 0;
                    focusNode.ChildNodes.Add(copy);
                    break;
                case CopyPasteCommand.Delete:
                    Node.Deparent();
                    break;
                case CopyPasteCommand.MoveTop:
                    Node.Deparent();
                    Siblings.Insert(0, Node);
                    break;
                case CopyPasteCommand.MoveUp:
                {
                    var index = Siblings.IndexOf(Node);
                    Node.Deparent();
                    Siblings.Insert(index - 1, Node);
                    break;
                }
                case CopyPasteCommand.MoveDown:
                {
                    var index = Siblings.IndexOf(Node);
                    Node.Deparent();
                    Siblings.Insert(index + 1, Node);
                    break;
                }
                case CopyPasteCommand.MoveBottom:
                    Node.Deparent();
                    Siblings.Add(Node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        public IEnumerable<IVisualElement> GetVisualElements() => EmptyArrays<IVisualElement>.Array;
        public IEnumerable<IVisualEffect> GetVisualEffects() => viewService.SelectedNode == Node
            ? HighlightVisualEffect.Singleton.EnumSelfAs<IVisualEffect>() 
            : EmptyArrays<IVisualEffect>.Array;
    }
}