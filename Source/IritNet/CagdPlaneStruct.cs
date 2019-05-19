namespace IritNet
{
    public unsafe struct CagdPlaneStruct
    {
        public CagdPlaneStruct *Pnext;
        public IPAttributeStruct *Attr;
        public fixed double Plane[4];
    }
}