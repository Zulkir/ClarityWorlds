using Assets.Scripts.Assets;
using Assets.Scripts.Gui;
using Assets.Scripts.Hacks;
using Assets.Scripts.Infra;
using Assets.Scripts.Interaction;
using Assets.Scripts.Interaction.Minimap;
using Assets.Scripts.Interaction.MoveInPlace;
using Assets.Scripts.Rendering;
using Assets.Scripts.Rendering.Materials;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Logging;
using Clarity.App.Worlds.SaveLoad;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.ActiveModel.JitsuGen;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Gui.MessageBoxes;
using Clarity.Engine.Gui.MessagePopups;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;
using JitsuGen.Core;
using JitsuGen.Output;

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
            di.Bind<IVrHeadPositionService>().To<VrHeadPositionService>();
            di.Bind<IStandardMaterialCache>().To<StandardMaterialCache>();
            di.Bind<IRtImageBuilder>().To<UcRtImageBuilder>();
            di.Bind<IRichTextMeasurer>().To<UcRichTextMeasurer>();
            di.Bind<IMessageBoxService>().To<MessageBoxService>();
            di.Bind<IVrInitializerService>().To<VrInitializerService>();
            di.Bind<IGlobalObjectService>().To<GlobalObjectService>();
            di.Bind<IVrManipulationService>().To<VrManipulationService>();
            di.Bind<ILogService>().To<UcLogService>();
            di.Bind<IMessagePopupService>().To<UcMessagePopupService>();
            di.Bind<IVrInputDispatcher>().To<NoviceVrInputDispatcher>();
            di.BindMulti<IVrNavigationMode>().To<MinimapVrNavigationMode>();
            //di.BindMulti<IVrNavigationMode>().To<FixedTeleportVrNavigationMode>();
            //di.BindMulti<IVrNavigationMode>().To<FixedSmoothVrNavigationMode>();
            di.BindMulti<IVrNavigationMode>().To<FreeSmoothVrNavigationMode>();
            di.BindMulti<IVrNavigationMode>().To<MoveInPlaceVrNavigationMode>();
            di.BindMulti<IVrNavigationMode>().To<FreeTeleportVrNavigationMode>();
        }

        public void OnStartup(IDiContainer di)
        {
            di.Get<IVrManipulationService>();
        }
    }
}