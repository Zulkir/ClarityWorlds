namespace IritNet
{
    public unsafe struct GMLsPointArray2
    {
        public GMLsPoint F0;
        public GMLsPoint F1;

        public GMLsPoint this[int index]
        {
            get
            {
                var loc = this;
                return ((GMLsPoint*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((GMLsPoint*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}