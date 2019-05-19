namespace IritNet
{
    public unsafe struct MvarPolylineStruct
    {
        public  MvarPolylineStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public MvarPtStruct *Pl;
        public void * PAux;
    }
}
