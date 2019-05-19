namespace IritNet
{
    public unsafe struct IRndrTriangleStruct
    {
        public IRndrEdgeStructArray3 Edge;        /* Array of edges representing Triangle. */
        public IRndrEdgeStructPtrArray3 SortedEdge;
        public int YMin, YMax;               /* Scan line range Triangle is located in. */
        public double ZMin, ZMax;                       /* Z limits of this triangle. */
        public IPPolygonStruct *Poly;         /* Pointer to the compliant Irit polygon. */
        public IRndrObjectStruct *Object;      /* Object that Triangle is contained in. */
        public IRndrIntensivityStruct **Vals;
        public IRndrIntensivityStruct **dVals;
        public double dz;
        public int IsBackFaced;
        public IRndrVisibleValidityType Validity;
    }
}