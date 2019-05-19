namespace IritNet
{
    public unsafe struct MvarSrfSrfInterCacheAttribName
    {
        public fixed byte Value[Irit.IRIT_LINE_LEN];

        public byte this[int index]
        {
            get
            {
                var loc = this;
                return ((byte*)&loc)[index];
            }
            set
            {
                var loc = this;
                ((byte*)&loc)[index] = value;
                this = loc;
            }
        }
    }
}