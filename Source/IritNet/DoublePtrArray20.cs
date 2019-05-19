namespace IritNet
{
    public unsafe struct DoublePtrArray20
    {
        public double* F0;
        public double* F1;
        public double* F2;
        public double* F3;
        public double* F4;
        public double* F5;
        public double* F6;
        public double* F7;
        public double* F8;
        public double* F9;
        public double* F10;
        public double* F11;
        public double* F12;
        public double* F13;
        public double* F14;
        public double* F15;
        public double* F16;
        public double* F17;
        public double* F18;
        public double* F19;

        public double* this[int index]
        {
            get
            {
                var loc = this;
                return ((double**)&loc)[index];
            }
            set
            {
                var loc = this;
                ((double**)&loc)[index] = value;
                this = loc;
            }
        }
    }
}