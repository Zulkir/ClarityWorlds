using System;

namespace Clarity.Common.CodingUtilities.Patterns
{
    public struct ProxyPropertyFacade<TValue>
    {
        private readonly object closure1;
        private readonly object closure2;
        private readonly Func<object, object, TValue> getValue;
        private readonly bool isImmutable;

        public bool IsImmutable => isImmutable;
        public TValue GetValue() => getValue(closure1, closure2);

        public ProxyPropertyFacade(object closure1, object closure2, Func<object, object, TValue> getValue, bool isImmutable)
        {
            this.closure1 = closure1;
            this.closure2 = closure2;
            this.getValue = getValue;
            this.isImmutable = isImmutable;
        }
    }
}