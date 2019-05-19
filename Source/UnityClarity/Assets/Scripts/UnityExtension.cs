using Assets.Scripts.Assets;
using Assets.Scripts.Gui;
using Assets.Scripts.Hacks;
using Assets.Scripts.Rendering;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.ActiveModel.JitsuGen;
using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.Gui;
using Clarity.Core.AppCore.SaveLoad;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
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
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}