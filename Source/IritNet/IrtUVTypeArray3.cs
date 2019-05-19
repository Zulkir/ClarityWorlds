namespace IritNet
{
    public unsafe struct IrtUVTypeArray3
    {
        public IrtUVType F0;
        public IrtUVType F1;
        public IrtUVType F2;

        public IrtUVType this[int index]
        {
            get
            {
                var loc = this;
                return ((IrtUVType*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IrtUVType*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}