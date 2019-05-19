namespace IritNet
{
    public unsafe struct jmp_buf
    {
        public fixed int Values[16];

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