using System;

namespace Clarity.Common.CodingUtilities.Patterns 
{
    public struct ProxyProperty<TMaster, TValue>
    {
        private readonly TValue immutableValue;
        private readonly Func<TMaster, TValue> getValueIndirect;

        public bool IsImmutable => 
            getValueIndirect == null;

        public TValue GetValue(TMaster master) => 
            getValueIndirect != null ? getValueIndirect(master) : immutableValue;

        public ProxyProperty(TValue immutableValue)
        {
            this.immutableValue = immutableValue;
            getValueIndirect = null;
        }

        public ProxyProperty(Func<TMaster, TValue> getValueIndirect)
        {
            this.getValueIndirect = getValueIndirect;
            immutableValue = default(TValue);
        }
    }
}