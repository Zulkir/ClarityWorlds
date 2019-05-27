namespace IritNet
{
    public unsafe struct CagdPolylineStruct
    {
        public  CagdPolylineStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public CagdPolylnStruct* Polyline; /* Polyline length is defined using Length. */
        public int Length;
    }
}
