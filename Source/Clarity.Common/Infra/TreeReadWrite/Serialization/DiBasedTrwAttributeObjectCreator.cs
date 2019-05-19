using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Clarity.Common.Infra.Di;
using JetBrains.Annotations;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class DiBasedTrwAttributeObjectCreator : ITrwAttributeObjectCreator
    {
        private readonly IDiContainer di;

        public DiBasedTrwAttributeObjectCreator(IDiContainer di)
        {
            this.di = di;
        }

        public Func<T> GetConstructor<T>()
        {
            var ctor = ChooseCtor<T>();
            if (ctor == null)
                return () => (T)FormatterServices.GetUninitializedObject(typeof(T));
            var args = ctor.GetParameters().Select(x => x.ParameterType).Select(x => di.Get(x)).ToArray();
            return () => (T)ctor.Invoke(args);
        }

        [CanBeNull]
        private static ConstructorInfo ChooseCtor<T>()
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return ctors.FirstOrDefault(x => x.GetCustomAttribute<TrwSerializeAttribute>() != null);
        }
    }
}