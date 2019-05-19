using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void MvarSetErrorFuncType (MvarFatalErrorType ErrorFunc);
    public unsafe delegate void MvarExprTreePrintFuncType ( byte *s);
    public unsafe delegate void MvarMVsZerosMVsCBFuncType (MvarMVStruct **MVs, int n);
    public unsafe delegate void MvarMVsZerosSubdivCBFuncType (MvarZeroPrblmStruct *p, int i);
    public unsafe delegate void MvarMVsZerosVerifyOneSolPtCBFuncType (MvarPtStruct *Pt);
    public unsafe delegate void MvarMVsZerosVerifyAllSolsCBFuncType (MvarPolylineStruct **MVPls);
    public unsafe delegate void MvarMapPrm2EucCBFuncType (double *R, int n);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVNew(int Dim,
                        MvarGeomType GType,
                        MvarPointType PType,
                         int *Lengths);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVNew(int Dim,
                            int *Lengths,
                            int *Orders,
                           MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVNew(int Dim,  int *Lengths, MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarPwrMVNew(int Dim,  int *Lengths, MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBuildParamMV(int Dim, int Dir, double Min, double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCopy( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCopyList( MvarMVStruct *MVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVFree(MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVFreeList(MvarMVStruct *MVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtNew(int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtRealloc(MvarPtStruct *Pt, int NewDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtCopy( MvarPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtCopyList( MvarPtStruct *PtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtSortListAxis(MvarPtStruct *PtList, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPtFree(MvarPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPtFreeList(MvarPtStruct *PtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPolyReverseList(MvarPtStruct *Pts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarPolylineNew(MvarPtStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarPolylineCopy( MvarPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarPolylineCopyList(MvarPolylineStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPolylineFree(MvarPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPolylineFreeList(MvarPolylineStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecNew(int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecArrayNew(int Size, int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecRealloc(MvarVecStruct *Vec, int NewDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecCopy( MvarVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecCopyList( MvarVecStruct *VecList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecFree(MvarVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecFreeList(MvarVecStruct *VecList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecArrayFree(MvarVecStruct *MVVecArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecAdd(MvarVecStruct *VRes,
                 MvarVecStruct *V1,
                 MvarVecStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecAddScale(MvarVecStruct *VRes,
                      MvarVecStruct *V1,
                      MvarVecStruct *V2,
                     double Scale2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecSub(MvarVecStruct *VRes,
                 MvarVecStruct *V1,
                 MvarVecStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarVecDotProd( MvarVecStruct *V1,  MvarVecStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarVecSqrLength( MvarVecStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarVecSqrLength2( double *v, int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarVecLength( MvarVecStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarVecScale(MvarVecStruct *V, double ScaleFactor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarVecBlend(MvarVecStruct *VRes,
                   MvarVecStruct *V1,
                   MvarVecStruct *V2,
                  double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecNormalize(MvarVecStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecOrthogonalize(MvarVecStruct *Dir,  MvarVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecOrthogonal2(MvarVecStruct *Dir,
                        MvarVecStruct *Vec1,
                        MvarVecStruct *Vec2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecSetOrthogonalize( MvarVecStruct **Vecs,
                            MvarVecStruct **OrthoVecs,
                            int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecWedgeProd(MvarVecStruct **Vectors,
                           int Size, 
                           MvarVecStruct **NewVecs,
                           int NewSize, 
                           int CheckDet,
                           double *DetVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarPlaneNormalize(MvarPlaneStruct *Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarLinePlaneInter( MvarVecStruct *P,
                                   MvarVecStruct *V,
                                   MvarPlaneStruct *Pln,
                                  double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPlaneStruct *MvarPlaneNew(int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPlaneStruct *MvarPlaneCopy( MvarPlaneStruct *Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPlaneStruct *MvarPlaneCopyList( MvarPlaneStruct *PlaneList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPlaneFree(MvarPlaneStruct *Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPlaneFreeList(MvarPlaneStruct *PlaneList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MvarNormalConeNew(int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MvarNormalConeCopy( MvarNormalConeStruct
                                                                 *NormalCone);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MvarNormalConeCopyList( MvarNormalConeStruct
                                                                *NormalCones);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarNormalConeFree(MvarNormalConeStruct *NormalCone);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarNormalConeFreeList(MvarNormalConeStruct *NormalConeList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarGetLastPt(MvarPtStruct *Pts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarPtCmpTwoPoints( MvarPtStruct *P1,
                        MvarPtStruct *P2,
                       double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarVecCmpTwoVectors( double *P1,
                          double *P2,
                         int Length,
                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarPtDistTwoPoints( MvarPtStruct *P1,  MvarPtStruct *P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarPtDistSqrTwoPoints( MvarPtStruct *P1,
                                  MvarPtStruct *P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarPtInBetweenPoint( MvarPtStruct *Pt1,
                                    MvarPtStruct *Pt2,
                                   double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarPolyMergePolylines(MvarPolylineStruct *Polys,
                                           double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMatchPointListIntoPolylines( MvarPtStruct
                                                                     *PtsList,
                                                    double MaxTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCnvrtCagdPtsToMVPts( CagdPtStruct *Pts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCnvrtMVPolysToMVPts( MvarPolylineStruct *MVPlls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVPolysToCtlPts( MvarPolylineStruct
                                                                     *MVPlls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *MvarCnvrtMVPtsToPts( MvarPtStruct *MVPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVPtsToCtlPts( MvarPtStruct *MVPts,
                                              double MergeTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVPtsToPolys( MvarPtStruct *MVPts,
                                              MvarMVStruct *MV,
                                             double MergeTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *MvarCnvrtMVPtsToPolys2( MvarPtStruct *InPts,
                                               double FineNess,
                                               int Dim,
                                               double *ParamDomain);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVPolysToIritPolys( MvarPolylineStruct
                                                                      *MVPlls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVPolysToIritPolys2( MvarPolylineStruct
                                                                       *MVPlls,
                                                    int IgnoreIndividualPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarCnvrtMVPolysToIritCrvs( MvarPolylineStruct *MVPlls,
                                          int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarCnvrtIritLinCrvsToMVPolys( CagdCrvStruct *Crvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCnvrtMVTrsToIritPolygons( MvarTriangleStruct
                                                                        *MVTrs,
                                                    int *Coords);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarTriangleStruct *MvarIrit2DTrTo2DMVTrs( IPObjectStruct *ObjTrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtBzr2BspMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtBsp2BzrMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtPwr2BzrMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtBzr2PwrMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrLinearInOneDir(int Dim, int Dir, MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVUnitMaxCoef(MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVTransform(MvarMVStruct *MV, double *Translate, double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVMatTransform(MvarMVStruct *MV, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCoerceMVsTo( MvarMVStruct *MV, MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCoerceMVTo( MvarMVStruct *MV, MvarPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPointType MvarMergeTwoPointTypes(MvarPointType PType1, MvarPointType PType2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVDomain( MvarMVStruct *MV,
                  double *Min,
                  double *Max, 
                  int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVDomainAlloc( MvarMVStruct *MV,
                       double **MinDmn, 
                       double **MaxDmn);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVDomainFree(double *MinDmn, double *MaxDmn);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVSetDomain(MvarMVStruct *MV,
                              double Min,
                              double Max,
                              int Axis,
                              int InPlace);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarMVVolumeOfDomain(MvarMVStruct *  MVs, int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVAuxDomainSlotReset(MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVAuxDomainSlotCopy(MvarMVStruct *MVDst,  MvarMVStruct *MVSrc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVAuxDomainSlotSet(MvarMVStruct *MV,
                            double Min,
                            double Max,
                            int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVAuxDomainSlotSetRel(MvarMVStruct *MV,
                               double Min,
                               double Max,
                               int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVAuxDomainSlotGet( MvarMVStruct *MV,
                           double *Min,
                           double *Max,
                           int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVSetAllDomains(MvarMVStruct *MV,
                                  double *Min,
                                  double *Max,
                                  int InPlace);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarParamInDomain( MvarMVStruct *MV,
                            double t,
                            int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarParamsInDomain( MvarMVStruct *MV,  double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVUpdateConstDegDomains(MvarMVStruct **MVs, int NumOfMVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVIntersPtOnBndry(MvarMVStruct *MV, 
                                    MvarPtStruct *PointIns, 
                                    MvarPtStruct *PointOuts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarMVEval( MvarMVStruct *MV,  double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarMVEval2( MvarMVStruct *MV,  double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarMVEvalGradient2( MvarMVStruct *MV,
                                double *Params,
                               int *HasOrig);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPlaneStruct *MvarMVEvalTanPlane( MvarMVStruct *MV,
                                     double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVFromMV( MvarMVStruct *MV,
                           double t,
                           int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVFromMesh( MvarMVStruct *MV,
                             int Index,
                             int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCrvToMV( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarMVToCrv( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarSrfToMV( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *MvarMVToSrf( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarTVToMV( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarMVToTV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVRegionFromMV( MvarMVStruct *MV,
                                 double t1,
                                 double t2,
                                 int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVRegionFromMV( MvarMVStruct *MV,
                                    double t1,
                                    double t2,
                                    int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVOpenEnd( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVRefineAtParams( MvarMVStruct *MV,
                                   int Dir,
                                   int Replace,
                                   double *t,
                                   int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVKnotInsertNDiff( MvarMVStruct *MV,
                                       int Dir,
                                       int Replace,
                                       double *t,
                                       int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDerive( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVDerive( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVDeriveScalar( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVDerive( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVDeriveScalar( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVDeriveAllBounds( MvarMVStruct *MV, IrtMinMaxType* MinMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBzrMVDeriveAllBounds( MvarMVStruct *MV, IrtMinMaxType* MinMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBspMVDeriveAllBounds( MvarMVStruct *MV, IrtMinMaxType* MinMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVGradientStruct *MvarMVPrepGradient( MvarMVStruct *MV,
                                         int Orig);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVFreeGradient(MvarMVGradientStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarMVEvalGradient( MvarMVGradientStruct *MV,
                               double *Params,
                              int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVGradientStruct *MvarMVBoundGradient( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVSubdivAtParam( MvarMVStruct *MV,
                                  double t,
                                  int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVSubdivAtParam( MvarMVStruct *MV,
                                     double t,
                                     int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVSubdivAtParam( MvarMVStruct *MV,
                                     double t,
                                     int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVSubdivAtParamOneSide( MvarMVStruct *MV,
                                         double t,
                                         int Dir,
                                         byte LeftSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVSubdivAtParamOneSide( MvarMVStruct *MV,
                                            double t,
                                            int Dir,
                                            byte LeftSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVSubdivAtParamOneSide( MvarMVStruct *MV,
                                            double t,
                                            int Dir,
                                            byte LeftSide);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDegreeRaise( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDegreeRaiseN( MvarMVStruct *MV, int *NewOrders);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDegreeRaiseN2( MvarMVStruct *MV, int *NewOrders);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVPwrDegreeRaise( MvarMVStruct *MV,
                                   int Dir,
                                   int IncOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMakeMVsCompatible(MvarMVStruct **MV1,
                                MvarMVStruct **MV2,
                                int SameOrders,
                                int SameKVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMakeMVsCompatible2(MvarMVStruct **MV1,
                                 MvarMVStruct **MV2,
                                 int SameOrders,
                                 int SameKVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMakeMVsOneDimCompatible(MvarMVStruct **MV1,
                                      MvarMVStruct **MV2,
                                      int Dim,
                                      int SameOrders,
                                      int SameKVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVMinMax( MvarMVStruct *MV,
                  int Axis,
                  double *Min,
                  double *Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVBBox( MvarMVStruct *MV, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVListBBox( MvarMVStruct *MVs, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMergeBBox(GMBBBboxStruct *DestBBox,  GMBBBboxStruct *SrcBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVIsConstant( MvarMVStruct *MV, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBBoxOfDotProd( GMBBBboxStruct *BBox1,
                        GMBBBboxStruct *BBox2,
                       GMBBBboxStruct *DProdBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBBoxOfDotProd2( GMBBBboxStruct *BBox1,
                         GMBBBboxStruct *BBox2,
                        GMBBBboxStruct *DProdBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBBoxOfCrossProd( GMBBBboxStruct *BBox1,
                          GMBBBboxStruct *BBox2,
                         GMBBBboxStruct *DCrossBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct **MvarBndryMVsFromMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVPreciseBBox( MvarMVStruct *MV,
                                GMBBBboxStruct *BBox,
                                double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVListPreciseBBox( MvarMVStruct *MVs,
                           GMBBBboxStruct *BBox,
                           double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarTrivarPreciseBBox( TrivTVStruct *TV,
                           GMBBBboxStruct *BBox,
                           double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarTrivarListPreciseBBox( TrivTVStruct *Trivars,
                               GMBBBboxStruct *BBox,
                               double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSrfPreciseBBox( CagdSrfStruct *Srf,
                        GMBBBboxStruct *BBox,
                        double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSrfListPreciseBBox( CagdSrfStruct *Srfs,
                            GMBBBboxStruct *BBox,
                            double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMdlTrimSrfPreciseBBox( MdlTrimSrfStruct *TSrf,
                               GMBBBboxStruct *BBox,
                               double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMdlTrimSrfListPreciseBBox( MdlTrimSrfStruct *TSrfs,
                                   GMBBBboxStruct *BBox,
                                   double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarTrimSrfPreciseBBox( TrimSrfStruct *TSrf,
                            GMBBBboxStruct *BBox,
                            double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarTrimSrfListPreciseBBox( TrimSrfStruct *TSrfs,
                                GMBBBboxStruct *BBox,
                                double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarCrvPreciseBBox( CagdCrvStruct *Crv,
                        GMBBBboxStruct *BBox,
                        double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarCrvListPreciseBBox( CagdCrvStruct *Crvs,
                            GMBBBboxStruct *BBox,
                            double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _MvarIncrementMeshIndices( MvarMVStruct *MV, int *Indices, int *Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _MvarIncrementMeshOrderIndices( MvarMVStruct *MV,
                                   int *Indices,
                                   int *Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _MvarIncSkipMeshIndices1st( MvarMVStruct *MV, int *Indices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _MvarIncSkipMeshIndices( MvarMVStruct *MV,
                            int *Indices,
                            int Dir,
                            int *Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _MvarIncBoundMeshIndices( MvarMVStruct *MV,
                             int *Indices,
                             int *LowerBound,
                             int *UpperBound,
                             int *Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarGetPointsMeshIndices( MvarMVStruct *MV, int *Indices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarGetPointsPeriodicMeshIndices( MvarMVStruct *MV, int *Indices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMeshIndicesFromIndex(int Index,  MvarMVStruct *MV, int *Indices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarEditSingleMVPt(MvarMVStruct *MV,
                                 CagdCtlPtStruct *CtlPt,
                                 int *Indices,
                                 int Write);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsSameSpace( MvarMVStruct *MV1,
                            MvarMVStruct *MV2,
                           double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsSame( MvarMVStruct *MV1,
                       MvarMVStruct *MV2,
                      double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarPromoteMVToMV( MvarMVStruct *MV, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarPromoteMVToMV2( MvarMVStruct *MV,
                                 int NewDim, 
                                 int StartAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCrvMakeCtlPtParam( CagdCrvStruct *Crv,
                                    int CtlPtIdx,
                                    double Min,
                                    double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVShiftAxes( MvarMVStruct *MV, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVParamShift( MvarMVStruct *MV, int AxisSrc, int AxisTar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVReverse( MvarMVStruct *MV, int Axis1, int Axis2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVReverseDir( MvarMVStruct *MV, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMergeMVMV( MvarMVStruct *MV1,
                             MvarMVStruct *MV2,
                            int Dir,
                            int Discont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVHasOpenECInDir( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVHasOpenEC( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVIsPeriodicInDir( MvarMVStruct *MV, int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVIsPeriodic( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVInteriorKnots( MvarMVStruct *MV, double *Knot);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVMultiLinearMV( double *Min,
                                   double *Max, 
                                  int Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarDbg( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarDbgDsp( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarDbgInfo( void **Objs, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarETDbg( MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVExtension( MvarMVStruct *OrigMV,
                               int *ExtMins,
                               int *ExtMaxs,
                               double *Epsilons);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVKnotHasC0Discont( MvarMVStruct *MV,
                                 int *Dir,
                                 double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVMeshC0Continuous( MvarMVStruct *MV,
                                 int Dir,
                                 int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVIsMeshC0DiscontAt( MvarMVStruct *MV,
                                  int Dir,
                                  double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVKnotHasC1Discont( MvarMVStruct *MV,
                                 int *Dir,
                                 double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVMeshC1Continuous( MvarMVStruct *MV,
                                 int Dir,
                                 int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVIsMeshC1DiscontAt( MvarMVStruct *MV,
                                  int Dir,
                                  double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarBspCrvInterpVecs( MvarVecStruct *PtList,
                                    int Order,
                                    int CrvSize,
                                    CagdParametrizationType ParamType,
                                    int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarVecStruct *MvarPtsSortAxis(MvarVecStruct *PtList, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarPointFromPointLine( MvarVecStruct *Point,
                             MvarVecStruct *Pl,
                             MvarVecStruct *Vl,
                            MvarVecStruct *ClosestPoint);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarDistPointLine( MvarVecStruct *Point,
                            MvarVecStruct *Pl,
                            MvarVecStruct *Vl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarLineFitToPts( MvarVecStruct *PtList,
                           MvarVecStruct *LineDir,
                           MvarVecStruct *LinePos);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVAdd( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVSub( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVMult( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVInvert( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVScalarScale( MvarMVStruct *MV, double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVMultScalar( MvarMVStruct *MV1,
                                MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDotProd( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVVecDotProd( MvarMVStruct *MV,  double *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCrossProd( MvarMVStruct *MV1,
                               MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCrossProdZ( MvarMVStruct *MV1,
                                MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCrossProd2D( MvarMVStruct *MV1X,
                                 MvarMVStruct *MV1Y,
                                 MvarMVStruct *MV2X,
                                 MvarMVStruct *MV2Y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVRtnlMult( MvarMVStruct *MV1X,
                              MvarMVStruct *MV1W,
                              MvarMVStruct *MV2X,
                              MvarMVStruct *MV2W,
                             int OperationAdd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVMultBlend( MvarMVStruct *MV1,
                               MvarMVStruct *MV2,
                              double Blend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct **MvarMVSplitScalar( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVMergeScalar(MvarMVStruct *  *ScalarMVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMultComputationMethod(int BspMultUsingInter);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVMult( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVMult( MvarMVStruct *MV1,  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVDeriveRational( MvarMVStruct *MV,
                                      int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVDeriveRational( MvarMVStruct *MV,
                                      int Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDeterminant2( MvarMVStruct *MV11,
                                  MvarMVStruct *MV12,
                                  MvarMVStruct *MV21,
                                  MvarMVStruct *MV22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDeterminant3( MvarMVStruct *MV11,
                                  MvarMVStruct *MV12,
                                  MvarMVStruct *MV13,
                                  MvarMVStruct *MV21,
                                  MvarMVStruct *MV22,
                                  MvarMVStruct *MV23,
                                  MvarMVStruct *MV31,
                                  MvarMVStruct *MV32,
                                  MvarMVStruct *MV33);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDeterminant4( MvarMVStruct *MV11,
                                  MvarMVStruct *MV12,
                                  MvarMVStruct *MV13,
                                  MvarMVStruct *MV14,
                                  MvarMVStruct *MV21,
                                  MvarMVStruct *MV22,
                                  MvarMVStruct *MV23,
                                  MvarMVStruct *MV24,
                                  MvarMVStruct *MV31,
                                  MvarMVStruct *MV32,
                                  MvarMVStruct *MV33,
                                  MvarMVStruct *MV34,
                                  MvarMVStruct *MV41,
                                  MvarMVStruct *MV42,
                                  MvarMVStruct *MV43,
                                  MvarMVStruct *MV44);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDeterminant5( MvarMVStruct *MV11,
                                  MvarMVStruct *MV12,
                                  MvarMVStruct *MV13,
                                  MvarMVStruct *MV14,
                                  MvarMVStruct *MV15,
                                  MvarMVStruct *MV21,
                                  MvarMVStruct *MV22,
                                  MvarMVStruct *MV23,
                                  MvarMVStruct *MV24,
                                  MvarMVStruct *MV25,
                                  MvarMVStruct *MV31,
                                  MvarMVStruct *MV32,
                                  MvarMVStruct *MV33,
                                  MvarMVStruct *MV34,
                                  MvarMVStruct *MV35,
                                  MvarMVStruct *MV41,
                                  MvarMVStruct *MV42,
                                  MvarMVStruct *MV43,
                                  MvarMVStruct *MV44,
                                  MvarMVStruct *MV45,
                                  MvarMVStruct *MV51,
                                  MvarMVStruct *MV52,
                                  MvarMVStruct *MV53,
                                  MvarMVStruct *MV54,
                                  MvarMVStruct *MV55);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDeterminant( MvarMVStruct *  *MVsMatrix,
                                int MatrixSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarZeroSolver(MvarZeroPrblmStruct *Problem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarZeroSolveMatlabEqns(
                                              MvarMatlabEqStruct **Eqns,
                                              int NumOfEqns,
                                              int MaxVarsNum,
                                              double *MinDmn,
                                              double *MaxDmn,
                                              double NumericTol,
                                              double SubdivTol,
                                              double StepTol,
                                              MvarConstraintType *Constraints);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVsZeros0D(MvarMVStruct *  *MVs,
                             MvarConstraintType *Constraints,
                             int NumOfMVs,
                             double SubdivTol,
                             double NumericTol,
                             int HighDimBndry);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarZero0DNumeric(MvarPtStruct *ZeroPt,
                                 MvarExprTreeEqnsStruct *Eqns,
                                MvarMVStruct  *  *MVs,
                                int NumMVs,
                                double NumericTol,
                                 double *InputMinDmn,
                                 double *InputMaxDmn);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVsZeros2DBy0D(MvarMVStruct *  *MVs,
                                 MvarConstraintType *Constraints,
                                 int NumOfMVs,
                                 double SubdivTol,
                                 double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarETsZeros0D(MvarExprTreeStruct *  *MVETs,
                             MvarConstraintType *Constraints,
                             int NumOfMVETs,
                             double SubdivTol,
                             double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *MvarCrvZeroSet( CagdCrvStruct *Curve,
                             int Axis,
                             double SubdivTol,
                             double NumericTol,
                             int FilterTangencies);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosSameSpace(MvarMVStruct **MVs, int NumOfMVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosNormalConeTest(int NormalConeTest);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarMVsZerosDmnExt(double DmnExt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSubdivTolActionType MvarZerosSubdivTolAction(
                                 MvarZeroSubdivTolActionType SubdivTolAction);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosGradPreconditioning(int GradPreconditioning);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZeros2DPolylines(int IsPolyLines2DSolution);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZeros2DCornersOnly(int Is2DSolutionCornersOnly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosCrtPts(int CrtPtsDetectionByDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosNormalizeConstraints(int NormalizeConstraints);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosDomainReduction(int DomainReduction);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosParallelHyperPlaneTest(int ParallelHPlaneTest);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZerosKantorovichTest(int KantorovichTest);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVsZerosSubdivCBFuncType MvarMVsZerosSubdivSetCallBackFunc(
                             MvarMVsZerosSubdivCBFuncType SubdivCallBackFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVsZerosVerifyOneSolPtCBFuncType MvarMVsZerosVerifyOneSolPtCallBackFunc(
                   MvarMVsZerosVerifyOneSolPtCBFuncType VerifyOneSolPtCBFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVsZerosVerifyAllSolsCBFuncType MvarMVsZerosVerifyAllSolsCallBackFunc(
                     MvarMVsZerosVerifyAllSolsCBFuncType VerifyAllSolsCBFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVsZerosMVsCBFuncType MvarMVsZerosETs2MVsCBFunc(
                                     MvarMVsZerosMVsCBFuncType ETs2MVsCBFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarZeroSolverSolutionNew(
                                            MvarTriangleStruct *Tr,
                                            MvarPolylineStruct *Pl,
                                            MvarPtStruct *Pt,
                                            MvarZrSlvrRepresentationType Rep);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarZeroSolverSolCpy(MvarZeroSolutionStruct  *Sol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarZeroSolutionCpyList(MvarZeroSolutionStruct 
                                                 *SolutionList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVDDecompositionModeType MvarMVDSetDecompositionMode(
                                              MvarMVDDecompositionModeType m);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarZeroSolverSolutionFree(MvarZeroSolutionStruct *Solution,
                                int FreeUnion);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVsZerosVerifier(MvarMVStruct *  *MVs,
                          int NumOfZeroMVs,
                          MvarPtStruct *Sols,
                          double NumerEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfSrfInter( CagdSrfStruct *Srf1, 
                                     CagdSrfStruct *Srf2,
                                    double Step,
                                    double SubdivTol,
                                    double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfSrfInterC1Disc( CagdSrfStruct *Srf1, 
                                           CagdSrfStruct *Srf2,
                                          double Step,
                                          double SubdivTol,
                                          double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVsZeros1D(MvarMVStruct *  *MVs,
                                   MvarConstraintType *Constraints,
                                   int NumOfMVs,
                                   double Step,
                                   double SubdivTol,
                                   double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVsZeros1DOneTrace(MvarMVStruct *  *MVs,
                                           MvarConstraintType *Constraints,
                                           int NumOfMVs,
                                           MvarPtStruct *StartEndPts,
                                           double Step,
                                           double SubdivTol,
                                           double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVsZeros1DMergeSingularPts(int MergeSingularPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfZeroSet( CagdSrfStruct *Surface,
                                   int Axis,
                                   double Step,
                                   double SubdivTol,
                                   double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarSrfSrfInterCacheStruct *MvarSrfSrfInterCacheAlloc(
                               MvarSrfSrfInterCacheAttribName AttribName,
                              int ShouldAssignIds);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarSrfSrfInterCacheGetSrfId( MvarSrfSrfInterCacheStruct *SSICache, 
                                  CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarSrfSrfInterCacheDataStruct *MvarSrfSrfInterCacheGetData(
                                    MvarSrfSrfInterCacheStruct *SSICache,
                                    CagdSrfStruct *Srf1, 
                                    CagdSrfStruct             *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarSrfSrfCacheShouldAssignIds( MvarSrfSrfInterCacheStruct 
                                                                *DataCache);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarSrfSrfInterCacheDataStruct *MvarSrfSrfInterCacheAddData(
                                       MvarSrfSrfInterCacheStruct *SSICache, 
                                       CagdSrfStruct *Srf1, 
                                       CagdSrfStruct *Srf2, 
                                       MvarPolylineStruct *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSrfSrfInterCacheClear(MvarSrfSrfInterCacheStruct *SSICahce);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSrfSrfInterCacheFree(MvarSrfSrfInterCacheStruct *SSICahce);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanToCrvEndPtCntrOnCrv(
                                                CagdCrvStruct *Crv1,
                                                IrtPtType* Pt2,
                                                CagdCrvStruct *CntrOnCrv,
                                               double RadiusLB,
                                               double SubdivTol,
                                               double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanTo2CrvsCntrOnCrv( CagdCrvStruct *Crv,
                                               CagdCrvStruct *CntrOnCrv,
                                              double RadiusLB,
                                              double SubdivTol,
                                              double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanTo3CrvsExprTreeNoDiagonal(
                                                      CagdCrvStruct *Crv,
                                                     double RadiusLB,
                                                     double SubdivTol,
                                                     double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanTo2CrvsEndPtNoDiag( CagdCrvStruct *Crv1,
                                                 CagdCrvStruct *Crv2,
                                                 IrtPtType* Pt3,
                                                int ElimDiagonals,
                                                double RadiusLB,
                                                double SubdivTol,
                                                double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanToCrvEndPt( CagdCrvStruct *Crv1,
                                         IrtPtType* Pt2,
                                        double RadiusLB,
                                        double SubdivTol,
                                        double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrCircTanToCrv2EndPt( CagdCrvStruct *Crv1,
                                          IrtPtType* Pt2,
                                          IrtPtType* Pt3,
                                         double RadiusLB,
                                         double SubdivTol,
                                         double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSCvrBiNormals( CagdCrvStruct *Crv1,
                                 CagdCrvStruct *Crv2,
                                int ElimDiagonals,
                                double RadiusLB,
                                double SubdivTol,
                                double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanToCircCrv3By3( CagdCrvStruct *Bndry,
                                        CagdCrvStruct *InNrml,
                                       IrtPtType* XBnd,
                                       IrtPtType* YBnd,
                                        IrtPtType* Center,
                                       double Radius,
                                       int BndBndryPar,
                                       double BndryPar,
                                       double NumericTol,
                                       double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircOnLineTangToBdry( CagdCrvStruct *Bndry,
                                        CagdCrvStruct *InNrml,
                                       double Radius,
                                        IrtPtType* Dir,
                                        IrtPtType* Pt,
                                       double NumericTol,
                                       double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanAtTwoPts( CagdCrvStruct *Bndry,
                                  IrtPtType* XBnd,
                                  IrtPtType* YBnd,
                                  double Radius,
                                  double NumericTol,
                                  double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircAtDirMax( CagdCrvStruct *Bndry,
                               IrtPtType* XBnd,
                               IrtPtType* YBnd,
                               double Radius,
                                IrtPtType* Dir,
                               double NumericTol,
                               double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanToCrvXCoord( CagdCrvStruct *Bndry,
                                      CagdCrvStruct *InNrml,
                                     IrtPtType* YBnd,
                                     double Radius,
                                     double XCoord,
                                     double NumericTol,
                                     double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanToCrvYCoord( CagdCrvStruct *Bndry,
                                      CagdCrvStruct *InNrml,
                                     IrtPtType* XBnd,
                                     double Radius,
                                     double YCoord,
                                     double NumericTol,
                                     double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarMVsZeros2D(
                                MvarMVStruct *  *MVs,
                                MvarConstraintType *Constraints,
                                int NumOfMVs,
                                double Step,
                                double SubdivTol,
                                double NumericTol, 
                                MvarMapPrm2EucCBFuncType MapPt2EuclidSp, 
                                MvarMapPrm2EucCBFuncType MapPt2EuclidNrml);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarZeroSolverPolyProject(MvarPolylineStruct *PolyList,
                                              int *Coords,
                                              int ProjDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMinSpanConeAvg(MvarVecStruct *MVVecs,
                       int VecsNormalized,
                       int NumOfVecs,
                       MvarNormalConeStruct *MVCone);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMinSpanCone(MvarVecStruct *MVVecs,
                    int VecsNormalized,
                    int NumOfVecs,
                    MvarNormalConeStruct *MVCone);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MVHyperPlaneFromNPoints(MvarPlaneStruct *MVPlane,
                            MvarVecStruct *  *Vecs,
                            int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MVHyperConeFromNPoints(MvarNormalConeStruct *MVCone,
                           MvarVecStruct *  *Vecs,
                           int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MVHyperConeFromNPoints2(MvarNormalConeStruct *MVCone,
                            MvarVecStruct *  *Vecs,
                            int m);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MVHyperConeFromNPoints3(MvarNormalConeStruct *MVCone,
                            MvarVecStruct *  *Vecs,
                            int m);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MVarMVNormalCone( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MVarMVNormalCone2( MvarMVStruct *MV,
                                        double *  *GradPoints,
                                        int TotalLength,
                                        int *MaxDevIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MVarMVNormalConeMainAxis( MvarMVStruct *MV,
                                               MvarVecStruct **MainAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MVarMVNormalConeMainAxis2( MvarMVStruct *MV,
                                                double *  *GradPoints,
                                                int TotalLength,
                                                MvarVecStruct **MainAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MvarMVNormal2Cones( MvarMVStruct *MV,
                                         double ExpandingFactor,
                                         int NumOfZeroMVs,
                                         MvarNormalConeStruct **Cone1,
                                         MvarNormalConeStruct **Cone2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVConesOverlap(MvarMVStruct **MVs, int NumOfZeroMVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfSrfMinkowskiSum( CagdSrfStruct *Srf1, 
                                               CagdSrfStruct *Srf2,
                                              double SubdivTol,
                                              double CrvTraceStep,
                                              double NumericTol, 
                                              int ParallelNrmls, 
                                              double OffsetTrimDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarCrvSrfMinkowskiSum( CagdCrvStruct *Crv, 
                                               CagdSrfStruct *Srf,
                                              double SubdivTol,
                                              double CrvTraceStep,
                                              double NumericTol, 
                                              double OffsetTrimDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSurfaceRayIntersection( CagdSrfStruct *Srf,
                                          IrtPtType* RayOrigin,
                                          IrtVecType* RayDir,
                                         double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarSrfRayIntersect( CagdSrfStruct *Srf, 
                         IrtVecType* RayPt,
                         IrtVecType* RayDir, 
                        CagdUVStruct **InterPntsUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarTrimSrfRayIntersect(  TrimSrfStruct *TrimSrf, 
                            IrtVecType* RayPt,
                            IrtVecType* RayDir, 
                            CagdUVStruct **InterPntsUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *Mvar2CntctCompute2CntctMotion( CagdCrvStruct *CCrvA,
                                                   CagdCrvStruct *CCrvB,
                                                  double Step,
                                                  double SubdivTol,
                                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeFromCrv( CagdCrvStruct *Crv,
                                        int NewDim,
                                        int StartAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeFromSrf( CagdSrfStruct *Srf,
                                        int NewDim,
                                        int StartAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeFromMV( MvarMVStruct *MV,
                                       int NewDim,
                                       int StartAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeFromMV2( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarExprTreeToMV( MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeLeafNew(int IsRef,
                                        MvarMVStruct *MV,
                                        int NewDim,
                                        int StartAxis,
                                        MvarNormalConeStruct *MVBCone,
                                         GMBBBboxStruct *MVBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeIntrnlNew(MvarExprTreeNodeType NodeType,
                                          MvarExprTreeStruct *Left,
                                          MvarExprTreeStruct *Right,
                                           GMBBBboxStruct *MVBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeCopy( MvarExprTreeStruct *ET,
                                     int ThisNodeOnly,
                                     int DuplicateMVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarExprTreeFreeSlots(MvarExprTreeStruct *ET, int ThisNodeOnly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarExprTreeFree(MvarExprTreeStruct *ET, int ThisNodeOnly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeSize(MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreesSame( MvarExprTreeStruct *ET1,
                             MvarExprTreeStruct *ET2,
                            double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarExprTreePrintInfo( MvarExprTreeStruct *ET,
                           int CommonExprIdx,
                           int PrintMVInfo,
                           MvarExprTreePrintFuncType PrintFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeAdd(MvarExprTreeStruct *Left,
                                    MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeSub(MvarExprTreeStruct *Left,
                                    MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeMult(MvarExprTreeStruct *Left,
                                     MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeMultScalar(MvarExprTreeStruct *Left,
                                           MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeMergeScalar(MvarExprTreeStruct *Left,
                                            MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeDotProd(MvarExprTreeStruct *Left,
                                        MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeCrossProd(MvarExprTreeStruct *Left,
                                          MvarExprTreeStruct *Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeExp(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeLog(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeCos(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeSqrt(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeSqr(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeNPow(MvarExprTreeStruct *Left, int Power);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarExprTreeStruct *MvarExprTreeRecip(MvarExprTreeStruct *Left);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeSubdivAtParam( MvarExprTreeStruct *ET,
                              double t,
                              int Dir,
                              MvarExprTreeStruct **Left,
                              MvarExprTreeStruct **Right);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  GMBBBboxStruct *MvarExprTreeBBox(MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *MvarExprTreeCompositionDerivBBox(MvarExprTreeStruct *ET,
                                                 GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarETDomain( MvarExprTreeStruct *ET,
                 double *Min,
                 double *Max,
                 int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeLeafDomain(MvarExprTreeStruct *ET,
                           double *Min,
                           double *Max,
                           int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarETUpdateConstDegDomains(MvarExprTreeStruct **MVETs, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreesVerifyDomain(MvarExprTreeStruct *ET1,
                              MvarExprTreeStruct *ET2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreesMakeConstDomainsUpdate(MvarExprTreeStruct **MVETs, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarExprAuxDomainReset(MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeCnvrtBsp2BzrMV(MvarExprTreeStruct *ET,
                               MvarMinMaxType *Domain);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeCnvrtBzr2BspMV(MvarExprTreeStruct *ET);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeInteriorKnots( MvarExprTreeStruct *ET, double *Knot);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarExprTreeEval( MvarExprTreeStruct *ET,
                            double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarExprTreeGradient( MvarExprTreeStruct *ET,
                                double *Params,
                                int *Dim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPlaneStruct *MvarExprTreeEvalTanPlane( MvarExprTreeStruct *ET,
                                          double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeZerosUseCommonExpr(int UseCommonExpr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeZerosCnvrtBezier2MVs(int Bezier2MVs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVsZerosMVsCBFuncType MvarExprTreeZerosCnvrtBezier2MVsCBFunc(
                                         MvarMVsZerosMVsCBFuncType MVsCBFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarExprTreesZeros(MvarExprTreeStruct *  *MVETs,
                                 MvarConstraintType *Constraints,
                                 int NumOfMVETs,
                                 double SubdivTol,
                                 double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarExprTreeEqnsZeros(MvarExprTreeEqnsStruct *Eqns,
                                    double SubdivTol,
                                    double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarNormalConeStruct *MVarExprTreeNormalCone(MvarExprTreeStruct *Eqn);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarExprTreeConesOverlap(MvarExprTreeEqnsStruct *Eqns);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvCrvInter( CagdCrvStruct *Crv1,
                               CagdCrvStruct *Crv2,
                              double SubdivTol,
                              double NumericTol,
                              int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvCrvContact( CagdCrvStruct *Crv1,
                                 CagdCrvStruct *Crv2,
                                 CagdCrvStruct *MotionCrv1,
                                 CagdCrvStruct *ScaleCrv1,
                                double SubdivTol,
                                double NumericTol,
                                int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfSrfSrfInter( CagdSrfStruct *Srf1,
                                  CagdSrfStruct *Srf2,
                                  CagdSrfStruct *Srf3,
                                 double SubdivTol,
                                 double NumericTol,
                                 int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfSrfContact( CagdSrfStruct *Srf1,
                                 CagdSrfStruct *Srf2,
                                 CagdCrvStruct *Srf1Motion,
                                 CagdCrvStruct *Srf1Scale,
                                double SubdivTol,
                                double NumericTol,
                                int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVFactorUMinusV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVFactorRMinusT( MvarMVStruct *MV, int RIdx, int TIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvAntipodalPoints( CagdCrvStruct *Crv,
                                     double SubdivTol,
                                     double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfAntipodalPoints( CagdSrfStruct *Srf,
                                     double SubdivTol,
                                     double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvSelfInterDiagFactor( CagdCrvStruct *Crv,
                                         double SubdivTol,
                                         double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvSelfInterNrmlDev( CagdCrvStruct *Crv,
                                      double SubdivTol,
                                      double NumericTol,
                                      double MinNrmlDeviation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarBzrSelfInter4VarDecomp( CagdSrfStruct *Srf,
                                MvarMVStruct **U1MinusU3Factor,
                                MvarMVStruct **U2MinusU4Factor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarBzrSrfSelfInterDiagFactor( CagdSrfStruct *Srf,
                                                  double SubdivTol,
                                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarBspSrfSelfInterDiagFactor( CagdSrfStruct *Srf,
                                                  double SubdivTol,
                                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarAdjacentSrfSrfInter( CagdSrfStruct *Srf1,
                                             CagdSrfStruct *Srf2,
                                            CagdSrfBndryType Srf1Bndry,
                                            double SubdivTol,
                                            double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfSelfInterDiagFactor( CagdSrfStruct *Srf,
                                               double SubdivTol,
                                               double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfSelfInterNrmlDev( CagdSrfStruct *Srf,
                                            double SubdivTol,
                                            double NumericTol,
                                            double MinNrmlDeviation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCntctTangentialCrvCrvC1( CagdCrvStruct *Crv1,
                                           CagdCrvStruct *Crv2,
                                          double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVsBisector( MvarMVStruct *MV1,
                               MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCrvSrfBisector( MvarMVStruct *MV1,
                                  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarSrfSrfBisector( MvarMVStruct *MV1,
                                  MvarMVStruct *MV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * MvarCrvSrfBisectorApprox2( MvarMVStruct *MV1,
                                   MvarMVStruct *MV2,
                                  int OutputType,
                                  double SubdivTol,
                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarCrvSrfBisectorApprox( MvarMVStruct *CMV1,
                                                  MvarMVStruct *CMV2,
                                                 double SubdivTol,
                                                 double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarZeroSolutionStruct *MvarSrfSrfBisectorApprox( MvarMVStruct *CMV1,
                                                  MvarMVStruct *CMV2,
                                                 double SubdivTol,
                                                 double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * MvarSrfSrfBisectorApprox2( MvarMVStruct *MV1,
                                   MvarMVStruct *MV2,
                                  int OutputType,
                                  double SubdivTol,
                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarCrvCrvBisector2D(CagdCrvStruct *Crv1,
                                    CagdCrvStruct *Crv2, 
                                    double Step, 
                                    double SubdivTol,
                                    double NumericTol, 
                                    double *BBoxMin,
                                    double *BBoxMax,
                                    int SupportPrms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct **MvarTrisector3DCreateMVs(void * FF1, 
                                        void * FF2,
                                        void * FF3,
                                        double *BBoxMin,
                                        double *BBoxMax,
                                        int *Eqns);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarTrisectorCrvs(void * FF1,
                                      void * FF2,
                                      void * FF3,
                                      double Step, 
                                      double SubdivTol,
                                      double NumericTol,
                                      double *BBoxMin,
                                      double *BBoxMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarCrv2DMAT( CagdCrvStruct *OCrv,
                            double SubdivTol,
                            double NumericTol,
                            int InvertOrientation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarComputeVoronoiCell(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarBsctTrimCrvPt(CagdCrvStruct *Crv, 
                                 double *Pt, 
                                 double Alpha,
                                 CagdCrvStruct *BaseCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarUniFuncsComputeLowerEnvelope(CagdCrvStruct *InputCurves, 
                                      CagdCrvStruct **LowerEnvelope);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVBiTangentLine( CagdCrvStruct *Crv1,
                                   CagdCrvStruct *Crv2,
                                  double SubdivTol,
                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVBiTangents( MvarMVStruct *MV1,
                                      MvarMVStruct *MV2,
                                     double SubdivTol,
                                     double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarMVTriTangents( MvarMVStruct *MV1,
                                 MvarMVStruct *MV2,
                                 MvarMVStruct *MV3,
                                int Orientation,
                                double SubdivTol,
                                double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanTo2Crvs( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2,
                                 double Radius,
                                 double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCircTanTo3Crvs( CagdCrvStruct *Crv1,
                                  CagdCrvStruct *Crv2,
                                  CagdCrvStruct *Crv3,
                                 double SubdivTol,
                                 double NumericTol,
                                 int OneSideOrientation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVTriTangentLineCreateMVs( MvarMVStruct *CMV1,
                                    MvarMVStruct *CMV2,
                                    MvarMVStruct *CMV3,
                                   MvarMVStruct **MVs,
                                   MvarConstraintType *Constraints);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMVTriTangentLineCreateETs( MvarMVStruct *CMV1,
                                    MvarMVStruct *CMV2,
                                    MvarMVStruct *CMV3,
                                   MvarExprTreeStruct **ETs,
                                   MvarConstraintType *Constraints);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarMVTriTangentLine( CagdSrfStruct *Srf1,
                                     CagdSrfStruct *Srf2,
                                     CagdSrfStruct *Srf3,
                                    double StepSize,
                                    double SubdivTol,
                                    double NumericTol,
                                    int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarRoundChamferCrvAtC1Discont( CagdCrvStruct *Crv,
                                              MvarCrvCornerType CornerType,
                                              double Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MVarCrvKernel( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MVarCrvGammaKernel( CagdCrvStruct *Crv, double Gamma);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MVarCrvGammaKernelSrf( CagdCrvStruct *Crv,
                                    double ExtentScale,
                                    double GammaMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MVarCrvKernelSilhouette( CagdCrvStruct *Crv,
                                            double Gamma,
                                            double SubEps,
                                            double NumEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MVarCrvDiameter( CagdCrvStruct *Crv,
                                       double SubEps,
                                       double NumEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarDistSrfPoint( CagdSrfStruct *Srf,
                            void *SrfPtPrepHandle,
                             IrtPtType* Pt,
                            int MinDist,
                            double SubdivTol,
                            double NumericTol,
                            MvarPtStruct **ExtremePts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MvarDistSrfPointPrep( CagdSrfStruct *CSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarDistSrfPointFree(void *SrfPtPrepHandle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarLclDistSrfPoint( CagdSrfStruct *Srf,
                                  void *SrfPtPrepHandle,
                                   IrtPtType* Pt,
                                  double SubdivTol,
                                  double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *MvarDistSrfLine( CagdSrfStruct *Srf,
                            IrtPtType* LnPt,
                            IrtVecType* LnDir,
                           int MinDist,
                           double SubdivTol,
                           double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarLclDistSrfLine( CagdSrfStruct *Srf,
                                  IrtPtType* LnPt,
                                  IrtVecType* LnDir,
                                 double SubdivTol,
                                 double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDistCrvSrf( CagdCrvStruct *Crv1,
                                CagdSrfStruct *Srf2,
                               int DistType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVDistSrfSrf( CagdSrfStruct *Srf1,
                                CagdSrfStruct *Srf2,
                               int DistType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MvarInverseCrvOnSrfProjPrep( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarInverseCrvOnSrfProjFree(void *SrfPrepHandle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarInverseCrvOnSrfProj( CagdCrvStruct *E3Crv,
                                        CagdSrfStruct *Srf,
                                       void *SrfPrepHandle,
                                       double ApproxTol,
                                        double *PrevUVPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarIsCrvPreciselyOnSrf( CagdCrvStruct *Crv,
                                        CagdSrfStruct *Srf,
                                       double Step, 
                                       double SubdivTol,
                                       double NumericTol,
                                       void *SrfPrepHandle) ;

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarNumericImporveSharedPoints( CagdSrfStruct *Srf1,
                                   void *DistSrfPointPreps1,
                                   double *UV1,
                                    CagdSrfStruct *Srf2,
                                   void *DistSrfPointPreps2,
                                   double *UV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarProjUVCrvOnE3CrvSameSpeed( CagdCrvStruct *UVLinCrv1,
                                              CagdSrfStruct *Srf1,
                                              CagdCrvStruct *UVCrv2,
                                              CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMakeCrvsOnSrfsSimilarSpeed( CagdSrfStruct *Srf1,
                                    CagdSrfStruct *Srf2,
                                   CagdCrvStruct **UVCrv1,
                                   CagdCrvStruct **UVCrv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarTwoMVsMorphing( MvarMVStruct *MV1,
                                  MvarMVStruct *MV2,
                                 double Blend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarComputeRayTraps( CagdCrvStruct *Crvs,
                                  int Orient,
                                  double SubdivTol,
                                  double NumerTol,
                                  int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarComputeRayTraps3D( CagdSrfStruct *Srfs,
                                    int Orient,
                                    double SubdivTol,
                                    double NumerTol,
                                    int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCtrlComputeCrvNCycle( CagdCrvStruct *Crv,
                                       int CycLen,
                                       double SubdivTol,
                                       double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCtrlComputeSrfNCycle( CagdSrfStruct *Srf,
                                       int CycLen,
                                       double SubdivTol,
                                       double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfAccessibility( CagdSrfStruct *PosSrf,
                                          CagdSrfStruct *OrientSrf,
                                          CagdSrfStruct *CheckSrf,
                                         double SubdivTol,
                                         double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfSilhInflections( CagdSrfStruct *Srf,
                                      IrtVecType* ViewDir,
                                     double SubdivTol,
                                     double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct **MvarFlecnodalCrvsCreateMVCnstrnts( CagdSrfStruct *CSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarSrfFlecnodalCrvs( CagdSrfStruct *Srf, 
                                         double Step, 
                                         double SubdivTol, 
                                         double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfFlecnodalPts( CagdSrfStruct *Srf,
                                  double SubdivTol,
                                  double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MVarProjNrmlPrmt2MVScl( CagdSrfStruct *Srf,
                                      CagdSrfStruct *NrmlSrf,
                                      MvarMVStruct *MVScl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfRadialCurvature( CagdSrfStruct *Srf,
                                      IrtVecType* ViewDir,
                                     double SubdivTol,
                                     double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCrvCrvtrByOneCtlPt( CagdCrvStruct *Crv,
                                     int CtlPtIdx,
                                     double Min,
                                     double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarImplicitCrvExtreme( CagdSrfStruct *Srf,
                                      CagdSrfDirType Dir,
                                     double SubdivTol,
                                     double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarRationalSrfsPoles( CagdSrfStruct *Srf,
                                      double SubdivTol,
                                      double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimSrfStruct *MvarSrfSplitPoleParams( CagdSrfStruct *Srf,
                                             double SubdivTol,
                                             double NumericTol,
                                             double OutReach);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfSilhouette( CagdSrfStruct *Srf,
                                          IrtVecType* VDir,
                                         double Step,
                                         double SubdivTol,
                                         double NumericTol,
                                         int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfSilhouetteThroughPoint( CagdSrfStruct *Srf,
                                                      IrtPtType* VPoint,
                                                     double Step,
                                                     double SubdivTol,
                                                     double NumericTol,
                                                     int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfSilhouetteThroughPoint2(
                                                   MvarMVStruct *SrfMv,
                                                    MvarMVStruct *NrmlMv,
                                                    IrtPtType* VPoint,
                                                   double Step,
                                                   double SubdivTol,
                                                   double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarSkel2DSetEpsilon(double NewEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarSkel2DSetFineNess(double NewFineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarSkel2DSetMZeroTols(double SubdivTol, double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarSkel2DSetOuterExtent(double NewOutExtent);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarSkel2DInter3PrimsStruct *MvarSkel2DInter3Prims(MvarSkel2DPrimStruct *Prim1,
                                                   MvarSkel2DPrimStruct *Prim2,
                                                   MvarSkel2DPrimStruct *Prim3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSkel2DInter3PrimsFree(MvarSkel2DInter3PrimsStruct *SK2DInt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarSkel2DInter3PrimsFreeList(MvarSkel2DInter3PrimsStruct *SK2DIntList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMinSpanCirc( IPObjectStruct *Objs,
                    double *Center,
                    double *Radius,
                    double SubdivTol,
                    double NumerTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarTanHyperSpheresofNManifolds(MvarMVStruct **MVs,
                                              int NumOfMVs,
                                              double SubdivTol,
                                              double NumerTol,
                                              int UseExprTree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarCrvTrimGlblOffsetSelfInter(CagdCrvStruct *Crv,
                                               CagdCrvStruct *OffCrv,
                                              double TrimAmount,
                                              double SubdivTol,
                                              double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfTrimGlblOffsetSelfInter(
                                                   CagdSrfStruct *Srf,
                                                    CagdSrfStruct *OffSrf,
                                                   double TrimAmount,
                                                   int Validate,
                                                   int Euclidean,
                                                   double SubdivTol,
                                                   double NumerTol,
                                                   int NumerImp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarSrfTrimGlblOffsetSelfInterNI(
                                                  IPPolygonStruct *Plls, 
                                                 CagdSrfStruct *OffSrf, 
                                                double SubdivTol, 
                                                double NumerTol,
                                                int Euclidean,
                                                double SameUVTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarDistPointCrvC1(IrtPtType* P,
                              CagdCrvStruct *Crv,
                             MvarHFDistParamStruct *Param,
                             int MinDist,
                             double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFExtremeLclDistPointCrvC1(IrtPtType* P,
                                          CagdCrvStruct *Crv1,
                                          CagdCrvStruct *Crv2,
                                         MvarHFDistParamStruct *Param2,
                                         double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarHFDistAntipodalCrvCrvC1( CagdCrvStruct *Crv1,
                                           CagdCrvStruct *Crv2,
                                          double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarHFDistPairParamStruct *MvarHFDistInterBisectCrvCrvC1(
                                                      CagdCrvStruct *Crv1,
                                                      CagdCrvStruct *Crv2,
                                                     double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistFromCrvToCrvC1( CagdCrvStruct *Crv1,
                                    CagdCrvStruct *Crv2,
                                   MvarHFDistParamStruct *Param1,
                                   MvarHFDistParamStruct *Param2,
                                   double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistCrvCrvC1( CagdCrvStruct *Crv1,
                              CagdCrvStruct *Crv2,
                             MvarHFDistParamStruct *Param1,
                             MvarHFDistParamStruct *Param2,
                             double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistPointSrfC1( IrtPtType* P,
                                CagdSrfStruct *Srf,
                               MvarHFDistParamStruct *Param,
                               int MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFExtremeLclDistPointSrfC1( IrtPtType* P,
                                          CagdSrfStruct *Srf1,
                                          CagdSrfStruct *Srf2,
                                         MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarHFDistAntipodalCrvSrfC1( CagdSrfStruct *Srf1,
                                           CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistFromCrvToSrfC1( CagdCrvStruct *Crv1,
                                    CagdSrfStruct *Srf2,
                                   MvarHFDistParamStruct *Param1,
                                   MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistFromSrfToCrvC1( CagdSrfStruct *Srf1,
                                    CagdCrvStruct *Crv2,
                                   MvarHFDistParamStruct *Param1,
                                   MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistSrfCrvC1( CagdSrfStruct *Srf1,
                              CagdCrvStruct *Crv2,
                             MvarHFDistParamStruct *Param1,
                             MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarHFDistAntipodalSrfSrfC1( CagdSrfStruct *Srf1,
                                           CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarHFDistPairParamStruct *MvarHFDistBisectSrfSrfC1( CagdSrfStruct *Srf1,
                                                     CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarHFDistPairParamStruct *MvarHFDistInterBisectSrfSrfC1(
                                                     CagdSrfStruct *Srf1,
                                                     CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistFromSrfToSrfC1( CagdSrfStruct *Srf1,
                                    CagdSrfStruct *Srf2,
                                   MvarHFDistParamStruct *Param1,
                                   MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarHFDistSrfSrfC1( CagdSrfStruct *Srf1,
                              CagdSrfStruct *Srf2,
                             MvarHFDistParamStruct *Param1,
                             MvarHFDistParamStruct *Param2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvCrvMinimalDist( CagdCrvStruct *Crv1,
                                     CagdCrvStruct *Crv2,
                                    double *MinDist,
                                    int ComputeAntipodals,
                                    double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCrvSrfMinimalDist( CagdSrfStruct *Srf1,
                                     CagdCrvStruct *Crv2,
                                    double *MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarSrfSrfMinimalDist( CagdSrfStruct *Srf1,
                                     CagdSrfStruct *Srf2,
                                    double *MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarCrvMaxXYOriginDistance( CagdCrvStruct *Crv,
                                     double Epsilon,
                                     double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MvarSrfLineOneSidedMaxDist( CagdSrfStruct *Srf,
                                      IrtUVType UV1,
                                      IrtUVType UV2,
                                     CagdSrfDirType ClosedDir,
                                     double Epsilon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarInterSrfLine( IrtVecType* PlnNrml1,
                               double PlnVal1,
                                IrtVecType* PlnNrml2,
                               double PlnVal2,
                                CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarInterSrfCrv( CagdCrvStruct *Crv,
                               CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVOrthoCrvProjOnSrf( CagdCrvStruct *Crv,
                                             CagdSrfStruct *Srf,
                                            double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVOrthoCrvProjOnTrimSrf( CagdCrvStruct *Crv,
                                                 TrimSrfStruct *TSrf,
                                                double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVOrthoCrvProjOnModel( CagdCrvStruct *Crv,
                                               MdlModelStruct *Mdl,
                                              double Tol,
                                              TrimSrfStruct **TSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPolylineStruct *MvarMVOrthoIsoCrvProjOnSrf( CagdSrfStruct *Srf1,
                                                double RVal,
                                                double CrvT0,
                                                double CrvT1,
                                               CagdSrfDirType Dir,
                                                CagdSrfStruct *Srf2,
                                               double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtPeriodic2FloatMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCnvrtFloat2OpenMV( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MvarPiecewiseRuledAlgApproxLineAnalyze(
                                                 CagdSrfStruct *Srf,
                                                double Tolerance, 
                                                CagdCrvStruct **StripBoundries,
                                                int CrvSizeReduction,
                                                double SubdivTol,
                                                double NumericTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarPiecewiseRuledAlgApproxBuildRuledSrfs(
                                             IPObjectStruct *Srf,
                                             IPObjectStruct *TPath);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *MvarPiecewiseDvlpAlgApproxLineAnalyze(
                                               CagdSrfStruct *Srf,
                                              double Tolerance,
                                              CagdCrvStruct **StripBoundriesUV,
                                              int CrvSizeReduction,
                                              double SubdivTol,
                                              double NumericTol,
                                              double SrfExtent,
                                              int DvlpSteps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MvarDevelopSrfFromCrvSrf(
                                               CagdSrfStruct *Srf,
                                               CagdCrvStruct *Crv,
                                               CagdCrvStruct *OrientField,
                                              int CrvSizeReduction,
                                              double SubdivTol,
                                              double NumericTol,
                                              int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *MvarDevelopSrfFromCrvSrfMakeSrfs( CagdCrvStruct *Crv,
                                                 CagdSrfStruct *Srf,
                                                 CagdCrvStruct *UVTCrvs,
                                                int CrvSizeReduction);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MvarZrAlgCreate();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarZrAlgDelete(void *MVZrAlg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarZrAlgAssignExpr(void *MVZrAlg,  byte *Name,  byte *Expr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarZrAlgAssignNumVar(void *MVZrAlg,  byte *Name, double Val);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarZrAlgAssignMVVar(void *MVZrAlg,
                          byte *Name,
                         double DmnMin,
                         double DmnMax,
                          MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarBoolOne( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarBoolSum( CagdSrfStruct *Srf1,
                                 CagdSrfStruct *Srf2,
                                 CagdSrfStruct *Srf3,
                                 CagdSrfStruct *Srf4,
                                 CagdSrfStruct *Srf5,
                                 CagdSrfStruct *Srf6);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarBoolSum2( CagdSrfStruct *UMin,
                                  CagdSrfStruct *UMax,
                                  CagdSrfStruct *VMin,
                                  CagdSrfStruct *VMax,
                                  CagdSrfStruct *WMin,
                                  CagdSrfStruct *WMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarBoolSum3( CagdSrfStruct *Srf1,
                                  CagdSrfStruct *Srf2,
                                  CagdSrfStruct *Srf3,
                                  CagdSrfStruct *Srf4,
                                  CagdSrfStruct *Srf5,
                                  CagdSrfStruct *Srf6);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarCubicTVFit( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *MvarTrivarQuadraticTVFit( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarPtStruct *MvarCalculateExtremePoints( MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarTrivJacobianImprove(TrivTVStruct *TV,
                             double StepSize,
                             int NumIters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarCalculateTVJacobian( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarMakeUniquePointsList(MvarPtStruct **PtList, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVCompose( MvarMVStruct *TargetMV,
                             MvarMVStruct *SrcMV,
                             int *DimMapping,
                            int DoMerge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarSetErrorFuncType MvarSetFatalErrorFunc(MvarSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MvarFatalError(MvarFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *MvarDescribeError(MvarFatalErrorType ErrID);
    }
}
