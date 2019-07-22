namespace IritNet
{
    public unsafe struct IrtPtType
    {
        public fixed double Values[3];

        public IrtPtType(double x, double y, double z)
        {
            this[0] = x;
            this[1] = y;
            this[2] = z;
        }

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
            return string.Format("{{{0}, {1}, {2}}}", loc.Values[0], loc.Values[1], loc.Values[2]);
        }
    }
}