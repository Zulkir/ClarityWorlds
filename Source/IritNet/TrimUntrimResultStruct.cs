namespace IritNet
{
    public unsafe struct TrimUntrimResultStruct
    {
        public  TrimUntrimResultStruct *Pnext;
        public IPAttributeStruct *Attr;
        public CagdSrfStruct *ContainingSrf;
        public CagdCrvQuadTileStruct *UVTiles;
        public CagdSrfStruct *UntrimmedSrfs;
    }
}
