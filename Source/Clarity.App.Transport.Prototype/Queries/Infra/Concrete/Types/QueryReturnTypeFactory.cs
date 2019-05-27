using System;
using System.Collections.Concurrent;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types
{
    public static class QueryReturnTypeFactory
    {
        private static ConcurrentDictionary<Type, IInfraQueryReturnType> Cache { get; } = new ConcurrentDictionary<Type, IInfraQueryReturnType>();

        public static IInfraQueryReturnType FromGenericType<T>()
        {
            if (typeof(T) == typeof(int))
                return Cache.GetOrAdd(typeof(T), x => new ValueReturnType(DataFieldType.Int32));
            if (typeof(T) == typeof(float))
                return Cache.GetOrAdd(typeof(T), x => new ValueReturnType(DataFieldType.Float));
            // TODO
            throw new ArgumentOutOfRangeException();
        }
    }
}