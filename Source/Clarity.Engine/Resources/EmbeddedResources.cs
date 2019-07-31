using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Exceptions;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Media.Models.Explicit.Embedded;
using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Engine.Resources
{
    public class EmbeddedResources : IEmbeddedResources
    {
        private readonly CircleModelFactory circleModelFactory;
        private readonly RectangleModelFactory rectangleModelFactory;
        private readonly CubeModelFactory cubeModelFactory;
        private readonly LineModelFactory lineModelFactory;
        private readonly PlaneModelFactory planeModelFactory;
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
            rectangleModelFactory = resourceFactories.OfType<RectangleModelFactory>().First();
            cubeModelFactory = resourceFactories.OfType<CubeModelFactory>().First();
            lineModelFactory = resourceFactories.OfType<LineModelFactory>().First();
            planeModelFactory = resourceFactories.OfType<PlaneModelFactory>().First();
            simpleFrustumModelFactory = resourceFactories.OfType<SimpleFrustumModelFactory>().First();
            simplePlaneXyModelFactory = resourceFactories.OfType<SimplePlaneXyModelFactory>().First();
            simplePlaneXzModelFactory = resourceFactories.OfType<SimplePlaneXzModelFactory>().First();
            sphereModelFactory = resourceFactories.OfType<SphereModelFactory>().First();
        }

        public IExplicitModel LineModel() =>
            (IExplicitModel)lineModelFactory.GetModelSource().GetResource();

        public IExplicitModel CircleModel(int numSegments) =>
            (IExplicitModel)circleModelFactory.GetModelSource(numSegments).GetResource();

        public IExplicitModel RectangleModel() =>
            (IExplicitModel)rectangleModelFactory.GetModelSource().GetResource();

        public IExplicitModel CubeModel() =>
            (IExplicitModel)cubeModelFactory.GetModelSource().GetResource();

        public IExplicitModel PlaneModel(PlaneModelSourcePlane plane, PlaneModelSourceNormalDirection normalDirection,
                                   float halfWidth, float halfHeight, float scaleU, float scaleV) =>
            (IExplicitModel)planeModelFactory.GetModelSource(plane, normalDirection, halfWidth, halfHeight, scaleU, scaleV).GetResource();

        public IExplicitModel SimpleFrustumModel() =>
            (IExplicitModel)simpleFrustumModelFactory.GetModelSource().GetResource();

        public IExplicitModel SimplePlaneXyModel() =>
            (IExplicitModel)simplePlaneXyModelFactory.GetModelSource().GetResource();

        public IExplicitModel SimplePlaneXzModel() =>
            (IExplicitModel)simplePlaneXzModelFactory.GetModelSource().GetResource();

        public IExplicitModel SphereModel(int halfNumCircleSegments, bool inverse) =>
            (IExplicitModel)sphereModelFactory.GetModelSource(halfNumCircleSegments, inverse).GetResource();

        public IImage Image(string path)
        {
            using (var stream = embeddedResourceFiles.FileSystem.OpenRead(path)) 
                return imageLoader.TryLoad(stream, out var image, out var error) 
                    ? image.WithSource(new EmbeddedResourceSource(this, EmbeddedResourceType.Image, path)) 
                    : throw new DataLoadException(error);
        }

        public ISkybox Skybox(string path)
        {
            return skyboxLoader.TryLoad(embeddedResourceFiles.FileSystem, path, out var skybox, out _, out var error)
                ? skybox.WithSource(new EmbeddedResourceSource(this, EmbeddedResourceType.Skybox, path))
                : throw new DataLoadException(error);
        }
    }
}