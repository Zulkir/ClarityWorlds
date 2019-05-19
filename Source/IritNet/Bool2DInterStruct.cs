namespace IritNet
{
    public unsafe struct Bool2DInterStruct 
    {  /* Holds info. on 2D intersetion points. */
        public Bool2DInterStruct *Pnext;
        public IPVertexStruct* Poly1Vrtx;
        public IPVertexStruct *Poly2Vrtx;       /* Pointer to Pl1/2 inter. vrtx. */
        public IPVertexStruct* Poly1Vrtx2;
        public IPVertexStruct *Poly2Vrtx2;       /* In share corners - 2 inters! */
        public int DualInter;   /* If two intersections at the same location (corners). */
        public double Param1, Param2;     /* Parametrization along the poly vertices. */
        public IrtPtType InterPt;				/* Location of intersection. */
        public IrtVecType Normal;			/* Estimated normal at intersection. */
    }
}