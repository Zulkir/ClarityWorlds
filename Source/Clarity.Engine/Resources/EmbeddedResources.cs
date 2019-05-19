using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Models.Flexible.Embedded;
using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Engine.Resources
{
    public class EmbeddedResources : IEmbeddedResources
    {
        private readonly CircleModelFactory circleModelFactory;
        private readonly CubeModelFactory cubeModelFactory;
        private readonly LineModelFactory lineModelFactory;
        private readonly PlaneModelFactory planeModelFactory;
        private readonly Rect3DModelFactory rect3DModelFactory;
        private readonly SimpleFrustumModelFactory simpleFrustumModelFactory;
        private readonly SimplePlaneXyModelFactory simplePlaneXyModelFactory;
        private readonly SimplePlaneXzModelFactory simplePlaneXzModelFactory;
        private readonly SphereModelFactory sphereModelFactory;

        private readonly IEmbeddedResourceFiles embeddedResourceFiles;
        private readonly IImageLoader imageLoader;
        private readonly ISkyboxLoader skyboxLoader;

        public EmbeddedResources(IReadOnlyList<IResourceFactory> resourceFactories, IImageLoader imageLoader, ISkyboxLoader skyboxLoader, IEmbeddedResourceFiles embeddedResourceFiles)
        {
            this.imageLoader = imageLoader;
            this.skyboxLoader = skyboxLoader;
            this.embeddedResourceFiles = embeddedResourceFiles;
            circleModelFactory = resourceFactories.OfType<CircleModelFactory>().First();
            cubeModelFactory = resourceFactories.OfType<CubeModelFactory>().First();
            lineModelFactory = resourceFactories.OfType<LineModelFactory>().First();
            planeModelFactory = resourceFactories.OfType<PlaneModelFactory>().First();
            rect3DModelFactory = resourceFactories.OfType<Rect3DModelFactory>().First();
            simpleFrustumModelFactory = resourceFactories.OfType<SimpleFrustumModelFactory>().First();
            simplePlaneXyModelFactory = resourceFactories.OfType<SimplePlaneXyModelFactory>().First();
            simplePlaneXzModelFactory = resourceFactories.OfType<SimplePlaneXzModelFactory>().First();
            sphereModelFactory = resourceFactories.OfType<SphereModelFactory>().First();
        }

        public IFlexibleModel LineModel() =>
            (IFlexibleModel)lineModelFactory.GetModelSource().GetResource();

        public IFlexibleModel CircleModel(int numSegments) =>
            (IFlexibleModel)circleModelFactory.GetModelSource(numSegments).GetResource();

        public IFlexibleModel CubeModel() =>
            (IFlexibleModel)cubeModelFactory.GetModelSource().GetResource();

        public IFlexibleModel PlaneModel(PlaneModelSourcePlane plane, PlaneModelSourceNormalDirection normalDirection,
                                   float halfWidth, float halfHeight, float scaleU, float scaleV) =>
            (IFlexibleModel)planeModelFactory.GetModelSource(plane, normalDirection, halfWidth, halfHeight, scaleU, scaleV).GetResource();

        public IFlexibleModel Rect3DModel() =>
            (IFlexibleModel)rect3DModelFactory.GetModelSource().GetResource();

        public IFlexibleModel SimpleFrustumModel() =>
            (IFlexibleModel)simpleFrustumModelFactory.GetModelSource().GetResource();

        public IFlexibleModel SimplePlaneXyModel() =>
            (IFlexibleModel)simplePlaneXyModelFactory.GetModelSource().GetResource();

        public IFlexibleModel SimplePlaneXzModel() =>
            (IFlexibleModel)simplePlaneXzModelFactory.GetModelSource().GetResource();

        public IFlexibleModel SphereModel(int halfNumCircleSegments, bool inverse) =>
            (IFlexibleModel)sphereModelFactory.GetModelSource(halfNumCircleSegments, inverse).GetResource();

        public IImage Image(string path)
        {
            using (var stream = embeddedResourceFiles.FileSystem.OpenRead(path))
                return imageLoader.Load(stream)
                    .WithSource(new EmbeddedResourceSource(this, EmbeddedResourceType.Image, path));
        }

        public ISkybox Skybox(string path)
        {
            return skyboxLoader.Load(embeddedResourceFiles.FileSystem, path, out _)
                .WithSource(new EmbeddedResourceSource(this, EmbeddedResourceType.Skybox, path));
        }
    }
}