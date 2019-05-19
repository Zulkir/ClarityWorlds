using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Resources
{
    public class FactoryResourceCache : IFactoryResourceCache
    {
        private readonly Dictionary<IFactoryResourceSource, IResource> dict;

        public FactoryResourceCache()
        {
            dict = new Dictionary<IFactoryResourceSource, IResource>();
        }

        public IResource GetOrAdd(IFactoryResourceSource source)
        {
            return dict.GetOrAdd(source, x => x.Factory.GetNew(x));
        }

        // todo: are these necessary/usable ?
        public void FreeProbablyUnused(IEnumerable<IScene> scenes)
        {
            var usedResourceSources = new HashSet<IResourceSource>();
            foreach (var scene in scenes)
                AddAllUsedResourceSources(usedResourceSources, scene);
            var deathNode = dict.Keys.Where(key => !usedResourceSources.Contains(key)).ToList();
            foreach (var key in deathNode)
                dict.Remove(key);
        }

        private static void AddAllUsedResourceSources(ISet<IResourceSource> hashSet, IAmObject amObject)
        {
            foreach (var binding in amObject.Bindings)
            {
                var absVal = binding.GetAbstractValue();
                if (absVal is IResource resVal)
                    hashSet.Add(resVal.Source);
                if (absVal is IAmObject amVal)
                    AddAllUsedResourceSources(hashSet, amVal);
                if (absVal is IEnumerable<object> items)
                    foreach (var item in items)
                    {
                        if (item is IResource resItem)
                            hashSet.Add(resItem.Source);
                        if (item is IAmObject amItem)
                            AddAllUsedResourceSources(hashSet, amItem);
                    }
            }
        }

        public void FreeAll()
        {
            dict.Clear();
        }
    }
}