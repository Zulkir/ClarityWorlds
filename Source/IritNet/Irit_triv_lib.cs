using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void TrivSetErrorFuncType (TrivFatalErrorType ErrorFunc);
    public unsafe delegate void TrivTVTestingFuncType ( TrivTVStruct *TV,
                                                   TrivTVDirType *Dir,
                                                   double *t);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVNew(TrivGeomType GType,
                        CagdPointType PType,
                        int ULength,
                        int VLength,
                        int WLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVNew(int ULength,
                           int VLength,
                           int WLength,
                           int UOrder,
                           int VOrder,
                           int WOrder,
                           CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspPeriodicTVNew(int ULength,
                                   int VLength,
                                   int WLength,
                                   int UOrder,
                                   int VOrder,
                                   int WOrder,
                                   int UPeriodic,
                                   int VPeriodic,
                                   int WPeriodic,
                                   CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVNew(int ULength,
                           int VLength,
                           int WLength,
                           CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivPwrTVNew(int ULength,
                           int VLength,
                           int WLength,
                           CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVCopy( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVCopyList( TrivTVStruct *TVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVFree(TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVFreeList(TrivTVStruct *TVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTriangleStruct *TrivTriangleNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTriangleStruct *TrivTriangleCopy( TrivTriangleStruct *Triangle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTriangleStruct *TrivTriangleCopyList( TrivTriangleStruct
                                                                *TriangleList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTriangleFree(TrivTriangleStruct *Triangle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTriangleFreeList(TrivTriangleStruct *TriangleList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimBox(double MinX,
                            double MinY,
                            double MinZ,
                            double MaxX,
                            double MaxY,
                            double MaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimCylinder( IrtVecType* Center,
                                 double Radius,
                                 double Height,
                                 int Rational,
                                 double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimCone( IrtVecType* Center,
                             double Radius,
                             double Height,
                             int Rational,
                             double InternalCubeSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimCone2( IrtVecType* Center,
                              double MajorRadius,
                              double MinorRadius,
                              double Height,
                              int Rational,
                              double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimSphere( IrtVecType* Center,
                               double Radius,
                               int Rational,
                               double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivNSPrimTorus( IrtVecType* Center,
                              double MajorRadius,
                              double MinorRadius,
                              int Rational,
                              double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCnvrtBzr2BspTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCnvrtBsp2BzrTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVTransform(TrivTVStruct *TV, double *Translate, double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVMatTransform(TrivTVStruct *TV, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCoerceTVsTo( TrivTVStruct *TV, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCoerceTVTo( TrivTVStruct *TV, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVDomain( TrivTVStruct *TV,
                  double *UMin,
                  double *UMax,
                  double *VMin,
                  double *VMax,
                  double *WMin,
                  double *WMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVSetDomain(TrivTVStruct *TV,
                              double UMin,
                              double UMax,
                              double VMin,
                              double VMax,
                              double WMin,
                              double WMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVSetDomain2(TrivTVStruct *TV,
                               double Min,
                               double Max,
                               TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivParamInDomain( TrivTVStruct *TV,
                            double t,
                            TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivParamsInDomain( TrivTVStruct *TV,
                             double u,
                             double v,
                             double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrivTVEval( TrivTVStruct *TV,
                      double u,
                      double v,
                      double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrivTVEval2( TrivTVStruct *TV,
                       double u,
                       double v,
                       double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVEval3Prep( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrivTVEval3(double u,
                       double v,
                       double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrivSrfFromTV( TrivTVStruct *TV,
                             double t,
                             TrivTVDirType Dir,
                             int OrientBoundary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct **TrivBndrySrfsFromTV( TrivTVStruct *TV,
                                    int OrientBoundary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrivBndryEdgesFromTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *TrivBndryCrnrsFromTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrivSrfFromMesh( TrivTVStruct *TV,
                               int Index,
                               TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivSrfToMesh( CagdSrfStruct *Srf,
                   int Index,
                   TrivTVDirType Dir,
                   TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrivTVMultEval(double *UKnotVector,
                          double *VKnotVector,
                          double *WKnotVector,
                          int ULength,
                          int VLength,
                          int WLength,
                          int UOrder,
                          int VOrder,
                          int WOrder,
                          IrtPtType* Mesh,
                          IrtPtType* Params,
                          int NumOfParams,
                          int *RetSize,
                          CagdBspBasisFuncMultEvalType EvalType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVBlockEvalSetMesh(IrtPtType* Mesh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVBlockEvalStruct *TrivTVBlockEvalOnce(int i, int j, int k);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVBlockEvalDone();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVRegionFromTV( TrivTVStruct *TV,
                                 double t1,
                                 double t2,
                                 TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVRefineAtParams( TrivTVStruct *TV,
                                   TrivTVDirType Dir,
                                   int Replace,
                                   double *t,
                                   int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVKnotInsertNDiff( TrivTVStruct *TV,
                                       TrivTVDirType Dir,
                                       int Replace,
                                        double *t,
                                       int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVDerive( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVDeriveScalar( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVDerive( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVDeriveScalar( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVDerive( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVDeriveScalar( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVSubdivAtParam( TrivTVStruct *TV,
                                  double t,
                                  TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVSubdivAtAllDetectedLocations( TrivTVStruct *TV,
                                                 TrivTVTestingFuncType
                                                                TVTestFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVDegreeRaise( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVDegreeRaiseN( TrivTVStruct *TV,
                                 TrivTVDirType Dir,
                                 int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVDegreeRaise( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVDegreeRaise( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVBlossomDegreeRaise( TrivTVStruct *TV,
                                       TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVBlossomDegreeRaiseN( TrivTVStruct *TV,
                                        int NewUOrder,
                                        int NewVOrder,
                                        int NewWOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVReverseDir( TrivTVStruct *TV, TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVReverse2Dirs( TrivTVStruct *TV,
                                 TrivTVDirType Dir1,
                                 TrivTVDirType Dir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivMakeTVsCompatible(TrivTVStruct **TV1,
                                TrivTVStruct **TV2,
                                int SameUOrder,
                                int SameVOrder,
                                int SameWOrder,
                                int SameUKV,
                                int SameVKV,
                                int SameWKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVBBox( TrivTVStruct *TV, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVListBBox( TrivTVStruct *TVs, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *TrivTV2CtrlMesh( TrivTVStruct *Trivar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double TrivTVVolume( TrivTVStruct *TV, int VolType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVPointInclusionPrep(TrivTVStruct *TV, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVPointInclusion(TrivTVStruct *TV,  IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVPointInclusionFree(TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivInterpTrivar( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVInterpPts( TrivTVStruct *PtGrid,
                              int UOrder,
                              int VOrder,
                              int WOrder,
                              int TVUSize,
                              int TVVSize,
                              int TVWSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVInterpolate( TrivTVStruct *PtGrid,
                                int ULength,
                                int VLength,
                                int WLength,
                                int UOrder,
                                int VOrder,
                                int WOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVInterpScatPts( CagdCtlPtStruct *PtList,
                                  int USize,
                                  int VSize,
                                  int WSize,
                                  int UOrder,
                                  int VOrder,
                                  int WOrder,
                                  double *UKV,
                                  double *VKV,
                                  double *WKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivPromoteCrvToTV( CagdCrvStruct *Crv,
                                 TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivPromoteSrfToTV( CagdSrfStruct *Srf,
                                 TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVFromSrfs( CagdSrfStruct *SrfList,
                             int OtherOrder,
                             CagdEndConditionType OtherEC,
                             double *OtherParamVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrivTVInterpolateSrfsChordLenParams( CagdSrfStruct *SrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVInterpolateSrfs( CagdSrfStruct *SrfList,
                                    int OtherOrder,
                                    CagdEndConditionType OtherEC,
                                    CagdParametrizationType OtherParam,
                                    double *OtherParamVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivRuledTV( CagdSrfStruct *Srf1,
                           CagdSrfStruct *Srf2,
                          int OtherOrder,
                          int OtherLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTrilinearSrf( CagdPtStruct *Pt000,
                                CagdPtStruct *Pt001,
                                CagdPtStruct *Pt010,
                                CagdPtStruct *Pt011,
                                CagdPtStruct *Pt100,
                                CagdPtStruct *Pt101,
                                CagdPtStruct *Pt110,
                                CagdPtStruct *Pt111);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivExtrudeTV( CagdSrfStruct *Srf,
                             CagdVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivExtrudeTV2( CagdSrfStruct *Srf,
                              CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivZTwistExtrudeSrf( CagdSrfStruct *Srf,
                                   int Rational,
                                   double ZPitch);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVOfRev( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVOfRev2( CagdSrfStruct *Srf,
                           int PolyApprox,
                           double StartAngle,
                           double EndAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVOfRevPolynomialApprox( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVOfRevAxis( CagdSrfStruct *Srf,
                               TrivP4DType AxisPoint,
                               TrivV4DType AxisVector,
                              int PolyApprox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivEditSingleTVPt(TrivTVStruct *TV,
                                 CagdCtlPtStruct *CtlPt,
                                 int UIndex,
                                 int VIndex,
                                 int WIndex,
                                 int Write);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVsSame( TrivTVStruct *Tv1,
                       TrivTVStruct *Tv2,
                      double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVKnotHasC0Discont( TrivTVStruct *TV,
                                 TrivTVDirType *Dir,
                                 double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVMeshC0Continuous( TrivTVStruct *TV,
                                 TrivTVDirType Dir,
                                 int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVIsMeshC0DiscontAt( TrivTVStruct *TV,
                                  int Dir,
                                  double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVKnotHasC1Discont( TrivTVStruct *TV,
                                 TrivTVDirType *Dir,
                                 double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVMeshC1Continuous( TrivTVStruct *TV,
                                 TrivTVDirType Dir,
                                 int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVIsMeshC1DiscontAt( TrivTVStruct *TV,
                                  int Dir,
                                  double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBspTVHasBezierKVs( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBspTVHasOpenEC( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivDbg( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivDbgDsp( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCnvrtPeriodic2FloatTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivCnvrtFloat2OpenTV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVOpenEnd( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTwoTVsMorphing( TrivTVStruct *TV1,
                                  TrivTVStruct *TV2,
                                 double Blend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivFFDCtlMeshUsingTV(double **Points,
                           int Length,
                           CagdPointType PType,
                            TrivTVStruct *DeformTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *TrivFFDObjectTV( IPObjectStruct *PObj,
                                        TrivTVStruct *DeformTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *TrivFFDTileObjectInTV(  IPObjectStruct *PObj,
                                              TrivTVStruct *DeformTV,
                                             double UTimes,
                                             double VTimes,
                                             double WTimes,
                                             int FitObj,
                                             double CropBoundaries,
                                             double MaxEdgeLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivEvalTVCurvaturePrelude( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivEvalCurvature(IrtPtType* Pos,
                            double *PCurv1,
                            double *PCurv2,
                            IrtVecType* PDir1,
                            IrtVecType* PDir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivEvalGradient(IrtPtType* Pos, IrtVecType* Gradient);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivEvalTVCurvaturePostlude();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivPlaneFrom4Points( TrivP4DType Pt1,
                          TrivP4DType Pt2,
                          TrivP4DType Pt3,
                          TrivP4DType Pt4,
                         TrivPln4DType Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivVectCross3Vecs( TrivV4DType A,
                         TrivV4DType B,
                         TrivV4DType C,
                        TrivV4DType Res);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *TrivComposeTileObjectInTV(
                                              IPObjectStruct *PObj,
                                             TrivTVStruct *DeformTV,
                                            double UTimes,
                                            double VTimes,
                                            double WTimes,
                                            int FitObj,
                                            double CropBoundaries);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *TrivComposeTileObjectInTVBzr(
                                               IPObjectStruct *PObj,
                                              TrivTVStruct *DeformTV,
                                             double UTimes,
                                             double VTimes,
                                             double WTimes,
                                             int FitObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *TrivComposeOneObjectInTVBzr(
                                               IPObjectStruct *PObj,
                                              TrivTVStruct *DeformTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrivComposeTVCrv( TrivTVStruct *TV,
                                 CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrivBzrComposeTVCrv( TrivTVStruct *TV,
                                    CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrivComposeTVSrf( TrivTVStruct *TV,
                                 CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrivBzrComposeTVSrf( TrivTVStruct *TV,
                                    CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivComposeTVTV( TrivTVStruct *TV1,
                               TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimSrfStruct *TrivAdapIsoExtractSrfs( TrivTVStruct *Trivar,
                                             TrivTVDirType Dir,
                                             double Epsilon,
                                             int InitialDiv,
                                             double CntrEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrivAdapIsoExtractCrvs( TrivTVStruct *Trivar,
                                      TrivTVDirType SrfDir,
                                      double Epsilon,
                                      int InitialDiv,
                                      CagdSrfDirType CrvDir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivInverseQueryStruct *TrivPrepInverseQueries( TrivTVStruct
                                                                     *Trivar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivInverseQuery( TrivInverseQueryStruct *Handle,
                      double *XYZPos,
                     double *UVWParams,
                     int InitialGuess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivFreeInverseQueries( TrivInverseQueryStruct *Handle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVAdd( TrivTVStruct *TV1,  TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVSub( TrivTVStruct *TV1,  TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVMult( TrivTVStruct *TV1,  TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVInvert( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVMultScalar( TrivTVStruct *TV1,
                                TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVDotProd( TrivTVStruct *TV1,  TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVVecDotProd( TrivTVStruct *TV,  IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVCrossProd( TrivTVStruct *TV1,  TrivTVStruct *TV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVRtnlMult( TrivTVStruct *TV1X,
                              TrivTVStruct *TV1W,
                              TrivTVStruct *TV2X,
                              TrivTVStruct *TV2W,
                             int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct **TrivTVSplitScalarN( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivTVSplitScalar( TrivTVStruct *TV,
                       TrivTVStruct **TVW,
                       TrivTVStruct **TVX,
                       TrivTVStruct **TVY,
                       TrivTVStruct **TVZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVMergeScalarN(TrivTVStruct *  *TVVec, int NumTVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVMergeScalar( TrivTVStruct *TVW,
                                 TrivTVStruct *TVX,
                                 TrivTVStruct *TVY,
                                 TrivTVStruct *TVZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivAlgebraicSumTV( CagdCrvStruct *Crv,
                                  CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivAlgebraicProdTV( CagdCrvStruct *Crv,
                                   CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivSwungAlgSumTV( CagdCrvStruct *Crv,
                                 CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivSetErrorFuncType TrivSetFatalErrorFunc(TrivSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivFatalError(TrivFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *TrivDescribeError(TrivFatalErrorType ErrID);
    }
}
