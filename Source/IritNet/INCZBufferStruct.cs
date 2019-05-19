namespace IritNet
{
    public unsafe struct INCZBufferStruct
    {
        public IRndrZBufferStruct ZBuf;
        public IRndrSceneStruct Scene;
        public IRndrObjectStruct Obj;
        public IRndrTriangleStruct Tri;
        public IRndrLineSegmentStruct Seg;
        public INCModeType Mode;
        public double ClbkZ, ZPixelsRemoved;
        public int GridSizeX, GridSizeY;
        public int ActiveRegionXMin, ActiveRegionYMin,
            ActiveRegionXMax, ActiveRegionYMax;
    }
}