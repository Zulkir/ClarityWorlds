using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public class PropertyBag : IPropertyBag
    {
        private readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        public IEnumerable<string> Keys => dict.Keys;

        public bool ContainsProperty(string property) => dict.ContainsKey(property);

        public TVal SearchValue<TVal>(string property, TVal defaultValue = default(TVal)) => dict.TryGetValue(property, out var val) && val is TVal cVal ? cVal : defaultValue;

        public bool TryGetValue<TVal>(string property, out TVal val)
        {
            if (dict.TryGetValue(property, out var oVal) && oVal is TVal cVal)
            {
                val = cVal;
                return true;
            }
            val = default(TVal);
            return false;
        }

        public TVal GetOrAdd<TVal>(string property) where TVal : new() =>
            GetOrAdd(property, x => new TVal());

        public TVal GetOrAdd<TVal>(string property, Func<string, TVal> create)
        {
            if (dict.TryGetValue(property, out var oVal) && oVal is TVal cVal)
                return cVal;
            cVal = create(property);
            dict[property] = cVal;
            return cVal;
        }

        public void SetValue<TVal>(string property, TVal val)
        {
            dict[property] = val;
        }
    }
}