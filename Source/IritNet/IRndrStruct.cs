namespace IritNet
{
    public unsafe struct IRndrStruct
    {
        public IRndrZBufferStruct ZBuf;
        public IRndrSceneStruct Scene;
        public IRndrObjectStruct Obj;
        public IRndrTriangleStruct Tri;
        public IRndrLineSegmentStruct Seg;
        public IRndrModeType Mode;
        public IRndrVMStruct VisMap;
    }
}