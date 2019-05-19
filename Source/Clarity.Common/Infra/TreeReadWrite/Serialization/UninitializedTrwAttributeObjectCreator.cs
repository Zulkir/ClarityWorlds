using System;
using System.Runtime.Serialization;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class UninitializedTrwAttributeObjectCreator : ITrwAttributeObjectCreator
    {
        public Func<T> GetConstructor<T>()
        {
            return () => (T)FormatterServices.GetUninitializedObject(typeof(T));
        }
    }
}