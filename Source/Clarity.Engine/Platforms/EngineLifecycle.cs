using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.ActiveModel.ClassEmitting;
using Clarity.Common.Infra.Di;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Formats.Json;
using Clarity.Common.Infra.TreeReadWrite.Formats.Xml;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Media.Models.Flexible.Embedded;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Media.Text.Common;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Resources;
using Clarity.Engine.Serialization;
using Clarity.Engine.Special.Sketching;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Platforms
{
    public class EngineLifecycle
    {
        //public void StartAndRun(IEnvironment environment)
        //{
        //    var di = new DiContainer();
        //    BindDefaults(di);
        //    BindExtensions(di, environment);
        //    InitializeStatics(di);
        //    StartupExtensions(di, environment);
        //    StartupCore(di);
        //    Run(di);
        //}

        protected virtual void BindDefaults(IDiContainer di)
        {
            di.Bind<IAmDiBasedObjectFactory>().To(d => new AmDiBasedObjectFactory(d.Get<IReadOnlyList<IAmBindingTypeDescriptor>>(), d.Get));
            //di.Bind<IAmDiBasedObjectFactory>().To(d => new AmJitsuGenDiBasedObjectFactory(d.Get));
            di.BindMulti<IAmBindingTypeDescriptor>().To<AmSingularBindingTypeDescriptor>();
            di.BindMulti<IAmBindingTypeDescriptor>().To<AmListBindingTypeDescriptor>();
            di.Bind<IRenderLoopDispatcher>().To<RenderLoopDispatcher>();
            di.Bind<IRayHitIndex>().To<RayHitIndex>();
            di.Bind<ITemporaryCacheService>().To<TemporaryCacheService>();
            di.Bind<IEmbeddedResources>().To<EmbeddedResources>();
            di.Bind<IEmbeddedResourceFiles>().To<EmbeddedResourceFiles>();
            di.Bind<IInputService>().To<InputService>();
            di.Bind<ITrwFactory>().To<TrwFactory>();
            di.BindMulti<ITrwFormat>().To<TrwFormatJson>();
            di.BindMulti<ITrwFormat>().To<TrwFormatXml>();
            di.Bind<ITrwSerializationHandlerContainer>().To<TrwSerializationHandlerContainer>();
            di.Bind<ITrwAttributeObjectCreator>().To<DiBasedTrwAttributeObjectCreator>();
            di.Bind<ISerializationNecessities>().To<SerializationNecessities>();
            di.BindMulti<IResourceFactory>().To<LineModelFactory>();
            di.BindMulti<IResourceFactory>().To<CircleModelFactory>();
            di.BindMulti<IResourceFactory>().To<CubeModelFactory>();
            di.BindMulti<IResourceFactory>().To<PlaneModelFactory>();
            di.BindMulti<IResourceFactory>().To<Rect3DModelFactory>();
            di.BindMulti<IResourceFactory>().To<SimpleFrustumModelFactory>();
            di.BindMulti<IResourceFactory>().To<SimplePlaneXyModelFactory>();
            di.BindMulti<IResourceFactory>().To<SimplePlaneXzModelFactory>();
            di.BindMulti<IResourceFactory>().To<SphereModelFactory>();
            di.Bind<ISkyboxLoader>().To<SkyboxLoader>();
            di.Bind<ISketchService>().To<SketchService>();
            di.Bind<IFactoryResourceCache>().To<FactoryResourceCache>();
            di.Bind<IRichTextBoxLayoutBuilder>().To<RichTextBoxLayoutBuilder>();
            di.Bind<ITextLineBreaker>().To<TextLineBreaker>();

            di.Bind<IClipboardService>().To<DummyClipboardService>();
        }

        protected virtual void BindExtensions(IDiContainer di, IEnvironment environment)
        {
            foreach (var extension in environment.Extensions)
                extension.Bind(di);
        }

        protected virtual void InitializeStatics(IDiContainer di)
        {
            AmFactory.Initialize(di.Get<IAmDiBasedObjectFactory>());
        }

        protected virtual void StartupExtensions(IDiContainer di, IEnvironment environment)
        {
            foreach (var extension in environment.Extensions)
                extension.OnStartup(di);
        }

        protected virtual void StartupCore(IDiContainer di)
        {
            di.Get<IInputHandler>();
            di.Get<IRenderLoopDispatcher>();
            //di.Get<IAudioSystem>();
        }
    }
}