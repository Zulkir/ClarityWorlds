namespace IritNet
{
    public unsafe struct UserMicroPreProcessTileCBStruct
    {
        public fixed double UVWMin[3], UVWMax[3];  /* Domain of tile in deformation func. */
        public fixed int UVWIndices[3];  /* Base index of this tile in the 3D grid of tiles. */
        public fixed int UVWTotalRep[3];  /* Number of tiles to place in 3 directions in each*/
        /* Bezier patch/global (following GlobalPlacement) of deformation map.  */
        public fixed int NumBranchesUV[2]; /* 1 + Number of C0 discontinuities in u and v    */
        /* directions of trivariate tiles.		    */
        public IrtHmgnMatType Mat;  /* Mapping of [0, 1]^3 to tile position in defmat. */
        public void *CBFuncData;  /* Input data pointer given by UserMicroParamStruct. */
    }
}