using System;

namespace IritNet
{
    // size: p + 2i + 3d + i + d + i + d + i + i + 3p = 4p + 4d + 6i = 4p + 7d
    public unsafe struct UserMicroRegularParamStruct
    {
        public UserMicroTileStruct* Tile;    /* The tile to be used in regular tiling. */
        public int FFDeformApprox;  /* If TRUE, do approximated Freeform deform. */
        public int TilingStepMode;   /* Repeat tiling if TRUE, tiling Repeat[i]  */
        /* tiles in UVW, either globally or per Bezier domain. If FALSE, each   */
        /* tile is displace Repeat[i] amount in UVW (which can be overlapping). */
        public fixed double TilingSteps[3];    /* If TilingStepMode TRUE, UVW repetitions */
        /* count. If TilingStepMode FALSE, UVW displacements per tile in domain.*/
        public int ApprxTrimCrvsInShell; /* TRUE to piecewise-linear-approximate */
        /* global boundary crvs of tiles in E3, FALSE for precise composition.  */
        /* Tolerance of approximation controlled by TrimSetTrimCrvLinearApprox. */
        public double C0DiscontWScale;           /* W scale of C^0 discont. tiling. */
        public int MergeCurves;    /* If TRUE, curves tiles are merged globally. */
        public double MaxPolyEdgeLen; /* For poly tiles, max edge length in tile to */
        /* allow (longer edges are split), or zero to disable. */
        public int ApproxLowOrder; /* If 3 or 4, higher order freeforms results are    */
        /* reduced (approximated) to quadratics or cubics.  */
        public int GlobalPlacement; /* TRUE for tile placement along entire      */
        /* domain of deformation map, FALSE for placement per each Bezier patch.*/
        public IntPtr PreProcessCBFunc; /* If !NULL, a call */
        /* back func. called just before composition, editing the tile.         */
        public IntPtr PostProcessCBFunc; /* If !NULL, a    */
        /* call back func. called just after composition, editing the tile.     */
        public void* CBFuncData;      /* Optional pointer to be passed to CB functions.*/
    }
}