namespace IritNet
{
    public unsafe struct IrtUVType
    {
        public fixed double Values[2];

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