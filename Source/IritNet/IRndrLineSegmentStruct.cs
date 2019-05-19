namespace IritNet
{
    public unsafe struct IRndrLineSegmentStruct
    {
        public IPPolygonStruct *Tri;
        public IrtPlnTypeArray5 Vertex; /* 4 for the rectangle + one extra for sharp bends.*/
        public IrtVecTypeArray5 Normal; /* 4 for the rectangle + one extra for sharp bends.*/
        public IrtPlnType LastPoint;
        public IrtPlnType LastDelta;
        public IPVertexStructPtrArray3 TriVertex;
        public IRndrPolylineOptionsStruct PolyOptions;
        public double k;
        public int NumVertex;
        public int TrianglesNum;           /* No. of triangles constructing the segment */
        public int SharpBend;                    /* TRUE for more than 90 degrees turn. */
    }
}