namespace IritNet 
{
    public unsafe struct IrtPlnTypeArray5
    {
        public IrtPlnType F0;
        public IrtPlnType F1;
        public IrtPlnType F2;
        public IrtPlnType F3;
        public IrtPlnType F4;

        public IrtPlnType this[int index]
        {
            get
            {
                var loc = this;
                return ((IrtPlnType*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IrtPlnType*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}