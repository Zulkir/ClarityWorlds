using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel.ClassEmitting;

namespace Clarity.Common.Infra.ActiveModel
{
    public class AmDiBasedObjectFactory : IAmDiBasedObjectFactory
    {
        private readonly IAmObjectClassFactory classFactory;
        private readonly ConcurrentDictionary<Type, IAmObjectInstantiator> instantiators;
        private readonly Func<Type, object> getDependency;

        public AmDiBasedObjectFactory(IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors, Func<Type, object> getDependency)
        {
            this.getDependency = getDependency;
            classFactory = new AmObjectClassFactory(bindingTypeDescriptors);
            instantiators = new ConcurrentDictionary<Type, IAmObjectInstantiator>();
        }

        public bool HasInstantiator(Type objectOriginType) => 
            instantiators.ContainsKey(objectOriginType);

        public TObj Create<TObj>() where TObj : IAmObject => 
            GetInstantiator<TObj>().Instantiate();

        public IAmObjectInstantiator<TObj> GetInstantiator<TObj>() where TObj : IAmObject =>
            (IAmObjectInstantiator<TObj>)GetInstantiator(typeof(TObj));

        public IAmObjectInstantiator GetInstantiator(Type objectType) => 
            instantiators.GetOrAdd(objectType, t => classFactory.CreateObjectClass(t, getDependency));
    }
}