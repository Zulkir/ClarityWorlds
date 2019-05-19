namespace IritNet
{
    public unsafe struct VMdlSlicerInfoStruct
    {
        public TrivTVStruct *Trivar;
        public MdlModelStruct *MdlSrf;
        public TrivInverseQueryStruct *RevHandle;
        public VMdlSlicerParamsStruct Params;
        public VMdlSliceLclInfoStruct CurrSlice;
    }
}