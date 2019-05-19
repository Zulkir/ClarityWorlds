namespace IritNet
{
    public unsafe struct IPVertexStruct
    {
        public  IPVertexStruct *Pnext;                        /* To next in chain. */
        public  IPAttributeStruct *Attr;
        public  IPPolygonStruct *PAdj;                     /* To adjacent polygon. */
        public byte Tags;                                         /* Some attributes. */
        public IrtPtType Coord;                               /* Holds X, Y, Z coordinates. */
        public IrtNrmlType Normal;                       /* Hold Vertex normal into the solid. */
    }
}
