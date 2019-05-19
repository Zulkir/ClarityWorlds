namespace IritNet
{
    public unsafe struct TrivIGAXMLPropertyArray16
    {
        public TrivIGAXMLProperty F0;
        public TrivIGAXMLProperty F1;
        public TrivIGAXMLProperty F2;
        public TrivIGAXMLProperty F3;
        public TrivIGAXMLProperty F4;
        public TrivIGAXMLProperty F5;
        public TrivIGAXMLProperty F6;
        public TrivIGAXMLProperty F7;
        public TrivIGAXMLProperty F8;
        public TrivIGAXMLProperty F9;
        public TrivIGAXMLProperty F10;
        public TrivIGAXMLProperty F11;
        public TrivIGAXMLProperty F12;
        public TrivIGAXMLProperty F13;
        public TrivIGAXMLProperty F14;
        public TrivIGAXMLProperty F15;

        public TrivIGAXMLProperty this[int index]
        {
            get
            {
                var loc = this;
                return ((TrivIGAXMLProperty*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((TrivIGAXMLProperty*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}