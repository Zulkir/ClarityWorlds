namespace IritNet
{
    public unsafe struct DoubleArray3
    {
        public double F0;
        public double F1;
        public double F2;

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
    }
}