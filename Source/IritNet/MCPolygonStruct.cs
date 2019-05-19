namespace IritNet
{
    public unsafe struct MCPolygonStruct
    {
        public MCPolygonStruct *Pnext;		        /* To next in chain. */
        public int NumOfVertices;
        public IrtPtTypeArray13 V;
        public IrtPtTypeArray13 N;
    }
}