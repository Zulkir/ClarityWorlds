using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers 
{
    public class StringDictionaryTrwHandler<TDict, TValue> : ObjectDiffableTrwHandlerBase<TDict, TValue, string>
        where TDict : IDictionary<string, TValue>, new()
    {
        protected override TDict CreateBuilder() => new TDict();
        protected override IEnumerable<string> EnumerateProps(TDict obj) => obj.Keys;
        protected override string GetPropName(string prop) => prop;
        protected override Type GetPropType(string prop) => typeof(TValue);
        protected override TValue GetPropValue(TDict obj, string prop) => obj[prop];
        protected override void SetPropValue(TDict builder, object boxedBuilder, string prop, TValue value) => builder[prop] = value;

        protected override bool TryGetProp(TDict obj, string name, out string prop)
        {
            prop = name;
            return true;
        }
    }
}