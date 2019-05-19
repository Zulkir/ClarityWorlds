namespace IritNet 
{
    public unsafe struct IRndrColorTypeArray3
    {
        public IRndrColorType F0;
        public IRndrColorType F1;
        public IRndrColorType F2;

        public IRndrColorType this[int index]
        {
            get
            {
                var loc = this;
                return ((IRndrColorType*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IRndrColorType*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}