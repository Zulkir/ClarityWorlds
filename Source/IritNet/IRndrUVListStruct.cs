namespace IritNet
{
    public unsafe struct IRndrUVListStruct
    {
        public IrtPtType Coord;
        public IRndrVisibleFillType Value;
        public IRndrVisibleValidityType Validity;
        public int BackFaced;
        public IPPolygonStruct* Triangle;     /* The triangle which created this point. */
    }
}