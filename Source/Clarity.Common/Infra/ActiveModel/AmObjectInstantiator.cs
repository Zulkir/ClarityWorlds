using System;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel
{
    public class AmObjectInstantiator<T> : IAmObjectInstantiator<T>
        where T : IAmObject
    {
        public Type Class => typeof(T);
        private ConstructorInfo Constructor { get; }
        private object[] DefaultArguments { get; }

        public AmObjectInstantiator(Func<Type, object> getDefaultDependency)
        {
            Constructor = Class.GetConstructors().Single();
            DefaultArguments = BuildArguments(getDefaultDependency);
        }

        public T Instantiate(Func<Type, object> getDependency)
        {
            var args = getDependency != null 
                ? BuildArguments(getDependency) 
                : DefaultArguments;
            var instance = (T)Constructor.Invoke(args);
            instance.InternalInitInstantiator(this);
            return instance;
        }

        IAmObject IAmObjectInstantiator.Instantiate(Func<Type, object> getDependency) => 
            Instantiate(getDependency);

        private object[] BuildArguments(Func<Type, object> getDependency)
        {
            var dependencies = Constructor.GetParameters().Select(x => getDependency(x.ParameterType));
            return dependencies.ToArray();
        }
    }
}