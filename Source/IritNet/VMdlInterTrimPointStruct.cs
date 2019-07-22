namespace IritNet
{
    public unsafe struct VMdlInterTrimPointStruct
    {
        public  VMdlInterTrimPointStruct* Pnext;
        public  IPAttributeStruct* Attr;
        /* curves that pass through this point. */
        public  VMdlInterTrimCurveSegRefStruct* InterCurveSegRefList;
        public IrtPtType E3Pnt;
    }
}
