namespace IritNet
{
    public unsafe struct TrivIGABoundaryNodeStruct
    {
        public TrivIGABoundaryNodeStruct *Pnext;
        public int NodeID;
        public TrivIGANodeBoundaryType BoundaryType;
        public byte *BoundaryAxisConditions;
        public double Value;
    }
}