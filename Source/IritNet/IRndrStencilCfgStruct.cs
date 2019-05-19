namespace IritNet
{
    public unsafe struct IRndrStencilCfgStruct
    {
        public IRndrStencilCmpType SCmp;
        public int Ref;
        public uint Mask;
        public IRndrStencilOpType OpFail;
        public IRndrStencilOpType OpZFail;
        public IRndrStencilOpType OpZPass;
    }
}