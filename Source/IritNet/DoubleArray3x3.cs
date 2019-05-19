namespace IritNet
{
    public unsafe struct DoubleArray3x3
    {
        public fixed double Values[3 * 3];

        public double this[int i, int j]
        {
            get
            {
                var loc = this;
                return ((double*)&loc)[3 * i + j];
            }
            set
            {
                var loc = this;
                ((double*)&loc)[3 * i + j] = value;
                this = loc;
            }
        }
    }
}