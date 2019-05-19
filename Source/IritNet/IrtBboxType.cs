namespace IritNet
{
    public unsafe struct IrtBboxType
    {
        public IrtPtType F0;
        public IrtPtType F1;

        public IrtPtType this[int index]
        {
            get
            {
                var loc = this;
                return ((IrtPtType*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IrtPtType*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}