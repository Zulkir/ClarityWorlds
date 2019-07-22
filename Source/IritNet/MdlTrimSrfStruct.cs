namespace IritNet
{
    public unsafe struct MdlTrimSrfStruct
    {
        public  MdlTrimSrfStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public CagdSrfStruct* Srf;                /* Surface trimmed by MdlTrimSegList. */
        public MdlLoopStruct* LoopList;
    }
}
