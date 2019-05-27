namespace IritNet
{
    public unsafe struct VMdlGenericRefStruct
    {
        public  VMdlInterTrimPointRefStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public void* ObjRef;
    }
}
