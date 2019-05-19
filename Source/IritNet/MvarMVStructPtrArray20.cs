namespace IritNet
{
    public unsafe struct MvarMVStructPtrArray20
    {
        public MvarMVStruct* F0;
        public MvarMVStruct* F1;
        public MvarMVStruct* F2;
        public MvarMVStruct* F3;
        public MvarMVStruct* F4;
        public MvarMVStruct* F5;
        public MvarMVStruct* F6;
        public MvarMVStruct* F7;
        public MvarMVStruct* F8;
        public MvarMVStruct* F9;
        public MvarMVStruct* F10;
        public MvarMVStruct* F11;
        public MvarMVStruct* F12;
        public MvarMVStruct* F13;
        public MvarMVStruct* F14;
        public MvarMVStruct* F15;
        public MvarMVStruct* F16;
        public MvarMVStruct* F17;
        public MvarMVStruct* F18;
        public MvarMVStruct* F19;

        public MvarMVStruct* this[int index]
        {
            get
            {
                var loc = this;
                return ((MvarMVStruct**)&loc)[index];
            }
            set
            {
                var loc = this;
                ((MvarMVStruct**)&loc)[index] = value;
                this = loc;
            }
        }
    }
}