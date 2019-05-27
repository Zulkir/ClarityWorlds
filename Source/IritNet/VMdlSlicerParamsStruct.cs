namespace IritNet
{
    public unsafe struct VMdlSlicerParamsStruct
    {
        public double ZRes;       /* Z size of a single pixel. */
        public fixed double XYRes[2];   /* XY size of a single pixel. */
        
        public fixed double Max[2];     /* XY bounding box of domain to slice. */
        public fixed double Min[2];
    }
}
