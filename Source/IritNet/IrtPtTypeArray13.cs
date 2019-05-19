namespace IritNet
{
    public unsafe struct IrtPtTypeArray13
    {
        public IrtPtType F0;
        public IrtPtType F1;
        public IrtPtType F2;
        public IrtPtType F3;
        public IrtPtType F4;
        public IrtPtType F5;
        public IrtPtType F6;
        public IrtPtType F7;
        public IrtPtType F8;
        public IrtPtType F9;
        public IrtPtType F10;
        public IrtPtType F11;
        public IrtPtType F12;

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