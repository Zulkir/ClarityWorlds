namespace IritNet 
{
    public unsafe struct TrivIGAAdjacencyInfoStructArray6
    {
        public TrivIGAAdjacencyInfoStruct F0;
        public TrivIGAAdjacencyInfoStruct F1;
        public TrivIGAAdjacencyInfoStruct F2;
        public TrivIGAAdjacencyInfoStruct F3;
        public TrivIGAAdjacencyInfoStruct F4;
        public TrivIGAAdjacencyInfoStruct F5;

        public TrivIGAAdjacencyInfoStruct this[int index]
        {
            get
            {
                var loc = this;
                return ((TrivIGAAdjacencyInfoStruct*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((TrivIGAAdjacencyInfoStruct*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}