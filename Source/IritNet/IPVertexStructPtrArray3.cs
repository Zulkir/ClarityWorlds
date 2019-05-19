namespace IritNet
{
    public unsafe struct IPVertexStructPtrArray3
    {
        public IPVertexStruct* F0;
        public IPVertexStruct* F1;
        public IPVertexStruct* F2;

        public IPVertexStruct* this[int index]
        {
            get
            {
                var loc = this;
                return ((IPVertexStruct**)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IPVertexStruct**)&loc)[index] = value;
                this = loc;
            }
        }
    }
}