using System;
using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public interface IPropertyBag
    {
        IEnumerable<string> Keys { get; }
        bool ContainsProperty(string property);
        TVal SearchValue<TVal>(string property, TVal defaultValue = default(TVal));
        bool TryGetValue<TVal>(string property, out TVal val);
        TVal GetOrAdd<TVal>(string property) where TVal : new();
        TVal GetOrAdd<TVal>(string property, Func<string, TVal> create);
        void SetValue<TVal>(string property, TVal val);
    }
}