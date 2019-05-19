using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AmReferenceLoadContext : IDisposable
    {
        private readonly Dictionary<int, IAmObject> objects;
        private readonly Dictionary<int, List<IAmSingularBinding>> singularBindings;
        private readonly Dictionary<IAmListBinding, List<int>> listBindings;

        public IAmBinding RefBinding { get; set; }

        public AmReferenceLoadContext()
        {
            objects = new Dictionary<int, IAmObject>();
            singularBindings = new Dictionary<int, List<IAmSingularBinding>>();
            listBindings = new Dictionary<IAmListBinding, List<int>>();
        }

        public void Dispose()
        {
            foreach (var kvp in listBindings)
            {
                var binding = kvp.Key;
                var refIdList = kvp.Value;
                binding.Clear();
                foreach (var refId in refIdList)
                {
                    // todo: make work and removel
                    try
                    {
                        var obj = objects[refId];
                        binding.AddAbstractItem(obj);
                    }
                    catch(Exception ex) { }
                }
            }
        }

        public void OnNewObject(int referenceId, IAmObject obj)
        {
            objects.Add(referenceId, obj);
            var bindingList = singularBindings.TryGetRef(referenceId);
            if (bindingList == null)
                return;
            foreach (var binding in bindingList)
                binding.SetAbstractValue(obj);
        }

        public void OnLoadReference(int refId)
        {
            if (RefBinding is IAmSingularBinding sBinding)
            {
                if (objects.TryGetValue(refId, out var obj))
                {
                    RefBinding.SetAbstractValue(obj);
                }
                else
                {
                    var list = singularBindings.GetOrAdd(refId, x => new List<IAmSingularBinding>());
                    list.Add(sBinding);
                }
            }
            else if (RefBinding is IAmListBinding lBinding)
            {
                var list = listBindings.GetOrAdd(lBinding, x => new List<int>());
                list.Add(refId);
            }
            else
            {
                throw new Exception("Unexpected binding type.");
            }
        }
    }
}