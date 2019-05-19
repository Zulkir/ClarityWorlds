namespace IritNet
{
    public unsafe struct VMdlSlicerParamDataStruct
    {
        public DoubleArray3 Param;
        public IntArray3 IData { get { var loc = Param; return *(IntArray3*)&loc;} set { var loc = Param; *(IntArray3*)&loc = value; Param = loc; } }
    }
}