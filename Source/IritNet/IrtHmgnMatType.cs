namespace IritNet
{
    public unsafe struct IrtHmgnMatType
    {
        public fixed double Values[16];

        public double this[int i, int j]
        {
            get
            {
                var loc = this;
                return ((double*)&loc)[4 * i + j];
            }
            set
            {
                var loc = this;
                ((double*)&loc)[4 * i + j] = value;
                this = loc;
            }
        }
    }
}