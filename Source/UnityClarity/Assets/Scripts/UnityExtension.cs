using Assets.Scripts.Assets;
using Assets.Scripts.Gui;
using Assets.Scripts.Hacks;
using Assets.Scripts.Rendering;
using Assets.Scripts.Rendering.Materials;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.SaveLoad;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.ActiveModel.JitsuGen;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Gui.MessageBoxes;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;
using JitsuGen.Core;
using JitsuGen.Output;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnityExtension : IExtension
    {
        public string Name => "SystemIntegration.Unity";

        private readonly ProgramObject programObjComponent;

        public UnityExtension(ProgramObject programObjComponent)
        {
            this.programObjComponent = programObjComponent;
        }

        public void Bind(IDiContainer di)
        {
            GenOutput.FillDomain(GenDomain.Static);
            di.Bind<ProgramObject>().To(programObjComponent);
            var eventObject = GameObject.Find("EventObj").GetComponent<EventSender>();
            di.Bind<IEventSender>().To(eventObject);
            di.Bind<IAmDiBasedObjectFactory>().To(d => new AmJitsuGenDiBasedObjectFactory(d.Get));
            di.Bind<IWindowingSystem>().To<UcGui>();
            di.Bind<IGui>().To<UcGui>();
            di.Bind<IRenderService>().To<UcRenderService>();
            di.Bind<IDefaultStateInitializer>().To<UcDefaultStateInitializer>();
            di.Bind<IMoviePlayer>().To<UcCgMoviePlayer>();
            di.Bind<IUcRenderingInfra>().To<UcRenderingInfra>();
            di.Bind<IImageLoader>().To<UcImageLoader>();
            di.BindMulti<IUcInputProvider>().To<UcMouseInputProvider>();
            di.BindMulti<IUcInputProvider>().To<UcKeyboardInputProvider>();
            di.Bind<IStandardMaterialCache>().To<StandardMaterialCache>();
            di.Bind<IRtImageBuilder>().To<UcRtImageBuilder>();
            di.Bind<IRichTextMeasurer>().To<UcRichTextMeasurer>();
            di.Bind<IMessageBoxService>().To<MessageBoxService>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}