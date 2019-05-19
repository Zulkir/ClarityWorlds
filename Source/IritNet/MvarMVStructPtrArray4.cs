namespace IritNet 
{
    public unsafe struct MvarMVStructPtrArray4
    {
        public MvarMVStruct *F0; 
        public MvarMVStruct *F1; 
        public MvarMVStruct *F2; 
        public MvarMVStruct *F3;
        
        public MvarMVStruct this[int index]
        {
            get
            {
                var loc = this;
                return ((MvarMVStruct*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((MvarMVStruct*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}