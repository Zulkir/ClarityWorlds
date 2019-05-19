namespace IritNet
{
    public unsafe struct CagdCrvQuadTileStruct
    {
        public struct Union
        {
            public CagdCrvStruct* LimitCrvs;
            public CagdSrfStruct* Srf { get => (CagdSrfStruct*)LimitCrvs; set => LimitCrvs = (CagdCrvStruct*)value; }
        }

        public CagdCrvQuadTileStruct *Pnext;
        public IPAttributeStruct *Attr;
        public Union U;
        public CagdCrvQuadTileRepresentationType ActiveRepresentation;
    }
}