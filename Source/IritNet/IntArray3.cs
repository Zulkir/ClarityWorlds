namespace IritNet
{
    public unsafe struct IntArray3
    {
        public fixed int Values[3];

        public int this[int index]
        {
            get
            {
                var loc = this;
                return ((int*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((int*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}