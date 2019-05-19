namespace IritNet
{
    public unsafe struct CagdPolygonStruct
    {
        public struct PolygonStruct
        {
            public IrtPtType Pt;   /* Polygon is either triangle or rectangle. */
            public IrtVecType Nrml;
            public IrtUVType UV;
        }

        public struct PolygonStructArray4
        {
            public PolygonStruct F0;
            public PolygonStruct F1;
            public PolygonStruct F2;
            public PolygonStruct F3;

            public PolygonStruct this[int index]
            {
                get
                {
                    var loc = this;
                    return ((PolygonStruct*)&loc)[index];
                }
                set
                {
                    var loc = this;
                    ((PolygonStruct*)&loc)[index] = value;
                    this = loc;
                }
            }
        }

        public struct PolyStripStruct
        {
            /* Polygonal strip can have arbitrary # of polygons. */
            /* Base line - the first edge. */
            public IrtPtTypeArray2 FirstPt;
            public IrtVecTypeArray2 FirstNrml;
            public IrtUVTypeArray2 FirstUV;
            public IrtPtType* StripPt;           /* Arrays of size NumOfPolys. */
            public IrtVecType* StripNrml;
            public IrtUVType* StripUV;
            public int NumOfPolys;
        }

        public struct Union
        {
            public PolygonStructArray4 Polygon;
            public PolyStripStruct PolyStrip { get { var loc = Polygon; return *(PolyStripStruct*)&loc; } set { var loc = this; *(PolyStripStruct*)&loc = value; this = loc; } }
        }

        public CagdPolygonStruct *Pnext;
        public IPAttributeStruct *Attr;
        public CagdPolygonType PolyType;
        public Union U;
    }
}