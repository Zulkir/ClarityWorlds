namespace IritNet
{
    public unsafe struct MvarZeroPrblmIntrnlStruct
    {
        public struct Union
        {
            public MvarMVStruct** FirstSmoothMVs;
            public MvarExprTreeStruct** FirstSmoothETs { get { return (MvarExprTreeStruct**)FirstSmoothMVs; } set { FirstSmoothMVs = (MvarMVStruct**)value; } }
        }
        
        public Union U;
        public MvarMVGradientStruct** MVGradients;
        public MvarZeroSolutionStruct* SolutionsOnBoundary;
        public int NumOfBoundarySolutions;
        public int UnderSubdivTol;
        public int SingleSolutionTest0D;
        public int NoLoopTest1D;
        public int SingleComponentTest1D;
        public int One2OneProjOnPlane2D;
        public int HasC1Discont;
        public int IsFirstSmooth;
        public int PurgeByNumericStep;
        public int ConstructionComplete;
        public int ZeroMVsSameSpace;
        public int OptimizationForm;  /* Solving for zero norm via minimization. */
        public int NonZeroOptim;  /* Allow optimizers that are not of zero norm. */
        public int IsBoundaryProblem; /* Of another higher dim problem.          */
        public int C1DiscontInd;
        public int ExpectedSolutionDim;	            /* When the problem is regular. */
        public int NumOfZeroSubdivConstraints;
        public int SubdivDepth;
        public int SubdivTolReached;
        public fixed int One2OneProjDirs[2];
        public double ParamPerturb;
        public double* OrigMVMinDmn;	     /* The domain of the original problem. */
        public double* OrigMVMaxDmn;
        public double* MVMinDmn;	      /* The domain of the current problem. */
        public double* MVMaxDmn;
        public double MaxSide;
        public int MaxSideDir;
        public int SubdivDir;
        public double ParamOfSubdiv;
        public double C1DiscontParam;
        public MvarZeroTJunctionStruct* TJList; /* Mesh T-Junctions, used in 2D solver. */
        public MvarZeroSolverCallBackFcnStruct CallbackFunctions;
        public MvarZero2DPredictedMeshQualityParamsStruct* MeshQParams; /* 2D solver.   */
    }
}