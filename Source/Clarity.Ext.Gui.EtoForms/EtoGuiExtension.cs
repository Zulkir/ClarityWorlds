﻿using System;
using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.Gui;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;
using Clarity.Ext.Gui.EtoForms.AppModes;
using Clarity.Ext.Gui.EtoForms.Common;
using Clarity.Ext.Gui.EtoForms.Props;
using Clarity.Ext.Gui.EtoForms.ResourceExplorer;
using Clarity.Ext.Gui.EtoForms.SaveLoad;
using Clarity.Ext.Gui.EtoForms.SceneTree;
using Clarity.Ext.Gui.EtoForms.StoryGraph;
using Clarity.Ext.Gui.EtoForms.Text;
using Eto;

namespace Clarity.Ext.Gui.EtoForms
{
    public class EtoGuiExtension : IExtension
    {
        public string Name => "Gui.EtoForms";

        public void Bind(IDiContainer di)
        {
            di.Bind<IWindowingSystem>().To<GuiEto>();
            di.Bind<IGui>().To<GuiEto>();
            if (EtoEnvironment.Platform.IsWindows)
            {
                di.Bind<Platform>().To<Eto.WinForms.Platform>();
                di.Bind<Eto.Forms.Application.IHandler>().To<LoopAppHandlerWinForms>();
                di.Bind<RenderControl.IHandler>().To<RenderingAreaHandlerWinFormsOgl>();
            }
            else if (EtoEnvironment.Platform.IsLinux)
            {
                di.Bind<Platform>().To<Eto.GtkSharp.Platform>();
                di.Bind<Eto.Forms.Application.IHandler>().To<LoopAppHandlerGtk>();
                di.Bind<RenderControl.IHandler>().To<RenderingAreaHandlerGtkOgl>();
            }
            else
            {
                throw new NotSupportedException("This platform is not supported.");
            }

            di.Bind<ICommonGuiObjects>().To<CommonGuiObjects>();
            di.Bind<INameGenerator>().To<NameGenerator>();
            di.Bind<ISceneTreeGui>().To<SceneTreeGui>();
            di.Bind<IResourceExplorerGui>().To<ResourceExplorerGui>();
            di.Bind<ISaveLoadGuiCommands>().To<SaveLoadGuiCommands>();
            di.Bind<IPropsGui>().To<PropsGui>();
            di.Bind<RenderControl>().To<RenderControl>();
            di.Bind<IRtImageBuilder>().To<RtImageBuilder>();
            di.Bind<IClipboardService>().To<ClipboardService>();
            di.Bind<IFontFamilyCache>().To<FontFamilyCache>();
            di.Bind<IRichTextMeasurer>().To<RichTextMeasurer>();
            di.Bind<IAppModesCommands>().To<AppModesCommands>();
            di.Bind<IStoryGraphGui>().To<StoryGraphGui>();
            di.Bind<IFrameTimeMeasurer>().To<FrameTimeMeasurer>();
            di.Bind<IMainForm>().To<DefaultMainForm>();
            di.Bind<IMouseInputProvider>().To<MouseInputProvider>();
            di.Bind<IKeyboardInputProvider>().To<KeyboardInputProvider>();
        }

        public void OnStartup(IDiContainer di)
        {
            di.Get<IGui>();
            di.Get<IMouseInputProvider>();
            di.Get<IKeyboardInputProvider>();
        }
    }
}