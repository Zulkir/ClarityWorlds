namespace IritNet
{
    public unsafe struct _MCInterStructArray12
    {
        public _MCInterStruct F0;
        public _MCInterStruct F1;
        public _MCInterStruct F2;
        public _MCInterStruct F3;
        public _MCInterStruct F4;
        public _MCInterStruct F5;
        public _MCInterStruct F6;
        public _MCInterStruct F7;
        public _MCInterStruct F8;
        public _MCInterStruct F9;
        public _MCInterStruct F10;
        public _MCInterStruct F11;

        public _MCInterStruct this[int index]
        {
            get
            {
                var loc = this;
                return ((_MCInterStruct*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((_MCInterStruct*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}