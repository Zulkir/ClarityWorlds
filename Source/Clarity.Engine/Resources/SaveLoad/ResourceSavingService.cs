using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad 
{
    public class ResourceSavingService : IResourceSavingService
    {
        private readonly IReadOnlyList<IResourceSaver> savers;

        public ResourceSavingService(IReadOnlyList<IResourceSaver> savers)
        {
            this.savers = savers.Reverse().ToArray();
        }

        public string SuggestFileName(IResource resource, string resourceName = null)
        {
            var saver = ChooseSaver(resource);
            return saver.SuggestFileName(resource, resourceName);
        }

        public void SaveResource(IResource resource, IFileSystem fileSystem, string path)
        {
            var saver = ChooseSaver(resource);
            saver.Save(resource, fileSystem, path);
        }

        private IResourceSaver ChooseSaver(IResource resource)
        {
            return savers.FirstOrDefault(x => x.CanSaveNatively(resource)) ??
                   savers.FirstOrDefault(x => x.CanSaveByConversion(resource)) ??
                   throw new NotSupportedException($"Failed to find a saver for resource '{resource}'.");
        }
    }
}