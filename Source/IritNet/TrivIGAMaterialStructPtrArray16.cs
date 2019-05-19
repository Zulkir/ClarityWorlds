namespace IritNet 
{
    public unsafe struct TrivIGAMaterialStructPtrArray16
    {
        public TrivIGAMaterialStruct *F0;
        public TrivIGAMaterialStruct *F1;
        public TrivIGAMaterialStruct *F2;
        public TrivIGAMaterialStruct *F3;
        public TrivIGAMaterialStruct *F4;
        public TrivIGAMaterialStruct *F5;
        public TrivIGAMaterialStruct *F6;
        public TrivIGAMaterialStruct *F7;
        public TrivIGAMaterialStruct *F8;
        public TrivIGAMaterialStruct *F9;
        public TrivIGAMaterialStruct *F10;
        public TrivIGAMaterialStruct *F11;
        public TrivIGAMaterialStruct *F12;
        public TrivIGAMaterialStruct *F13;
        public TrivIGAMaterialStruct *F14;
        public TrivIGAMaterialStruct *F15;

        public TrivIGAMaterialStruct this[int index]
        {
            get
            {
                var loc = this;
                return ((TrivIGAMaterialStruct*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((TrivIGAMaterialStruct*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}