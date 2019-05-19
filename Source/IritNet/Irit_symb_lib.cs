using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void SymbSetErrorFuncType (SymbFatalErrorType ErrorFunc);
    public unsafe delegate void SymbAdapIsoDistSqrFuncType (int Level,
                                                             CagdCrvStruct *Crv1,
                                                             CagdCrvStruct *NCrv1,
                                                             CagdCrvStruct *Crv2,
                                                             CagdCrvStruct *NCrv2);
    public unsafe delegate void SymbPlErrorFuncType (CagdSrfStruct *Srf,
                                                 CagdSrfDirType Dir,
                                                 int SubdivDepth);
    public unsafe delegate void SymbOffCrvFuncType ( CagdCrvStruct *Crv,
                                                     double Off,
                                                     int B);
    public unsafe delegate void SymbVarOffCrvFuncType ( CagdCrvStruct *Crv,
                                                         CagdCrvStruct *VarOff,
                                                        int B);
    public unsafe delegate void SymbUniformAprxSrfPtImportanceFuncType ( CagdSrfStruct *Srf,
                                                              double u,
                                                              double v);
    public unsafe delegate void SymbCrv2PolylineTlrncErrorFuncType ( CagdCrvStruct
                                                                                 *Crv);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbArcStruct *SymbArcArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbArcStruct *SymbArcNew(int Arc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbArcStruct *SymbArcCopy(SymbArcStruct *Arc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbArcStruct *SymbArcCopyList(SymbArcStruct *ArcList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbArcFree(SymbArcStruct *Arc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbArcFreeList(SymbArcStruct *ArcList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbArcArrayFree(SymbArcStruct *ArcArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *SymbCrv2Polyline( CagdCrvStruct *Crv,
                                     double TolSamplesPerCurve,
                                     SymbCrvApproxMethodType Method,
                                     int OptiLin);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbHugeCrv2Polyline( CagdCrvStruct *Crv,
                                   int Samples,
                                   int AddFirstPt,
                                   int AddLastPt,
                                   int AddParamVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbCrv2PolylineTlrncErrorFuncType SymbCrv2PolylineSetTlrncErrorFunc(
                                 SymbCrv2PolylineTlrncErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvAdd( CagdCrvStruct *Crv1,
                           CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvSub( CagdCrvStruct *Crv1,
                           CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMult( CagdCrvStruct *Crv1,
                            CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvInvert( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvScalarScale( CagdCrvStruct *Crv,
                                  double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvDotProd( CagdCrvStruct *Crv1,
                               CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvVecDotProd( CagdCrvStruct *Crv,
                                  double *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMultScalar( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvCrossProd( CagdCrvStruct *Crv1,
                                 CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvVecCrossProd( CagdCrvStruct *Crv,
                                    IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvRtnlMult( CagdCrvStruct *Crv1X,
                                CagdCrvStruct *Crv1W,
                                CagdCrvStruct *Crv2X,
                                CagdCrvStruct *Crv2W,
                               int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvDeriveRational( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvEnclosedArea( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbCrvEnclosedAreaEval( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv2DCurvatureSqr( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv3DCurvatureSqr( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv3DRadiusNormal( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv2DUnnormNormal( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv3DCurvatureNormal( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrv2DCurvatureSign( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvGenSignedCrvtr( CagdCrvStruct *Crv,
                                     int Samples,
                                     int Order,
                                     int ArcLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbSignedCrvtrGenCrv( CagdCrvStruct *Crvtr,
                                     double Tol,
                                     int Order,
                                     int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbMakePosCrvCtlPolyPos( CagdCrvStruct *OrigCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrv2DInflectionPts( CagdCrvStruct *Crv,
                                     double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvExtremCrvtrPts( CagdCrvStruct *Crv,
                                    double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvExtremCrvtrPts2( CagdCrvStruct *Crv,
                                     double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct **SymbCrvSplitScalarN( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbCrvSplitScalar( CagdCrvStruct *Crv,
                        CagdCrvStruct **CrvW,
                        CagdCrvStruct **CrvX,
                        CagdCrvStruct **CrvY,
                        CagdCrvStruct **CrvZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMergeScalarN(CagdCrvStruct *  *CrvVec, 
                                   int NumCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMergeScalar( CagdCrvStruct *CrvW,
                                   CagdCrvStruct *CrvX,
                                   CagdCrvStruct *CrvY,
                                   CagdCrvStruct *CrvZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvUnitLenScalar( CagdCrvStruct *OrigCrv,
                                    int Mult,
                                    double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvUnitLenCtlPts( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvSqrtScalar( CagdCrvStruct *OrigCrv,
                                 double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvArcLenSclrCrv( CagdCrvStruct *Crv,
                                    double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvArcLenCrv( CagdCrvStruct *Crv,
                                double Fineness,
                                int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbCrvArcLen( CagdCrvStruct *Crv, double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbCrvArcLen2( CagdCrvStruct *Crv, double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvArcLenSteps( CagdCrvStruct *Crv,
                                 double Length,
                                 double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbCrvMonotoneCtlPt( CagdCrvStruct *Crv, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbComposeCrvCrv( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbComposePeriodicCrvCrv( CagdCrvStruct *Crv1,
                                          CagdCrvStruct *Crv2,
                                         double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbComposeSrfSetCache(int Cache);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbComposeSrfClrCache( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbComposeSrfCrv( CagdSrfStruct *Srf,
                                  CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbComposeSrfPatch( CagdSrfStruct *Srf,
                                    IrtUVType UV00,
                                    IrtUVType UV01,
                                    IrtUVType UV10,
                                    IrtUVType UV11);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbComposePeriodicSrfCrv( CagdSrfStruct *Srf,
                                          CagdCrvStruct *Crv,
                                         double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbComposeSrfSrf( CagdSrfStruct *Srf1,
                                  CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbMapUVCrv2E3( CagdCrvStruct *Crv,
                                CagdSrfStruct *Srf,
                               int Compose);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct **SymbComputeCurvePowers( CagdCrvStruct *Crv, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct **SymbComputeSurfacePowers( CagdSrfStruct *CSrf, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbDecomposeCrvCrv(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *SymbComposeTileObjectInSrf(  IPObjectStruct
                                                                       *PTile,
                                                   CagdSrfStruct
                                                                   *DeformSrf,
                                                  double UTimes,
                                                  double VTimes,
                                                  int FitObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbDistCrvPoint( CagdCrvStruct *Crv,
                           void *CrvPtPrepHandle,
                            IrtPtType* Pt,
                           int MinDist,
                           double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *SymbDistCrvPointPrep( CagdCrvStruct *CCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbDistCrvPointFree(void *CrvPtPrepHandle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbLclDistCrvPoint( CagdCrvStruct *Crv,
                                  void *CrvPtPrepHandle,
                                   double *Pt,
                                  double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbDistCrvLine( CagdCrvStruct *Crv,
                           IrtLnType* Line,
                          int MinDist,
                          double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbLclDistCrvLine( CagdCrvStruct *Crv,
                                  IrtLnType* Line,
                                 double Epsilon,
                                 int InterPos,
                                 int ExtremPos);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbCrvPointInclusion( CagdCrvStruct *Crv,
                           IrtPtType* Pt,
                          double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvRayInter( CagdCrvStruct *Crv,
                               IrtPtType* RayPt,
                               IrtVecType* RayDir,
                              double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbDistBuildMapToCrv( CagdCrvStruct *Crv,
                                double Tolerance,
                                double *XDomain,
                                double *YDomain,
                                double **DiscMap,
                                double DiscMapXSize,
                                double DiscMapYSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvZeroSet( CagdCrvStruct *Crv,
                             int Axis,
                             double Epsilon,
                             int NoSolsOnEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvExtremSet( CagdCrvStruct *Crv,
                               int Axis,
                               double Epsilon,
                               int NoSolsOnEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvConstSet( CagdCrvStruct *Crv,
                              int Axis,
                              double Epsilon,
                              double ConstVal,
                              int NoSolsOnEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbScalarCrvLowDegZeroSet(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbCrvPosNegWeights( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbSplitRationalCrvsPoles( CagdCrvStruct *Crv,
                                         double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvSplitPoleParams( CagdCrvStruct *Crv,
                                      double Eps,
                                      double OutReach);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvsSplitPoleParams( CagdCrvStruct *Crvs,
                                       double Eps,
                                       double OutReach);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvOffset( CagdCrvStruct *Crv,
                             double OffsetDist,
                             int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvVarOffset( CagdCrvStruct *Crv,
                                 CagdCrvStruct *VarOffsetDist,
                                int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvSubdivOffset( CagdCrvStruct *Crv,
                                   double OffsetDist,
                                   double Tolerance,
                                   int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfCloseParallelSrfs2Shell( CagdSrfStruct *Srf1,
                                               CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvAdapOffset( CagdCrvStruct *OrigCrv,
                                 double OffsetDist,
                                 double OffsetError,
                                 SymbOffCrvFuncType OffsetAprxFunc,
                                 int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvAdapVarOffset( CagdCrvStruct *OrigCrv,
                                     CagdCrvStruct *VarOffsetDist,
                                    double OffsetError,
                                    SymbVarOffCrvFuncType VarOffsetAprxFunc,
                                    int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvAdapOffsetTrim( CagdCrvStruct *OrigCrv,
                                     double OffsetDist,
                                     double OffsetError,
                                     SymbOffCrvFuncType OffsetAprxFunc,
                                     int BezInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvLeastSquarOffset( CagdCrvStruct *Crv,
                                       double OffsetDist,
                                       int NumOfSamples,
                                       int NumOfDOF,
                                       int Order,
                                       double *Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMatchingOffset(CagdCrvStruct *Crv,
                                     double OffsetDist,
                                     double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvTrimGlblOffsetSelfInter( CagdCrvStruct *Crv,
                                               CagdCrvStruct *OffCrv,
                                              double SubdivTol,
                                              double TrimAmount,
                                              double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsOffsetLclSelfInters(CagdCrvStruct *Crv,
                              CagdCrvStruct *OffCrv,
                              CagdPtStruct **SIDmns);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvCrvConvolution(CagdCrvStruct *Crv1,
                                     CagdCrvStruct *Crv2,
                                     double OffsetDist,
                                     double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbEnvOffsetFromCrv( CagdCrvStruct *Crv,
                                    double Height,
                                    double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbUniformAprxPtOnCrvDistrib( CagdCrvStruct *Crv,
                                         int ParamUniform,
                                         int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbGetParamListAndReset();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbInsertNewParam(double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbInsertNewParam2(CagdPtStruct *PtList, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbEvalCrvCurvPrep(CagdCrvStruct *Crv, int Init);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbEvalCrvCurvature( CagdCrvStruct *Crv, double t, double *k);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbEvalCrvCurvTN(IrtVecType* Nrml, IrtVecType* Tan, int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *SymbSrf2Polygons( CagdSrfStruct *Srf,
                                    int FineNess,
                                    int ComputeNormals,
                                    int FourPerFlat,
                                    int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfAdd( CagdSrfStruct *Srf1,
                           CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfSub( CagdSrfStruct *Srf1,
                           CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMult( CagdSrfStruct *Srf1, 
                            CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfInvert( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfScalarScale( CagdSrfStruct *Srf,
                                  double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDotProd( CagdSrfStruct *Srf1,
                               CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfVecDotProd( CagdSrfStruct *Srf,
                                  IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMultScalar( CagdSrfStruct *Srf1,
                                  CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfCrossProd( CagdSrfStruct *Srf1,
                                 CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfVecCrossProd( CagdSrfStruct *Srf,
                                    IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfRtnlMult( CagdSrfStruct *Srf1X,
                                CagdSrfStruct *Srf1W,
                                CagdSrfStruct *Srf2X,
                                CagdSrfStruct *Srf2W,
                               int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDeriveRational( CagdSrfStruct *Srf,
                                     CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfNormalSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *Symb2DSrfJacobian( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbMeshAddSub(double **DestPoints,
                    double *  *Points1,
                    double *  *Points2,
                    CagdPointType PType,
                    int Size,
                    int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbMeshAddSubTo(double **DestPoints,
                      double *  *Points2,
                      CagdPointType PType,
                      int Size,
                      int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct **SymbSrfSplitScalarN( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrfSplitScalar( CagdSrfStruct *Srf,
                        CagdSrfStruct **SrfW,
                        CagdSrfStruct **SrfX,
                        CagdSrfStruct **SrfY,
                        CagdSrfStruct **SrfZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMergeScalarN(CagdSrfStruct *  *SrfVec, 
                                   int NumSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMergeScalar( CagdSrfStruct *SrfW,
                                   CagdSrfStruct *SrfX,
                                   CagdSrfStruct *SrfY,
                                   CagdSrfStruct *SrfZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbPrmtSclrCrvTo2D( CagdCrvStruct *Crv,
                                   double Min,
                                   double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbPrmtSclrSrfTo3D( CagdSrfStruct *Srf,
                                   double UMin, double UMax,
                                   double VMin, double VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbExtremumCntPtVals(double *  *Points,
                                 int Length,
                                 int FindMinimum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbSetAdapIsoExtractMinLevel(int MinLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbAdapIsoExtract( CagdSrfStruct *Srf,
                                   CagdSrfStruct *NSrf,
                                  SymbAdapIsoDistSqrFuncType AdapIsoDistFunc,
                                  CagdSrfDirType Dir,
                                  double Eps,
                                  int FullIso,
                                  int SinglePath);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *SymbAdapIsoExtractRectRgns( CagdSrfStruct *Srf,
                                                  CagdSrfDirType Dir,
                                                  double Size,
                                                  int Smoothing,
                                                  int OutputType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbAdapIsoSetWeightPt(double *Pt, double Scale, double Width);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfVolume1Srf( CagdSrfStruct *Srf,
                                 int Integrate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrfVolume1( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfVolume2Srf( CagdSrfStruct *Srf,
                                 int Integrate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrfVolume2( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfFirstMomentSrf( CagdSrfStruct *Srf,
                                     int Axis,
                                     int Integrate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrfFirstMoment( CagdSrfStruct *Srf, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfSecondMomentSrf( CagdSrfStruct *Srf,
                                      int Axis1,
                                      int Axis2,
                                      int Integrate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrfSecondMoment( CagdSrfStruct *Srf, int Axis1, int Axis2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrfFff( CagdSrfStruct *Srf,
                CagdSrfStruct **DuSrf,
                CagdSrfStruct **DvSrf,
                CagdSrfStruct **FffG11,
                CagdSrfStruct **FffG12,
                CagdSrfStruct **FffG22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrfSff( CagdSrfStruct *DuSrf,
                 CagdSrfStruct *DvSrf,
                CagdSrfStruct **SffL11,
                CagdSrfStruct **SffL12,
                CagdSrfStruct **SffL22,
                CagdSrfStruct **SNormal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrfTff( CagdSrfStruct *Srf,
                CagdSrfStruct **TffL11,
                CagdSrfStruct **TffL12,
                CagdSrfStruct **TffL22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDeterminant2( CagdSrfStruct *Srf11,
                                    CagdSrfStruct *Srf12,
                                    CagdSrfStruct *Srf21,
                                    CagdSrfStruct *Srf22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDeterminant3( CagdSrfStruct *Srf11,
                                    CagdSrfStruct *Srf12,
                                    CagdSrfStruct *Srf13,
                                    CagdSrfStruct *Srf21,
                                    CagdSrfStruct *Srf22,
                                    CagdSrfStruct *Srf23,
                                    CagdSrfStruct *Srf31,
                                    CagdSrfStruct *Srf32,
                                    CagdSrfStruct *Srf33);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvDeterminant2( CagdCrvStruct *Crv11,
                                    CagdCrvStruct *Crv12,
                                    CagdCrvStruct *Crv21,
                                    CagdCrvStruct *Crv22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvDeterminant3( CagdCrvStruct *Crv11,
                                    CagdCrvStruct *Crv12,
                                    CagdCrvStruct *Crv13,
                                    CagdCrvStruct *Crv21,
                                    CagdCrvStruct *Crv22,
                                    CagdCrvStruct *Crv23,
                                    CagdCrvStruct *Crv31,
                                    CagdCrvStruct *Crv32,
                                    CagdCrvStruct *Crv33);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfGaussCurvature( CagdSrfStruct *Srf,
                                     int NumerOnly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMeanNumer( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMeanCurvatureSqr( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfMeanEvolute( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfIsoFocalSrf( CagdSrfStruct *Srf,
                                  CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfCurvatureUpperBound( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfIsoDirNormalCurvatureBound( CagdSrfStruct *Srf,
                                                 CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDistCrvCrv( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2,
                                 int DistType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbSrfDistFindPoints( CagdSrfStruct *Srf,
                                    double Epsilon,
                                    int SelfInter);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvCrvInter( CagdCrvStruct *Crv1,
                               CagdCrvStruct *Crv2,
                              double CCIEpsilon,
                              int SelfInter);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConicDistCrvCrv( CagdCrvStruct *Crv1,
                                    CagdCrvStruct *Crv2,
                                   double Dist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbHausDistOfSamplefPts(IrtPtType*   V1,
                                   IrtPtType*   V2,
                                   int V1Size,
                                   int V2Size,
                                   int HDistSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbHausDistBySamplesCrvCrv( CagdCrvStruct *Crv1,
                                       CagdCrvStruct *Crv2,
                                      int Samples,
                                      int HDistSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbHausDistBySamplesCrvSrf( CagdCrvStruct *Crv1,
                                       CagdSrfStruct *Srf2,
                                      int Samples,
                                      int HDistSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbHausDistBySamplesSrfSrf( CagdSrfStruct *Srf1,
                                       CagdSrfStruct *Srf2,
                                      int Samples,
                                      int HDistSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbTwoCrvsMorphing( CagdCrvStruct *Crv1,
                                    CagdCrvStruct *Crv2,
                                   double Blend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbTwoCrvsMorphingCornerCut( CagdCrvStruct *Crv1,
                                             CagdCrvStruct *Crv2,
                                            double MinDist,
                                            int SameLength,
                                            int FilterTangencies);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbTwoCrvsMorphingMultiRes( CagdCrvStruct *Crv1,
                                            CagdCrvStruct *Crv2,
                                           double BlendStep);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbTwoSrfsMorphing( CagdSrfStruct *Srf1,
                                    CagdSrfStruct *Srf2,
                                   double Blend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrf2OptPolysCurvatureErrorPrep( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrf2OptPolysIsoDirCurvatureErrorPrep( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrf2OptPolysCurvatureError(CagdSrfStruct *Srf,
                                         CagdSrfDirType Dir,
                                         int SubdivLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrf2OptPolysBilinPolyError(CagdSrfStruct *Srf,
                                         CagdSrfDirType Dir,
                                         int SubdivLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *SymbSrf2OptimalPolygons(
                                CagdSrfStruct *Srf,
                                double Tolerance,
                                SymbPlSubdivStrategyType SubdivDirStrategy,
                                SymbPlErrorFuncType SrfPolyApproxErr,
                                int ComputeNormals,
                                int FourPerFlat,
                                int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfOffset( CagdSrfStruct *Srf, double OffsetDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfSubdivOffset( CagdSrfStruct *Srf,
                                   double OffsetDist,
                                   double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtUVType *SymbUniformAprxPtOnSrfDistrib(
                         CagdSrfStruct *Srf,
                        int ParamUniform,
                        int n,
                        SymbUniformAprxSrfPtImportanceFuncType EvalImportance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbUniformAprxPtOnSrfPrepDistrib( CagdSrfStruct *Srf, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtUVType *SymbUniformAprxPtOnSrfGetDistrib( CagdSrfStruct *Srf, int *n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbEvalSrfCurvPrep(CagdSrfStruct *Srf, int Init);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbEvalSrfCurvature( CagdSrfStruct *Srf,
                         double U,
                         double V,
                         int DirInUV,
                         double *K1,
                         double *K2,
                         IrtVecType* D1,
                         IrtVecType* D2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbEvalSrfCurvTN(IrtVecType* Nrml,
                       IrtVecType* DSrfU,
                       IrtVecType* DSrfV,
                       int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbEvalSrfAsympDir( CagdSrfStruct *Srf,
                        double U,
                        double V,
                        int DirInUV,
                        IrtVecType* AsympDir1,
                        IrtVecType* AsympDir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvMult( CagdCrvStruct *Crv1,
                           CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrCrvMultPtsVecs( double *Pts1,
                       int Order1,
                        double *Pts2,
                       int Order2,
                       double *ProdPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvMultList( CagdCrvStruct *Crv1Lst,
                               CagdCrvStruct *Crv2Lst);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvFactorT( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvFactor1MinusT( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbApproxLowDegStateType SymbApproxCrvsLowDegState(
                                             SymbApproxLowDegStateType State);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbApproxCrvAsBzrCubics( CagdCrvStruct *Crv,
                                        double Tol,
                                        double MaxLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbApproxCrvAsBzrQuadratics( CagdCrvStruct *Crv,
                                            double Tol,
                                            double MaxLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrComposeCrvCrv( CagdCrvStruct *Crv1,
                                 CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrComposeSrfCrv( CagdSrfStruct *Srf,
                                 CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrComposeSrfCrvInterp( CagdSrfStruct *Srf,
                                       CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrComposeSrfSrf( CagdSrfStruct *Srf1,
                                 CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfSubdivAtCurve( CagdSrfStruct *Srf,
                                    CagdCrvStruct *DivCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern BspMultComputationMethodType BspMultComputationMethod(
                                   BspMultComputationMethodType BspMultMethod);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvMult( CagdCrvStruct *Crv1,
                           CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvBlossomMult( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbBspBasisInnerProdPrep( double *KV,
                               int Len,
                               int Order1,
                               int Order2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbBspBasisInnerProdPrep2( double *KV1,
                                 double *KV2,
                                int Len1,
                                int Len2,
                                int Order1,
                                int Order2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbBspBasisInnerProd(int Index1, int Index2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double **SymbBspBasisInnerProdMat( double *KV,
                                     int Len,
                                     int Order1,
                                     int Order2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbBspBasisInnerProd2( double *KV,
                                 int Len,
                                 int Order1,
                                 int Order2,
                                 int Index1,
                                 int Index2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfMult( CagdSrfStruct *Srf1,
                           CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfFactorBilinear( CagdSrfStruct *Srf,
                                     double *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfFactorUMinusV( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrSrfFactorLowOrders( CagdSrfStruct *Srf,
                           CagdSrfStruct **S11,
                           CagdSrfStruct **S12,
                           CagdSrfStruct **S21,
                           CagdSrfStruct **S22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfFactorExtremeRowCol( CagdSrfStruct *Srf,
                                         CagdSrfBndryType Bndry);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfMult( CagdSrfStruct *Srf1,
                           CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfBlossomMult( CagdSrfStruct *Srf1,
                                  CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfFactorBilinear( CagdSrfStruct *Srf,
                                     double *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfFactorUMinusV( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbAllPrisaSrfs( CagdSrfStruct *Srfs,
                                int SamplesPerCurve,
                                double Epsilon,
                                CagdSrfDirType Dir,
                                 IrtVecType* Space);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbPiecewiseRuledSrfApprox( CagdSrfStruct *Srf,
                                           int ConsistentDir,
                                           double Epsilon,
                                           CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbPrisaRuledSrf( CagdSrfStruct *Srf,
                                 int SamplesPerCurve,
                                 double Space,
                                 IrtVecType* Offset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbPrisaGetCrossSections( CagdSrfStruct *RSrfs,
                                         CagdSrfDirType Dir,
                                          IrtVecType* Space);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbPrisaGetOneCrossSection( CagdSrfStruct *RSrf,
                                           CagdSrfDirType Dir,
                                           int Starting,
                                           int Ending);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbCrvMultiResKVBuild( CagdCrvStruct *Crv,
                           int Discont,
                           double ***KVList,
                           int **KVListSizes,
                           int *KVListSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbMultiResCrvStruct *SymbCrvMultiResDecomp( CagdCrvStruct *Crv,
                                             int Discont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbMultiResCrvStruct *SymbCrvMultiResDecomp2( CagdCrvStruct *Crv,
                                              int Discont,
                                              int SameSpace);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMultiResCompos( SymbMultiResCrvStruct *MRCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMultiResComposAtT( SymbMultiResCrvStruct *MRCrv,
                                        double T);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbCrvMultiResEdit( SymbMultiResCrvStruct *MRCrv,
                         double t,
                          IrtVecType* TransDir,
                         double Level,
                         double FracLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbCrvMultiResRefineLevel(SymbMultiResCrvStruct *MRCrv,
                                      double T,
                                      int SpanDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvMultiResBWavelet(double *KV,
                                       int Order,
                                       int Len,
                                       int KnotIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbCrvMultiResFree(SymbMultiResCrvStruct *MRCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbMultiResCrvStruct *SymbCrvMultiResNew(int Levels, int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbMultiResCrvStruct *SymbCrvMultiResCopy( SymbMultiResCrvStruct
                                                                  *MRCrvOrig);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvCnvxHull( CagdCrvStruct *Crv, double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvListCnvxHull(CagdCrvStruct *Crvs, double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *SymbCrvDiameter( CagdCrvStruct *Crv,
                                        double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbCrvDiameterMinMax( CagdCrvStruct *Crv,
                                  IPPolygonStruct *Cntrs,
                                 int Min);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCrvPtTangents( CagdCrvStruct *Crv,
                                 IrtPtType* Pt,
                                double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbTangentToCrvAtTwoPts( CagdCrvStruct *Crv,
                                       double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *SymbCircTanTo2Crvs( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2,
                                 double Radius,
                                 double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbBsctComputeInterMidPoint( CagdCrvStruct *Crv1,
                                        double t1,
                                         CagdCrvStruct *Crv2,
                                        double t2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvBisectors( CagdCrvStruct *Crv,
                                int BisectFunc,
                                double SubdivTol,
                                int NumerImprove,
                                int SameNormal,
                                int SupportPrms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvBisectorsSrf( CagdCrvStruct *Crv, int BisectFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvBisectorsSrf2( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvBisectorsSrf3( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvCrvBisectorSrf3D( CagdCrvStruct *Crv1,
                                        CagdCrvStruct *Crv2,
                                       double Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvBisectorsSrf3D(CagdSrfStruct *Srf1,
                                     CagdSrfStruct *Srf2,
                                     CagdSrfStruct *DSrf1,
                                     CagdSrfStruct *DSrf2,
                                     double Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvPtBisectorCrv2D( CagdCrvStruct *Crv,
                                       IrtPtType* Pt,
                                      double Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvPtBisectorSrf3D( CagdCrvStruct *Crv,
                                       IrtPtType* Pt,
                                      double RulingScale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfPtBisectorSrf3D( CagdSrfStruct *Srf,
                                       IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbPtCrvBisectOnSphere( IrtPtType* Pt,
                                        CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbPtCrvBisectOnSphere2( IrtPtType* Pt,
                                         CagdCrvStruct *Crv,
                                        double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvCrvBisectOnSphere( CagdCrvStruct *Crv1,
                                         CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvCrvBisectOnSphere2( CagdCrvStruct *Crv1,
                                          CagdCrvStruct *Crv2,
                                         double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCrvCrvBisectOnSphere3( CagdCrvStruct *Crv1,
                                          CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbPlanePointBisect( IrtPtType* Pt, double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCylinPointBisect( IrtPtType* CylPt,
                                     IrtVecType* CylDir,
                                    double CylRad,
                                     IrtPtType* Pt,
                                    double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConePointBisect( IrtPtType* ConeApex,
                                    IrtVecType* ConeDir,
                                   double ConeAngle,
                                    IrtPtType* Pt,
                                   double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSpherePointBisect( IrtPtType* SprCntr,
                                     double SprRad,
                                      IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbTorusPointBisect( IrtPtType* TrsCntr,
                                     IrtVecType* TrsDir,
                                    double TrsMajorRad,
                                    double TrsMinorRad,
                                     IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbPlaneLineBisect( IrtVecType* LineDir, double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConeLineBisect( IrtVecType* ConeDir,
                                  double ConeAngle,
                                   IrtVecType* LineDir,
                                  double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSphereLineBisect( IrtPtType* SprCntr,
                                    double SprRad,
                                    double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSpherePlaneBisect( IrtPtType* SprCntr,
                                     double SprRad,
                                     double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCylinPlaneBisect( IrtPtType* CylPt,
                                     IrtVecType* CylDir,
                                    double CylRad,
                                    double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConePlaneBisect( IrtPtType* ConeApex,
                                    IrtVecType* ConeDir,
                                   double ConeAngle,
                                   double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCylinSphereBisect( IrtPtType* CylPt,
                                      IrtVecType* CylDir,
                                     double CylRad,
                                      IrtPtType* SprCntr,
                                     double SprRad,
                                     double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSphereSphereBisect( IrtPtType* SprCntr1,
                                      double SprRad1,
                                       IrtPtType* SprCntr2,
                                      double SprRad2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConeSphereBisect( IrtPtType* ConeApex,
                                     IrtVecType* ConeDir,
                                    double ConeAngle,
                                     IrtPtType* SprCntr,
                                    double SprRad,
                                    double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConeConeBisect( IrtVecType* Cone1Dir,
                                  double Cone1Angle,
                                   IrtVecType* Cone2Dir,
                                  double Cone2Angle,
                                  double Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbTorusSphereBisect( IrtPtType* TrsCntr,
                                      IrtVecType* TrsDir,
                                     double TrsMajorRad,
                                     double TrsMinorRad,
                                      IrtPtType* SprCntr,
                                     double SprRad);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbTorusTorusBisect( IrtVecType* Trs1Cntr,
                                     IrtVecType* Trs1Dir,
                                    double Trs1MajorRad,
                                     IrtVecType* Trs2Cntr,
                                     IrtVecType* Trs2Dir,
                                    double Trs2MajorRad,
                                    double Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbCylinCylinBisect( IrtVecType* Cyl1Pos,
                                     IrtVecType* Cyl1Dir,
                                    double Cyl1Rad,
                                     IrtVecType* Cyl2Pos,
                                     IrtVecType* Cyl2Dir,
                                    double Cyl2Rad);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConeConeBisect2( IrtVecType* Cone1Pos,
                                    IrtVecType* Cone1Dir,
                                   double Cone1Angle,
                                    IrtVecType* Cone2Pos,
                                    IrtVecType* Cone2Dir,
                                   double Cone2Angle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbConeCylinBisect( IrtVecType* Cone1Pos,
                                    IrtVecType* Cone1Dir,
                                   double Cone1Angle,
                                    IrtVecType* Cyl2Pos,
                                    IrtVecType* Cyl2Dir,
                                   double Cyl2Rad);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  SymbNormalConeStruct *SymbTangentConeForCrv( CagdCrvStruct *Crv,
                                                  int Planar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  SymbNormalConeStruct *SymbNormalConeForSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbNormalConeForSrfDoOptimal(int Optimal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  SymbNormalConeStruct *SymbNormalConeForSrfAvg( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  SymbNormalConeStruct *SymbNormalConeForSrfOpt( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  SymbNormalConeStruct *SymbNormalConeForSrfMainAxis(
                                                   CagdSrfStruct *Srf,
                                                  IrtVecType* MainAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbNormal2ConesForSrf( CagdSrfStruct *Srf,
                           double ExpandingFactor,
                           SymbNormalConeStruct *Cone1,
                           SymbNormalConeStruct *Cone2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbNormalConeOverlap( SymbNormalConeStruct *NormalCone1,
                                 SymbNormalConeStruct *NormalCone2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbNormalConeStruct *SymbNormalConvexHullConeForSrf( CagdSrfStruct *Srf,
                                                     double ***CH,
                                                     int *NPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbNormalConvexHullConeOverlap( SymbNormalConeStruct
                                                                *NormalCone1,
                                           double **CH1,
                                          int NPts1,
                                           SymbNormalConeStruct
                                                                *NormalCone2,
                                           double **CH2,
                                          int NPts2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbRflctLnPrepSrf(CagdSrfStruct *Srf,
                         IrtVecType* ViewDir,
                         IrtVecType* LnDir,
                         byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbRflctLnGen(CagdSrfStruct *Srf,
                               IrtVecType* ViewDir,
                               IrtPtType* LnPt,
                               IrtVecType* LnDir,
                               byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbRflctLnFree(CagdSrfStruct *Srf,  byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbRflctCircPrepSrf(CagdSrfStruct *Srf,
                           IrtVecType* ViewDir,
                           IrtPtType* SprCntr,
                           byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbRflctCircGen(CagdSrfStruct *Srf,
                                 IrtVecType* ViewDir,
                                 IrtPtType* SprCntr,
                                double ConeAngle,
                                 byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbRflctCircFree(CagdSrfStruct *Srf,  byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbHighlightLnPrepSrf(CagdSrfStruct *Srf,
                             IrtVecType* LnDir,
                             byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbHighlightLnGen(CagdSrfStruct *Srf,
                                   IrtPtType* LnPt,
                                   IrtVecType* LnDir,
                                   byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbHighlightLnFree(CagdSrfStruct *Srf,  byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvOrthotomic( CagdCrvStruct *Crv,
                                  IrtPtType* P,
                                 double K);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfOrthotomic( CagdSrfStruct *Srf,
                                  IrtPtType* P,
                                 double K);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *SymbSrfSilhouette( CagdSrfStruct *Srf,
                                           IrtVecType* VDir,
                                          double SubdivTol,
                                          int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *SymbSrfPolarSilhouette( CagdSrfStruct *Srf,
                                                IrtVecType* VDir,
                                               double SubdivTol,
                                               int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *SymbSrfIsocline( CagdSrfStruct *Srf,
                                         IrtVecType* VDir,
                                        double Theta,
                                        double SubdivTol,
                                        int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbAlgebraicSumSrf( CagdCrvStruct *Crv1,
                                    CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbAlgebraicProdSrf( CagdCrvStruct *Crv1,
                                     CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSwungAlgSumSrf( CagdCrvStruct *Crv1,
                                   CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvDual( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDual( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDevelopableCrvOnSrf( CagdSrfStruct *Srf,
                                           CagdCrvStruct *Crv,
                                          double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDevelopableSrfBetweenFrames( IrtVecType* Frame1Pos,
                                                   IrtVecType* Frame1Tan,
                                                   IrtVecType* Frame1Nrml,
                                                   IrtVecType* Frame2Pos,
                                                   IrtVecType* Frame2Tan,
                                                   IrtVecType* Frame2Nrml,
                                                  double OtherScale,
                                                  double Tension);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbSrfDevelopableSrfBetweenFrames2( IrtVecType* Frame1Pos,
                                                    IrtVecType* Frame1Tan,
                                                    IrtVecType* Frame1Nrml,
                                                    IrtVecType* Frame2Pos,
                                                    IrtVecType* Frame2Tan,
                                                    IrtVecType* Frame2Nrml,
                                                   double OtherScale,
                                                   double Tension,
                                                   int DOFs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbRuledRuledZeroSetFunc(CagdCrvStruct *C1,
                                         CagdCrvStruct *C2,
                                         CagdCrvStruct *D1,
                                         CagdCrvStruct *D2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbRuledRuledIntersection(CagdCrvStruct *C1,
                                          CagdCrvStruct *C2,
                                          CagdCrvStruct *D1,
                                          CagdCrvStruct *D2,
                                          double SubdivTol,
                                          CagdCrvStruct **PCrvs1,
                                          CagdCrvStruct **PCrvs2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbRingRingIntersection(CagdCrvStruct *C1,
                                        CagdCrvStruct *r1,
                                        CagdCrvStruct *C2,
                                        CagdCrvStruct *r2,
                                        double SubdivTol,
                                        CagdCrvStruct **PCrvs1,
                                        CagdCrvStruct **PCrvs2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbRingRingZeroSetFunc(CagdCrvStruct *C1,
                                       CagdCrvStruct *r1,
                                       CagdCrvStruct *C2,
                                       CagdCrvStruct *r2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbRmKntBspCrvRemoveKnots( CagdCrvStruct *Crv,
                                          double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbRmKntBspCrvCleanKnots( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsConstCrv( CagdCrvStruct *Crv,
                         CagdCtlPtStruct **ConstVal,
                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsZeroCrv( CagdCrvStruct *Crv, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsCircularCrv( CagdCrvStruct *Crv,
                            IrtPtType* Center,
                            double *Radius,
                            double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsLineCrv( CagdCrvStruct *Crv,
                        IrtPtType* LnPos,
                        IrtVecType* LnDir,
                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsConstSrf( CagdSrfStruct *Srf,
                         CagdCtlPtStruct **ConstVal,
                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsZeroSrf( CagdSrfStruct *Srf, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsSphericalSrf( CagdSrfStruct *Srf,
                             IrtPtType* Center,
                             double *Radius,
                             double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsExtrusionSrf( CagdSrfStruct *Srf,
                       CagdCrvStruct **Crv,
                       IrtVecType* ExtDir,
                       double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsDevelopSrf( CagdSrfStruct *Srf, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsRuledSrf( CagdSrfStruct *Srf,
                   CagdCrvStruct **Crv1,
                   CagdCrvStruct **Crv2,
                   double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsSrfOfRevSrf( CagdSrfStruct *Srf,
                            CagdCrvStruct **CrossSec,
                            IrtPtType* AxisPos,
                            IrtVecType* AxisDir,
                            double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbIsPlanarSrf( CagdSrfStruct *Srf,
                          IrtPlnType* Plane,
                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbClipCrvToSrfDomain( CagdSrfStruct *Srf,
                                       CagdCrvStruct *UVCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbShapeBlendOnSrf(CagdSrfStruct *Srf,
                                   CagdCrvStruct *UVCrv,
                                    CagdCrvStruct *CrossSecShape,
                                   double TanScale,
                                   double Width,
                                    CagdCrvStruct *WidthField,
                                   double Height,
                                    CagdCrvStruct *HeightField);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *SymbShapeBlendSrf( CagdCrvStruct *Pos1Crv,
                                  CagdCrvStruct *Pos2Crv,
                                  CagdCrvStruct *Dir1Crv,
                                  CagdCrvStruct *Dir2Crv,
                                  CagdCrvStruct *CrossSecShape,
                                  CagdCrvStruct *Normal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbArcStruct *SymbCrvBiArcApprox( CagdCrvStruct *Crv,
                                  double Tolerance,
                                  double MaxAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbArcs2Crvs( SymbArcStruct *Arcs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvCubicApprox( CagdCrvStruct *CCrv,
                                  double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbCrvRelType SymbCrvsCompare( CagdCrvStruct *Crv1,
                                CagdCrvStruct *Crv2,
                               double Eps,
                               double *StartOvrlpPrmCrv1,
                               double *EndOvrlpPrmCrv1,
                               double *StartOvrlpPrmCrv2,
                               double *EndOvrlpPrmCrv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCanonicBzrCrv( CagdCrvStruct *Crv, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbBzrDegReduce( CagdCrvStruct *Crv, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvsLowerEnvelop( CagdCrvStruct *Crvs,
                                    double *Pt,
                                    double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbSplitCrvsAtExtremums( CagdCrvStruct *Crvs,
                                        int Axis,
                                         IrtPtType* Pt,
                                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *Symb2DCrvParameterizing2Crvs( CagdCrvStruct *Crv1,
                                             CagdCrvStruct *Crv2,
                                            int Dir,
                                            int ForceMatch);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *Symb2DCrvParamerize2Prms( CagdCrvStruct *Crv,
                                        double T1,
                                        double T2,
                                        int Dir,
                                        double *Error);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *Symb2DCrvParameterizeDomain( CagdCrvStruct *UVCrv,
                                           double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbSrfSmoothInternalCtlPts(CagdSrfStruct *Srf, double Weight);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbSrfJacobianImprove(CagdSrfStruct *Srf,
                                 double StepSize,
                                 int MaxIter);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbGet2CrvsIntersectionRegions( CagdCrvStruct *Crv1,
                                                CagdCrvStruct *Crv2,
                                               double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double SymbGet2CrvsIntersectionAreas( CagdCrvStruct *Crv1,
                                         CagdCrvStruct *Crv2,
                                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SymbGet2CrvsInterDAreaDCtlPts(CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2,
                                  double Eps,
                                  double **InterDomains,
                                  double **dAreadPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *SymbGetCrvSubRegionAlphaMatrix( CagdCrvStruct *Crv,
                                          double t1,
                                          double t2,
                                          int *Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *SymbCrvLstSqrAprxPlln( CagdCtlPtStruct *Polyline,
                                     double ExpectedError,
                                     int Order,
                                     double NumericTol,
                                     int ForceC1Continuity,
                                     double C1DiscontThresh,
                                     SymbCrvLSErrorMeasureType ErrMeasure);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbSetErrorFuncType SymbSetFatalErrorFunc(SymbSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *SymbDescribeError(SymbFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SymbFatalError(SymbFatalErrorType ErrID);
    }
}
