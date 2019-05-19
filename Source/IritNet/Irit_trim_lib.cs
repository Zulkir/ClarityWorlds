using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void TrimSetErrorFuncType (TrimFatalErrorType ErrorFunc);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimCrvSegNew(CagdCrvStruct *UVCrv, CagdCrvStruct *EucCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimCrvSegNewList(CagdCrvStruct *UVCrvs,
                                    CagdCrvStruct *EucCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimCrvSegCopy( TrimCrvSegStruct *TrimCrvSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimCrvSegCopyList( TrimCrvSegStruct *TrimCrvSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvSegFree(TrimCrvSegStruct *TrimCrvSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvSegFreeList(TrimCrvSegStruct *TrimCrvSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimCrvNew(TrimCrvSegStruct *TrimCrvSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimCrvCopy( TrimCrvStruct *TrimCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimCrvCopyList( TrimCrvStruct *TrimCrvList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvFree(TrimCrvStruct *TrimCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvFreeList(TrimCrvStruct *TrimCrvList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfNew(CagdSrfStruct *Srf,
                          TrimCrvStruct *TrimCrvList,
                          int HasTopLvlTrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfNew2(CagdSrfStruct *Srf,
                           CagdCrvStruct *TrimCrvList,
                           int HasTopLvlTrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfNew3(CagdSrfStruct *Srf,
                           CagdCrvStruct *TrimCrvList,
                           int HasTopLvlTrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfFromE3TrimmingCurves(TrimCrvStruct *TCrvs,
                                            IrtPlnType* Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfVerifyTrimCrvsValidity(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfCopy( TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfCopyList( TrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfFree(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfFreeList(TrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfTransform(TrimSrfStruct *TrimSrf,
                      double *Translate,
                      double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfMatTransform(TrimSrfStruct *TrimSrf, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfsSame( TrimSrfStruct *TSrf1,
                        TrimSrfStruct *TSrf2,
                       double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimGetLargestTrimmedSrf(TrimSrfStruct **TSrfs, int Extract);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimCrvSegStruct *TrimGetOuterTrimCrv( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimCrvSegStruct *TrimGetFullDomainTrimCrv( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimGetTrimmingCurves( TrimSrfStruct *TrimSrf,
                                     int ParamSpace,
                                     int EvalEuclid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimGetTrimmingCurves2( TrimCrvStruct *TrimCrvList,
                                       TrimSrfStruct *TrimSrf,
                                      int ParamSpace,
                                      int EvalEuclid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimManageTrimmingCurves(TrimSrfStruct *TrimSrf,
                                        int FitOrder,
                                        int EvalEuclid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimLinkTrimmingCurves2Loops( TrimCrvStruct *TCrvs,
                                            double MaxTol,
                                            int *ClosedLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimLinkTrimmingCurves2Loops1( TrimCrvSegStruct *TSegs,
                                            double MaxTol,
                                            int *ClosedLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimLinkTrimmingCurves2Loops2(TrimCrvStruct *TCrvs,
                                             double Tol,
                                             int *ClosedLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimClassifyTrimCrvsOrientation(TrimCrvStruct *TCrvs, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimVerifyClosedTrimLoop(TrimCrvStruct *TCrv,
                                   double Tolerance,
                                   int CoerceIdentical);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimCoerceTrimUVCrv2Plane(TrimCrvSegStruct *TSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimMergeTrimmingCurves2Loops(TrimCrvStruct *TrimCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimMergeTrimmingCurves2Loops2(CagdCrvStruct *UVCrvs,
                                              double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimAffineTransTrimCurves(TrimCrvStruct *TrimCrvList,
                               double OldUMin,
                               double OldUMax,
                               double OldVMin,
                               double OldVMax,
                               double NewUMin,
                               double NewUMax,
                               double NewVMin,
                               double NewVMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimAffineTransTrimSrf( TrimSrfStruct *TrimSrf,
                                      double NewUMin,
                                      double NewUMax,
                                      double NewVMin,
                                      double NewVMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *TrimCrvs2Polylines(TrimSrfStruct *TrimSrf,
                                       int ParamSpace,
                                       double TolSamples,
                                       SymbCrvApproxMethodType Method);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *TrimCrv2Polyline( CagdCrvStruct *TrimCrv,
                                     double TolSamples,
                                     SymbCrvApproxMethodType Method,
                                     int OptiLin);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimEvalTrimCrvToEuclid( CagdSrfStruct *Srf,
                                        CagdCrvStruct *UVCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimEvalTrimCrvToEuclid2( CagdSrfStruct *Srf,
                                         CagdCrvStruct *UVCrv,
                                        CagdCrvStruct **UVCrvLinear);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSetEuclidLinearFromUV(int EuclidLinearFromUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSetEuclidComposedFromUV(int EuclidComposedFromUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrimPointInsideTrimmedCrvs(TrimCrvStruct *TrimCrvList,
                                       TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfTrimCrvSquareDomain( TrimCrvStruct *TrimCrvList,
                                     double *UMin,
                                     double *UMax,
                                     double *VMin,
                                     double *VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfTrimCrvAllDomain( TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimClipSrfToTrimCrvs(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfDegreeRaise( TrimSrfStruct *TrimSrf,
                                  CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfSetStateTrimCrvsManagement(int TrimmingFitOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfSubdivAtParam(TrimSrfStruct *TrimSrf,
                                    double t,
                                    CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfSubdivAtInnerLoops(TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimSrfSubdivTrimCrvsAtInnerLoops( TrimCrvStruct *TCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfDirType TrimSrfSubdivValAtInnerLoop( TrimCrvStruct *TCrvs,
                                           double *SubdivVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimCnvrtBsp2BzrSrf(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfCnvrt2BzrTrimSrf(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrimSrfCnvrt2BzrRglrSrf(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrimSrfCnvrt2BzrRglrSrf2( TrimSrfStruct *TSrf,
                                        int ComposeE3,
                                        int OnlyBzrSrfs,
                                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrimSrfCnvrt2TensorProdSrf( TrimSrfStruct *TSrf,
                                          int ComposeE3,
                                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfSubdivTrimmingCrvs( TrimCrvStruct *TrimCrvs,
                              double t,
                              CagdSrfDirType Dir,
                              TrimCrvStruct **TrimCrvs1,
                              TrimCrvStruct **TrimCrvs2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfRegionFromTrimSrf(TrimSrfStruct *TrimSrf,
                                        double t1,
                                        double t2,
                                        CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfRefineAtParams( TrimSrfStruct *Srf,
                                     CagdSrfDirType Dir,
                                     int Replace,
                                     double *t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfReverse( TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfReverse2( TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimRemoveCrvSegTrimCrvs(TrimCrvSegStruct *TrimCrvSeg,
                             TrimCrvStruct **TrimCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimRemoveCrvSegTrimCrvSegs(TrimCrvSegStruct *TrimCrvSeg,
                                TrimCrvSegStruct **TrimCrvSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfDomain( TrimSrfStruct *TrimSrf,
                   double *UMin,
                   double *UMax,
                   double *VMin,
                   double *VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimCrvSegBBox( TrimCrvSegStruct *TCrvSeg,
                   int UV,
                   GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimCrvSegListBBox( TrimCrvSegStruct *TCrvSegs,
                       int UV,
                       GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimCrvBBox( TrimCrvStruct *TCrv, int UV, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimCrvListBBox( TrimCrvStruct *TCrvs, int UV, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfBBox( TrimSrfStruct *TSrf, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimSrfListBBox( TrimSrfStruct *TSrfs, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfNumOfTrimLoops( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSrfNumOfTrimCrvSegs( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrimSrfEval( TrimSrfStruct *TrimSrf, double u, double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimCrvTrimParamList(CagdCrvStruct *Crv,
                                    TrimIsoInterStruct *InterList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimIsoInterStruct **TrimIntersectTrimCrvIsoVals( TrimSrfStruct *TrimSrf,
                                                 int Dir,
                                                 double *OrigIsoParams,
                                                 int NumOfIsocurves,
                                                 int Perturb);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimIsoInterStruct **TrimIntersectCrvsIsoVals( CagdCrvStruct *UVCrvs,
                                              int Dir,
                                              double *IsoParams,
                                              int NumOfIsocurves);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrimCrvAgainstTrimCrvs(CagdCrvStruct *UVCrv,
                                       TrimSrfStruct *TrimSrf,
                                      double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *TrimSrfAdap2Polygons( TrimSrfStruct *TrimSrf,
                                        double Tolerance,
                                        int ComputeNormals,
                                        int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *TrimCrvsHierarchy2Polys(TrimCrvStruct *TrimLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimMake2ndCrvSameLengthAs1stCrv( CagdCrvStruct *Crv1,
                                      CagdCrvStruct **Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvSegReverse(TrimCrvSegStruct *TSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimCrvSegListReverse(TrimCrvSegStruct *TSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvSegStruct *TrimOrderTrimCrvSegsInLoop(TrimCrvSegStruct *TSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimClassifyTrimmingLoops(TrimCrvStruct **TrimLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimClassifyTrimLoopOrient( TrimCrvSegStruct *TSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvFreeListWithSubTrims(TrimCrvStruct *TrimCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimCrvFreeWithSubTrims(TrimCrvStruct *TrimCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimClassifyTrimCurveOrient( CagdCrvStruct *UVCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *TrimSrf2Polygons2( TrimSrfStruct *Srf,
                                     int FineNess, 
                                     int ComputeNormals,
                                     int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimSetNumTrimVrtcsInCell(int NumTrimVrtcsInCell);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbCrvApproxMethodType TrimSetTrimCrvLinearApprox(double UVTolSamples,
                                           SymbCrvApproxMethodType UVMethod);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double TrimGetTrimCrvLinearApprox();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfsFromContours( CagdSrfStruct *Srf,
                                      IPPolygonStruct *Cntrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfsFromContours2( CagdSrfStruct *Srf,
                                      CagdCrvStruct *CCntrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *TrimValidateNewTrimCntrs( CagdSrfStruct *Srf,
                                                   IPPolygonStruct
                                                                      *Cntrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimLoopWeightRelationInside(double V1, double V2, double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double TrimLoopUV2Weight( double *UV,
                            double *BndryUV,
                            double UMin,
                            double UMax,
                            double VMin,
                            double VMax,
                            int Last);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrimLoopWeight2UV(double Wgt,
                             double UMin,
                             double UMax,
                             double VMin,
                             double VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimSrfsFromTrimPlsHierarchy( IPPolygonStruct *TopLevel,
                                             IPPolygonStruct *TrimPls,
                                             CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimCrvStruct *TrimPolylines2LinTrimCrvs(  IPPolygonStruct *Polys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimIsPointInsideTrimSrf( TrimSrfStruct *TrimSrf,
                                   IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimIsPointInsideTrimCrvs( TrimCrvStruct *TrimCrvs,
                                    IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimIsPointInsideTrimUVCrv( CagdCrvStruct *UVCrv,
                               IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSetErrorFuncType TrimSetFatalErrorFunc(TrimSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *TrimDescribeError(TrimFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimFatalError(TrimFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimDbg( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimDbgTCrvs( TrimCrvStruct *TrimCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimDbgTCrvSegs( TrimCrvSegStruct *TrimSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimAllPrisaSrfs( TrimSrfStruct *TSrfs,
                                int SamplesPerCurve,
                                double Epsilon,
                                CagdSrfDirType Dir,
                                IrtVecType* Space);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimPiecewiseRuledSrfApprox( TrimSrfStruct *TSrf,
                                           int ConsistentDir,
                                           double Epsilon,
                                           CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimPrisaRuledSrf( TrimSrfStruct *TSrf,
                                 int SamplesPerCurve,
                                 double Space,
                                 IrtVecType* Offset,
                                 CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimUntrimResultStruct *TrimUntrimTrimSrf(TrimSrfStruct *TSrf,
                                          CagdQuadSrfWeightFuncType WeightFunc,
                                          int Compose);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimUntrimSetLineSweepOutputCrvPairs(int NewValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimUntrimmingResultFree(TrimUntrimResultStruct *Untrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrimUntrimmingResultFreeList(TrimUntrimResultStruct *Untrim);
    }
}
