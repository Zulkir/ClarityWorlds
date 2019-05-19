namespace IritNet
{
    public unsafe struct IPFreeformConvStateStruct
    {
        public int Talkative;                      /* If TRUE, be more talkative. */
        public int DumpObjsAsPolylines;  /* Should we dump polylines or polygons. */
        public int DrawFFGeom;           /* Should we dump the freeform geometry. */
        public int DrawFFMesh;         /* Should we dump control polygons/meshes. */
        public fixed int NumOfIsolines[3];    /* isolines for surfaces and trivariates. */
        public double CrvApproxTolSamples;        /* Samples/Tolerance of PL approx. */
        public SymbCrvApproxMethodType CrvApproxMethod;     /* Piecewise linear approx. */
        public int ShowInternal;       /* Do we display internal polygonal edges? */
        public int CubicCrvsAprox;    /* Do curves should be approx. as cubics. */
        public double FineNess;         /* Control over the polygonal approximation. */
        public int ComputeUV;    /* Attach UV attributes to vertices of polygons. */
        public int ComputeNrml;   /* Attach normal attrs to vertices of polygons. */
        public int FourPerFlat;          /* Two or four triangles per flat patch. */
        public int OptimalPolygons;           /* Optimal (or not optimal) approx. */
        public int BBoxGrid;           /* Compute bbox/grid subdivision for bbox. */
        public int LinearOnePolyFlag;/* Only one polygonal subdiv. along linears. */
        public int SrfsToBicubicBzr;      /* Should we convert to bicubic bezier? */
    }
}