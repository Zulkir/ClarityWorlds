namespace IritNet
{
    public unsafe struct IrtVecType
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
    }
}