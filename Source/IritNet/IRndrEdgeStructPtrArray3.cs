namespace IritNet 
{
    public unsafe struct IRndrEdgeStructPtrArray3
    {
        public IRndrEdgeStruct* F0;
        public IRndrEdgeStruct* F1;
        public IRndrEdgeStruct* F2;

        public IRndrEdgeStruct* this[int index]
        {
            get
            {
                var loc = this;
                return ((IRndrEdgeStruct**)&loc)[index];
            }
            set
            {
                var loc = this;
                ((IRndrEdgeStruct**)&loc)[index] = value;
                this = loc;
            }
        }
    }
}