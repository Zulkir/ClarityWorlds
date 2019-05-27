namespace IritNet
{
    public unsafe struct Bool2DInterStruct
    {
        public  Bool2DInterStruct* Pnext;
        public  IPVertexStruct* Poly1Vrtx, Poly2Vrtx;/* Ptr to Pl1/2 inter. vrtx.*/
        public  IPVertexStruct* Poly1Vrtx2, Poly2Vrtx2;      /* In share corners */
        /* - two inters! */
        public int DualInter;   /* If two intersections at the same location (corners). */
        public double Param1, Param2;     /* Parametrization along the poly vertices. */
        public IrtPtType InterPt;                                /* Location of intersection. */
        public IrtVecType Normal;                        /* Estimated normal at intersection. */
    }
}
