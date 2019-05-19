using System;
using System.Collections.Concurrent;
using System.Linq;
using JitsuGen.Core;

namespace Clarity.Common.Infra.ActiveModel.JitsuGen
{
    public class AmJitsuGenDiBasedObjectFactory : IAmDiBasedObjectFactory
    {
        private readonly IGenDomain genDomain;
        private readonly ConcurrentDictionary<Type, IAmObjectInstantiator> instantiators;
        private readonly Func<Type, object> getDependency;

        public AmJitsuGenDiBasedObjectFactory(Func<Type, object> getDependency)
        {
            this.getDependency = getDependency;
            genDomain = GenDomain.Static;
            instantiators = new ConcurrentDictionary<Type, IAmObjectInstantiator>();
        }

        public bool HasInstantiator(Type objectOriginType) =>
            instantiators.ContainsKey(objectOriginType);

        public TObj Create<TObj>() where TObj : IAmObject =>
            GetInstantiator<TObj>().Instantiate();

        public IAmObjectInstantiator<TObj> GetInstantiator<TObj>() where TObj : IAmObject =>
            (IAmObjectInstantiator<TObj>)GetInstantiator(typeof(TObj));

        public IAmObjectInstantiator GetInstantiator(Type objectType) =>
            instantiators.GetOrAdd(objectType, t => CreateInstantiator(genDomain.GetGeneratedType(t)));

        private IAmObjectInstantiator CreateInstantiator(Type type)
        {
            var instantiatorType = typeof(AmObjectInstantiator<>).MakeGenericType(type);
            return (IAmObjectInstantiator)instantiatorType.GetConstructors().Single().Invoke(new object[] { getDependency });
        }
    }
}