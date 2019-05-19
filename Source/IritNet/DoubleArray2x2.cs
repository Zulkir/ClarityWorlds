namespace IritNet
{
    public unsafe struct DoubleArray2x2
    {
        public fixed double Values[2 * 2];

        public double this[int i, int j]
        {
            get
            {
                var loc = this;
                return ((double*)&loc)[2 * i + j];
            }
            set
            {
                var loc = this;
                ((double*)&loc)[2 * i + j] = value;
                this = loc;
            }
        }
    }
}