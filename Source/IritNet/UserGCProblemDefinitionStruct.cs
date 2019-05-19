namespace IritNet
{
    public unsafe struct UserGCProblemDefinitionStruct
    {
        public UserGCObsPtGroupTypeStruct **ObsPtsGroups;    /* NULL terminated vector. */
        public IPObjectStruct *GeoObj;                     /* Object to compute GC for. */
        public IPObjectStruct *Obstacles;
        public IrtImgPixelStruct *UVMap;    /* The zeroes pixels are ignored UV values. */
        public UserGCSolvingParamsStruct SolvingParams;
        public UserGCDebugParamsStruct DebugParams;
    }
}
