namespace IritNet 
{
    public unsafe struct TrivPln4DType
    {
        public fixed double Values[5];

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