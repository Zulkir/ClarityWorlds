namespace IritNet
{
    public unsafe struct IritE2TExprNodeStruct
    {
        public IritE2TExprNodeStruct *Right;
        public IritE2TExprNodeStruct *Left;
        public int NodeKind;
        public double RData;
        public byte *SData;
    }
}