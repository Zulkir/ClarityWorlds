namespace IritNet
{
    public unsafe struct IPPolygonStruct
    {
        public  IPPolygonStruct *Pnext;                        /* To next in chain. */
        public  IPAttributeStruct *Attr;
        public IPVertexStruct *PVertex;                                    /* To vertices list. */
        public void * PAux;
        public byte Tags;                                         /* Some attributes. */
        public int IAux, IAux2, IAux3;
        public IrtPlnType Plane;                         /* Holds Plane as Ax + By + Cz + D. */
        public IrtBboxType BBox;                                        /* BBox of polygons. */
    }
}
