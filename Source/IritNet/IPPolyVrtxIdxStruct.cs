namespace IritNet
{
    public unsafe struct IPPolyVrtxIdxStruct
    {
        public  IPPolyVrtxIdxStruct *Pnext;                  /* To next in chain. */
        public  IPAttributeStruct *Attr;                  /* Object's attributes. */
        public   IPObjectStruct *PObj; /* Pointer to original polygonal obj. */
        public IPVertexStruct **Vertices;    /* NULL terminated vector of all vertices. */
        public IPPolyPtrStruct **PPolys;     /* Vector of polygons holding each vertex. */
        public int **Polygons;        /* A vector of -1 terminated vectors of vertices. */
        public int *_AuxVIndices;  /* Auxiliary memory to hold all indices in Polygons. */
        public int NumVrtcs;                         /* Number of vertices in geometry. */
        public int NumPlys;                          /* Number of polygons in geometry. */
        public int TriangularMesh; /* TRUE if a triangular polys only, FALSE otherwise. */
    }
}
