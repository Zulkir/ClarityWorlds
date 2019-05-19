namespace IritNet 
{
    public unsafe struct IrtVecTypeArray5
    {
        public IrtVecType F0;
        public IrtVecType F1;
        public IrtVecType F2;
        public IrtVecType F3;
        public IrtVecType F4;

        public IrtVecType this[int index]
        {
            get
            {
                var loc = this;
                return ((IrtVecType*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IrtVecType*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}