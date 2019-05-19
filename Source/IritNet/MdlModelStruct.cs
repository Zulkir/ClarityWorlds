namespace IritNet
{
    public unsafe struct MdlModelStruct
    {
        public  MdlModelStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public MdlTrimSrfStruct *TrimSrfList;
        public MdlTrimSegStruct *TrimSegList;       /* List of trimming curve segments. */
    }
}
