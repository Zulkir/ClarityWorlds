namespace IritNet
{
    public unsafe struct IRndrPixelType
    {
        public fixed byte Values[3];

        public byte this[int index]
        {
            get
            {
                var loc = this;
                return ((byte*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((byte*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}