using System;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.CopyPaste;
using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class PresentationComponent : SceneNodeComponentBase<PresentationComponent>,
        IGuiComponent
    {
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly IPresentationGuiCommands commands;
        private readonly IWorldCopyPasteService worldCopyPaste;

        private IAppModeService AppModeService => appModeServiceLazy.Value;

        protected PresentationComponent(Lazy<IAppModeService> appModeServiceLazy, IPresentationGuiCommands commands, IWorldCopyPasteService worldCopyPaste)
        {
            this.appModeServiceLazy = appModeServiceLazy;
            this.commands = commands;
            this.worldCopyPaste = worldCopyPaste;
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

            menuBuilder.StartSection();
            menuBuilder.AddCommand(commands.Cut);
            menuBuilder.AddCommand(commands.Copy);
            // todo
            //if (worldCopyPaste.CanPasteTo(node))
            //    menuBuilder.AddCommand(commands.Paste);
            menuBuilder.AddCommand(commands.Duplicate);

            menuBuilder.StartSection();
            var parent = node.ParentNode;
            if (parent != null)
            {
                if (parent.ChildNodes.IndexOf(node) != 0)
                    menuBuilder.AddCommand(commands.MoveUp);
                if (parent.ChildNodes.IndexOf(node) != parent.ChildNodes.Count - 1)
                    menuBuilder.AddCommand(commands.MoveDown);
            }
            
            if (node.HasComponent<ITransformable3DComponent>())
            {
                menuBuilder.StartSection();
                menuBuilder.AddCommand(commands.MakeScenePortal);
            }

            menuBuilder.StartSection();
            menuBuilder.AddCommand(commands.Delete);
        }
    }
}