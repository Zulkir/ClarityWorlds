namespace IritNet
{
    public unsafe struct IrtNrmlType
    {
        public fixed double Values[3];

        public double this[int index]
        {
            get
            {
                var loc = this;
                return ((double*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((double*)&loc)[index] = value;
                this = loc;
            }
        }

        public override string ToString()
        {
            var loc = this;
            return $"{{{loc.Values[0]}, {loc.Values[1]}, {loc.Values[2]}}}";
        }
    }
}