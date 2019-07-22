namespace IritNet
{
    public unsafe struct UserMicroTileStruct
    {
        public  UserMicroTileStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public IPObjectStruct* Geom; /* Geometry of a tile (curves/surfaces/polys etc. */
    }
}
