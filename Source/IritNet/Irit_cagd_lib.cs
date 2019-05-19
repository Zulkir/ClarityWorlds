using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void CagdSetErrorFuncType (CagdFatalErrorType ErrorFunc);
    public unsafe delegate void CagdPrintfFuncType (byte *Line);
    public unsafe delegate void CagdCompFuncType (void * P1, void * P2);
    public unsafe delegate void CagdCrvFuncType (CagdCrvStruct *Crv, double R);
    public unsafe delegate void CagdMatchNormFuncType ( IrtVecType* T1,
                                                    IrtVecType* T2,
                                                    IrtVecType* P1,
                                                    IrtVecType* P2);
    public unsafe delegate void CagdSrfErrorFuncType ( CagdSrfStruct *Srf);
    public unsafe delegate void CagdSrfAdapAuxDataFuncType ( CagdSrfStruct *Srf,
                                                  void * AuxSrfData,
                                                  double t,
                                                  CagdSrfDirType Dir,
                                                  CagdSrfStruct *Srf1,
                                                  void * AuxSrf1Data,
                                                  CagdSrfStruct *Srf2,
                                                  void * AuxSrf2Data);
    public unsafe delegate void CagdSrfAdapPolyGenFuncType (
                                                   CagdSrfStruct *Srf,
                                                  CagdSrfPtStruct *SrfPtList,
                                                   CagdSrfAdapRectStruct *Rect);
    public unsafe delegate void CagdSrfMakeTriFuncType (int ComputeNormals,
                                                             int ComputeUV,
                                                              double *Pt1,
                                                              double *Pt2,
                                                              double *Pt3,
                                                              double *Nl1,
                                                              double *Nl2,
                                                              double *Nl3,
                                                              double *UV1,
                                                              double *UV2,
                                                              double *UV3,
                                                             int *GenPoly);
    public unsafe delegate void CagdSrfMakeRectFuncType (int ComputeNormals,
                                                              int ComputeUV,
                                                               double *Pt1,
                                                               double *Pt2,
                                                               double *Pt3,
                                                               double *Pt4,
                                                               double *Nl1,
                                                               double *Nl2,
                                                               double *Nl3,
                                                               double *Nl4,
                                                               double *UV1,
                                                               double *UV2,
                                                               double *UV3,
                                                               double *UV4,
                                                              int *GenPoly);
    public unsafe delegate void CagdPlgErrorFuncType ( IrtPtType* P1,
                                                   IrtPtType* P2,
                                                   IrtPtType* P3);
    public unsafe delegate void CagdCrvTestingFuncType ( CagdCrvStruct *Crv,
                                                    double *t);
    public unsafe delegate void CagdSrfTestingFuncType ( CagdSrfStruct *Srf,
                                                    CagdSrfDirType Dir,
                                                    double *t);
    public unsafe delegate void CagdQuadSrfWeightFuncType ( CagdSrfStruct *QuadSrf, 
                                                       CagdCrvStruct *BoundaryCrv,
                                                       CagdPolylineStruct 
                                                                      *SampledPolygon, 
                                                       int *VIndices,
                                                      int numV);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdUVStruct *CagdUVNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdPtNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfPtStruct *CagdSrfPtNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCtlPtStruct *CagdCtlPtNew(CagdPointType PtType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdVecNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlaneStruct *CagdPlaneNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *CagdBBoxNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvNew(CagdGeomType GType, CagdPointType PType, int Length);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdPeriodicCrvNew(CagdGeomType GType,
                                  CagdPointType PType,
                                  int Length,
                                  int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfNew(CagdGeomType GType,
                          CagdPointType PType,
                          int ULength,
                          int VLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPeriodicSrfNew(CagdGeomType GType,
                                  CagdPointType PType,
                                  int ULength,
                                  int VLength,
                                  int UPeriodic,
                                  int VPeriodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdPolygonNew(int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdPolygonStripNew(int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdPolylineNew(int Length);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdUVStruct *CagdUVArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdPtArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCtlPtStruct *CagdCtlPtArrayNew(CagdPointType PtType, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdVecArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlaneStruct *CagdPlaneArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *CagdBBoxArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdPolygonArrayNew(int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdPolylineArrayNew(int Length, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCopy( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfCopy( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdUVStruct *CagdUVCopy( CagdUVStruct *UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdPtCopy( CagdPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfPtStruct *CagdSrfPtCopy( CagdSrfPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCtlPtStruct *CagdCtlPtCopy( CagdCtlPtStruct *CtlPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdVecCopy( CagdVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlaneStruct *CagdPlaneCopy( CagdPlaneStruct *Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *CagdBBoxCopy( GMBBBboxStruct *BBoc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdPolygonCopy( CagdPolygonStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdPolylineCopy( CagdPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCopyList( CagdCrvStruct *CrvList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfCopyList( CagdSrfStruct *SrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdUVStruct *CagdUVCopyList( CagdUVStruct *UVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdPtCopyList( CagdPtStruct *PtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfPtStruct *CagdSrfPtCopyList( CagdSrfPtStruct *SrfPtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCtlPtStruct *CagdCtlPtCopyList( CagdCtlPtStruct *CtlPtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdVecCopyList( CagdVecStruct *VecList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlaneStruct *CagdPlaneCopyList( CagdPlaneStruct *PlaneList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdPolylineCopyList( CagdPolylineStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdPolygonCopyList( CagdPolygonStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvFree(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvFreeList(CagdCrvStruct *CrvList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfFree(CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfFreeList(CagdSrfStruct *SrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfFreeCache(CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdUVFree(CagdUVStruct *UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdUVFreeList(CagdUVStruct *UVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdUVArrayFree(CagdUVStruct *UVArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPtFree(CagdPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPtFreeList(CagdPtStruct *PtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfPtFree(CagdSrfPtStruct *SrfPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfPtFreeList(CagdSrfPtStruct *SrfPtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPtArrayFree(CagdPtStruct *PtArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCtlPtFree(CagdCtlPtStruct *CtlPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCtlPtFreeList(CagdCtlPtStruct *CtlPtList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCtlPtArrayFree(CagdCtlPtStruct *CtlPtArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdVecFree(CagdVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdVecFreeList(CagdVecStruct *VecList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdVecArrayFree(CagdVecStruct *VecArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPlaneFree(CagdPlaneStruct *Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPlaneFreeList(CagdPlaneStruct *PlaneList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPlaneArrayFree(CagdPlaneStruct *PlaneArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBBoxFree(GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBBoxArrayFree(GMBBBboxStruct *BBoxArray, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolylineFree(CagdPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolylineFreeList(CagdPolylineStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolygonFree(CagdPolygonStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolygonFreeList(CagdPolygonStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListInsert(void * List,
                       void * NewElement,
                       CagdCompFuncType CompFunc,
                       int InsertEqual);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdListLength( void * List);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListAppend(void * List1, void * List2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListReverse(void * List);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListLast( void * List);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListPrev( void * List,  void * Item);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte CagdListFind( void * List,  void * Elem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListNth( void * List, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * CagdListSort(void * List,  byte *AttribName, int Ascending);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCoercePointTo(double *NewPoint,
                       CagdPointType NewPType,
                       double  *  *Points,
                       int Index,
                       CagdPointType OldPType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdDistTwoCtlPt(double  **Pt1,
                           int Index1,
                           double  **Pt2,
                           int Index2,
                           CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdDistTwoCtlPt2(double  *  *Pts,
                            int Index1,
                            int Index2,
                            CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdRealVecSame(double  *Vec1,
                          double  *Vec2,
                          int Len,
                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCoerceCrvsTo( CagdCrvStruct *Crv,
                                CagdPointType PType,
                                int AddParametrization);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCoerceCrvTo( CagdCrvStruct *Crv,
                               CagdPointType PType,
                               int AddParametrization);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCoerceSrfsTo( CagdSrfStruct *Srf,
                                CagdPointType PType,
                                int AddParametrization);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCoerceSrfTo( CagdSrfStruct *Srf,
                               CagdPointType PType,
                               int AddParametrization);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPointType CagdMergePointTypes(CagdPointType PType1, CagdPointType PType2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdDbg( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdDbgDsp( void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSetLinear2Poly(CagdLin2PolyType Lin2Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdTightBBox(int TightBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIgnoreNonPosWeightBBox(int IgnoreNonPosWeightBBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCtlPtBBox( CagdCtlPtStruct *CtlPt, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCtlPtListBBox( CagdCtlPtStruct *CtlPts, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPointsBBox2(double *  *Points,
                     int Min,
                     int Max,
                     int Dim,
                     double *BBoxMin,
                     double *BBoxMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPointsBBox(double *  *Points,
                    int Length,
                    int Dim,
                    double *BBoxMin,
                    double *BBoxMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdIChooseK(int i, int k);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdTransform(double **Points,
                   int Len,
                   int MaxCoord,
                   int IsNotRational,
                    double *Translate,
                   double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdTransform2(double **Points,
                    int Len,
                    int MaxCoord,
                    int IsNotRational,
                     double *Translate,
                    double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdScale(double **Points,
               int Len,
               int MaxCoord,
                double *Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdMatTransform(double **Points,
                      int Len,
                      int MaxCoord,
                      int IsNotRational,
                      IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdPointsHasPoles(double *  *Points, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdAllWeightsNegative(double *  *Points,
                                 CagdPointType PType,
                                 int Len,
                                 int Flip);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdAllWeightsSame(double *  *Points, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlgErrorFuncType CagdPolygonSetErrFunc(CagdPlgErrorFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfMakeTriFuncType CagdSrfSetMakeTriFunc(CagdSrfMakeTriFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfMakeRectFuncType CagdSrfSetMakeRectFunc(CagdSrfMakeRectFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfSetMakeOnlyTri(int OnlyTri);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdMakeTriangle(int ComputeNormals,
                                    int ComputeUV,
                                     double *Pt1,
                                     double *Pt2,
                                     double *Pt3,
                                     double *Nl1,
                                     double *Nl2,
                                     double *Nl3,
                                     double *UV1,
                                     double *UV2,
                                     double *UV3,
                                    int *GenPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdMakeRectangle(int ComputeNormals,
                                     int ComputeUV,
                                      double *Pt1,
                                      double *Pt2,
                                      double *Pt3,
                                      double *Pt4,
                                      double *Nl1,
                                      double *Nl2,
                                      double *Nl3,
                                      double *Nl4,
                                      double *UV1,
                                      double *UV2,
                                      double *UV3,
                                      double *UV4,
                                     int *GenPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdPtsSortAxis(CagdPtStruct *PtList, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvOnOneSideOfLine( CagdCrvStruct *Crv,
                                 double X1,
                                 double Y1,
                                 double X2,
                                 double Y2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolygonBBox( CagdPolygonStruct *Poly, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdPolygonListBBox( CagdPolygonStruct *Polys, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBlsmAlphaCoeffStruct *CagdBlsmAllocAlphaCoef(int Order,
                                                 int Length,
                                                 int NewOrder,
                                                 int NewLength,
                                                 int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBlsmAlphaCoeffStruct *CagdBlsmCopyAlphaCoef( CagdBlsmAlphaCoeffStruct
                                                                           *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBlsmFreeAlphaCoef(CagdBlsmAlphaCoeffStruct *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBlsmAddRowAlphaCoef(CagdBlsmAlphaCoeffStruct *A,
                             double *Coefs,
                             int ARow,
                             int ColIndex,
                             int ColLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBlsmSetDomainAlphaCoef(CagdBlsmAlphaCoeffStruct *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdBlsmScaleAlphaCoef(CagdBlsmAlphaCoeffStruct *A, double Scl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdBlsmEvalSymb(int Order,
                             double *Knots,
                            int KnotsLen,
                             double *BlsmVals,
                            int BlsmLen,
                            int *RetIdxFirst,
                            int *RetLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdBlossomEval( double *Pts,
                          int PtsStep,
                          int Order,
                           double *Knots,
                          int KnotsLen,
                           double *BlsmVals,
                          int BlsmLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdCrvBlossomEval( CagdCrvStruct *Crv,
                               double *BlsmVals,
                              int BlsmLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdSrfBlossomEval( CagdSrfStruct *Srf,
                               double *BlsmUVals,
                              int BlsmULen,
                               double *BlsmVVals,
                              int BlsmVLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdSrfBlossomEvalU( CagdSrfStruct *Srf,
                                    double *BlsmUVals,
                                   int BlsmULen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvBlossomDegreeRaiseN( CagdCrvStruct *Crv,
                                          int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvBlossomDegreeRaise( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfBlossomDegreeRaiseN( CagdSrfStruct *Srf,
                                          int NewUOrder,
                                          int NewVOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfBlossomDegreeRaise( CagdSrfStruct *Srf,
                                         CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBlsmAlphaCoeffStruct *CagdDegreeRaiseMatProd(CagdBlsmAlphaCoeffStruct *A1,
                                                 CagdBlsmAlphaCoeffStruct *A2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBlsmAlphaCoeffStruct *CagdBlossomDegreeRaiseMat( double *KV,
                                                    int Order,
                                                    int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBlsmAlphaCoeffStruct *CagdBlossomDegreeRaiseNMat( double *KV,
                                                     int Order,
                                                     int NewOrder,
                                                     int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdFitPlaneThruCtlPts(CagdPlaneStruct *Plane,
                                 CagdPointType PType,
                                 double  **Points,
                                 int Index1,
                                 int Index2,
                                 int Index3,
                                 int Index4);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdDistPtPlane( CagdPlaneStruct *Plane,
                          double *  *Points,
                          int Index,
                          int MaxDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdDistPtLine( CagdVecStruct *LineDir,
                         double *  *Points,
                         int Index,
                         int MaxDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvMatTransform( CagdCrvStruct *Crv,
                                   IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfMatTransform( CagdSrfStruct *Srf,
                                   IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvScale(CagdCrvStruct *Crv,  double *Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvTransform(CagdCrvStruct *Crv,
                       double *Translate,
                      double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfScale(CagdSrfStruct *Srf,  double *Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfTransform(CagdSrfStruct *Srf,
                       double *Translate,
                      double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvUnitMaxCoef(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfUnitMaxCoef(CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRotateToXY( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvRotateToXYMat( CagdCrvStruct *Crv, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdCrvNodes( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdEstimateCrvCollinearity( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvDomain( CagdCrvStruct *Crv, double *TMin, double *TMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSetDomain(CagdCrvStruct *Crv,
                                double TMin,
                                double TMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdCrvEval( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDerive( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDeriveScalar( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvScalarCrvSlopeBounds( CagdCrvStruct *Crv,
                                 double *MinSlope,
                                 double *MaxSlope);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvIntegrate( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrv2DNormalField( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvMoebiusTransform( CagdCrvStruct *Crv, double c);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvIsCtlPolyMonotone( CagdCrvStruct *Crv, int Axis, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSubdivAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSubdivAtParams( CagdCrvStruct *Crv,
                                      CagdPtStruct *Pts,
                                     double Eps,
                                     int *Proximity);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSubdivAtParams2( CagdCrvStruct *CCrv,
                                       CagdPtStruct *Pts,
                                      int Idx,
                                      double Eps,
                                      int *Proximity);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRegionFromCrv( CagdCrvStruct *Crv,
                                    double t1,
                                    double t2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRegionFromCrvWrap( CagdCrvStruct *Crv,
                                        double t1,
                                        double t2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRefineAtParams( CagdCrvStruct *Crv,
                                     int Replace,
                                     double *t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRefineUniformly( CagdCrvStruct *Crv, int RefLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvRefineUniformly2( CagdCrvStruct *Crv, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdCrvTangent( CagdCrvStruct *Crv,
                              double t,
                              int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdCrvBiNormal( CagdCrvStruct *Crv,
                               double t,
                               int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdCrvNormal( CagdCrvStruct *Crv,
                             double t,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdCrvNormalXY( CagdCrvStruct *Crv,
                               double t,
                               int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdCrv2Polyline( CagdCrvStruct *Crv,
                                     int SamplesPerCurve,
                                     int OptiLin);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvReverse( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvReverseUV( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSubdivAtAllC0Discont( CagdCrvStruct *Crv,
                                           byte EuclideanC1Discont,
                                           double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvSubdivAtAllC1Discont( CagdCrvStruct *Crv,
                                           byte EuclideanC1Discont,
                                           double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvSubdivAtAllDetectedLocations( CagdCrvStruct *Crv,
                                                  CagdCrvTestingFuncType
                                                                CrvTestFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDegreeRaise( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDegreeRaiseN( CagdCrvStruct *Crv, int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDegreeReduce( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCreateArc( CagdPtStruct *Center,
                                double Radius,
                                double StartAngle,
                                double EndAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCreateArcCCW( CagdPtStruct *Start,
                                    CagdPtStruct *Center,
                                    CagdPtStruct *End);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCreateArcCW( CagdPtStruct *Start,
                                   CagdPtStruct *Center,
                                   CagdPtStruct *End);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCreateConicCurve(double A,
                                    double B,
                                    double C,
                                    double D,
                                    double E,
                                    double F,
                                    double ZLevel,
                                    int RationalEllipses);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCreateConicCurve2(double A,
                                     double B,
                                     double C,
                                     double D,
                                     double E,
                                     double F,
                                     double ZLevel,
                                      double *PStartXY,
                                      double *PEndXY,
                                     int RationalEllipses);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdEllipse3Points(IrtPtType* Pt1,
                       IrtPtType* Pt2,
                       IrtPtType* Pt3,
                       double *A,
                       double *B,
                       double *C,
                       double *D,
                       double *E,
                       double *F);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdEllipse4Points(IrtPtType* Pt1,
                       IrtPtType* Pt2,
                       IrtPtType* Pt3,
                       IrtPtType* Pt4,
                       double *A,
                       double *B,
                       double *C,
                       double *D,
                       double *E,
                       double *F);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdEllipseOffset(double *A,
                      double *B,
                      double *C,
                      double *D,
                      double *E,
                      double *F,
                      double Offset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdConicMatTransform(double *A,
                          double *B,
                          double *C,
                          double *D,
                          double *E,
                          double *F,
                          IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdQuadricMatTransform(double *A,
                            double *B,
                            double *C,
                            double *D,
                            double *E,
                            double *F,
                            double *G,
                            double *H,
                            double *I,
                            double *J,
                            IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdConic2Quadric(double *A,
                      double *B,
                      double *C,
                      double *D,
                      double *E,
                      double *F,
                      double *G,
                      double *H,
                      double *I,
                      double *J);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCreateQuadricSrf(double A,
                                    double B,
                                    double C,
                                    double D,
                                    double E,
                                    double F,
                                    double G,
                                    double H,
                                    double I,
                                    double J);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeCrvCrv( CagdCrvStruct *Crv1,
                                CagdCrvStruct *Crv2,
                               int InterpDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeCrvList( CagdCrvStruct *CrvList,
                                int InterpDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeCrvList2(CagdCrvStruct *CrvList,
                                 double Tolerance,
                                 int InterpDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeCrvPt( CagdCrvStruct *Crv,
                               CagdPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergePtCrv( CagdPtStruct *Pt,
                               CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergePtPt( CagdPtStruct *Pt1,  CagdPtStruct *Pt2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergePtPt2( IrtPtType* Pt1,  IrtPtType* Pt2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeUvUv( IrtUVType UV1,  IrtUVType UV2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMergeCtlPtCtlPt( CagdCtlPtStruct *Pt1,
                                    CagdCtlPtStruct *Pt2,
                                   int MinDim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdCrvAreaPoly( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdCrvArcLenPoly( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdLimitCrvArcLen( CagdCrvStruct *Crv, double MaxLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdCrv2CtrlPoly( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdEditSingleCrvPt(CagdCrvStruct *Crv,
                                   CagdCtlPtStruct *CtlPt,
                                   int Index,
                                   int Write);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvDeletePoint( CagdCrvStruct *Crv, int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvInsertPoint( CagdCrvStruct *Crv,
                                  int Index,
                                   IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdMakeCrvsCompatible(CagdCrvStruct **Crv1,
                                 CagdCrvStruct **Crv2,
                                 int SameOrder,
                                 int SameKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdMakeCrvsCompatible2(CagdCrvStruct **Crv1,
                                  CagdCrvStruct **Crv2,
                                  int SameOrder,
                                  int SameKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvBBox( CagdCrvStruct *Crv, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvListBBox( CagdCrvStruct *Crvs, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvIsConstant( CagdCrvStruct *Crv, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvMinMax( CagdCrvStruct *Crv,
                   int Axis,
                   double *Min,
                   double *Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdCtlMeshAverageValue(double *  *Pts,
                                  int Length,
                                  int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdCrvAverageValue( CagdCrvStruct *Crv, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIsCrvInsideCH( CagdCrvStruct *Crv,
                       IrtE2PtStruct *CHPts,
                      int NumCHPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvFirstMoments( CagdCrvStruct *Crv,
                         int n,
                         IrtPtType* Pt,
                         IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCubicHermiteCrv( IrtPtType* Pt1,
                                    IrtPtType* Pt2,
                                    IrtVecType* Dir1,
                                    IrtVecType* Dir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCubicCrvFit( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdQuadraticCrvFit( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdQuinticHermiteSrf( CagdCrvStruct *CPos1Crv,
                                      CagdCrvStruct *CPos2Crv,
                                      CagdCrvStruct *CDir1Crv,
                                      CagdCrvStruct *CDir2Crv,
                                      CagdCrvStruct *C2Dir1Crv,
                                      CagdCrvStruct *C2Dir2Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCubicSrfFit( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdQuadraticSrfFit( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdMatchDistNorm( IrtVecType* T1,
                             IrtVecType* T2,
                             IrtVecType* P1,
                             IrtVecType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdMatchRuledNorm( IrtVecType* T1,
                              IrtVecType* T2,
                              IrtVecType* P1,
                              IrtVecType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdMatchRuled2Norm( IrtVecType* T1,
                               IrtVecType* T2,
                               IrtVecType* P1,
                               IrtVecType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdMatchBisectorNorm( IrtVecType* T1,
                                 IrtVecType* T2,
                                 IrtVecType* P1,
                                 IrtVecType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdMatchMorphNorm( IrtVecType* T1,
                              IrtVecType* T2,
                              IrtVecType* P1,
                              IrtVecType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdMatchingFixVector(int *OldVec, double *NewVec, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdMatchingFixCrv(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdMatchingPolyTransform(double **Poly,
                               int Len,
                               double NewBegin,
                               double NewEnd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdMatchingVectorTransform(double *Vec,
                                 double NewBegin,
                                 double NewEnd,
                                 int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdMatchingTwoCurves( CagdCrvStruct *Crv1,
                                      CagdCrvStruct *Crv2,
                                     int Reduce,
                                     int SampleSet,
                                     int ReparamOrder,
                                     int RotateFlag,
                                     int AllowNegativeNorm,
                                     int ReturnReparamFunc,
                                     int MinimizeMaxError,
                                     CagdMatchNormFuncType MatchNormFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvTwoCrvsOrient(CagdCrvStruct *Crv1, CagdCrvStruct *Crv2, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIsClosedCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdAreClosedCrvs( CagdCrvStruct *Crvs,
                             CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIsZeroLenCrv( CagdCrvStruct *Crv, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvTanAngularSpan( CagdCrvStruct *Crv,
                                IrtVecType* ConeDir,
                                double *AngularSpan);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdDistCrvLine( CagdCrvStruct *Crv, IrtLnType* Line);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdCrvCrvInter( CagdCrvStruct *Crv1,
                               CagdCrvStruct *Crv2,
                              double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdInsertInterPointReset();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdInsertInterPoints(double t1, double t2, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvCrvInterArrangment( CagdCrvStruct *ArngCrvs,
                                         int SplitCrvs,
                                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvsSameFuncSpace( CagdCrvStruct *Crv1,
                                 CagdCrvStruct *Crv2,
                                double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvsSame( CagdCrvStruct *Crv1,
                        CagdCrvStruct *Crv2,
                       double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvsSame2( CagdCrvStruct *Crv1,
                         CagdCrvStruct *Crv2,
                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvsSame3( CagdCrvStruct *Crv1,
                         CagdCrvStruct *Crv2,
                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvsSameUptoRigidScl2D( CagdCrvStruct *Crv1,
                                      CagdCrvStruct *Crv2,
                                     IrtPtType* Trans,
                                     double *Rot,
                                     double *Scl,
                                     double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdCrvZeroSet( CagdCrvStruct *Crv,
                             int Axis,
                             int NRInit,
                             double NumericTol,
                             double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdCrvZeroSetC0( CagdCrvStruct *Crv,
                               int Axis,
                               int NRInit,
                               double NumericTol,
                               double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdSrfNodes( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdEstimateSrfPlanarity( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfDomain( CagdSrfStruct *Srf,
                   double *UMin,
                   double *UMax,
                   double *VMin,
                   double *VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfSetDomain(CagdSrfStruct *Srf,
                                double UMin,
                                double UMax,
                                double VMin,
                                double VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdSrfEval( CagdSrfStruct *Srf, double u, double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfEstimateCurveness( CagdSrfStruct *Srf,
                              double *UCurveness,
                              double *VCurveness);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdSrf2Polygons( CagdSrfStruct *Srf,
                                    int FineNess,
                                    int ComputeNormals,
                                    int FourPerFlat,
                                    int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdSrf2PolygonsN( CagdSrfStruct *Srf,
                                     int Nu,
                                     int Nv,
                                     int ComputeNormals,
                                     int FourPerFlat,
                                     int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfErrorFuncType CagdSrf2PolyAdapSetErrFunc(CagdSrfErrorFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfAdapAuxDataFuncType
              CagdSrf2PolyAdapSetAuxDataFunc(CagdSrfAdapAuxDataFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfAdapPolyGenFuncType
              CagdSrf2PolyAdapSetPolyGenFunc(CagdSrfAdapPolyGenFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfAdap2PolyDefErrFunc( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfIsLinearCtlMeshOneRowCol( CagdSrfStruct *Srf,
                                          int Idx,
                                          CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfIsLinearCtlMesh( CagdSrfStruct *Srf, int Interior);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfIsLinearBndryCtlMesh( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfIsCoplanarCtlMesh( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdSrfAdap2Polygons( CagdSrfStruct *Srf,
                                        double Tolerance,
                                        int ComputeNormals,
                                        int FourPerFlat,
                                        int ComputeUV,
                                        void * AuxSrfData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdSrf2PolygonsGenPolys( CagdSrfStruct *Srf,
                                            int FourPerFlat,
                                            double *PtWeights,
                                            CagdPtStruct *PtMesh,
                                            CagdVecStruct *PtNrml,
                                            CagdUVStruct *UVMesh,
                                            int FineNessU,
                                            int FineNessV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *CagdSrfAdapRectPolyGen( CagdSrfStruct *Srf,
                                          CagdSrfPtStruct *SrfPtList,
                                           CagdSrfAdapRectStruct *Rect);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdSrfAdap2PolyEvalNrmlBlendedUV( double *UV1,
                                              double *UV2,
                                              double *UV3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrf2PolygonStrip(int PolygonStrip);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrf2PolygonFast(int PolygonFast);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrf2PolygonMergeCoplanar(int MergeCoplanarPolys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Cagd2PolyClipPolysAtPoles(int ClipPolysAtPoles);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdEvaluateSurfaceVecField(IrtVecType* Vec,
                                 CagdSrfStruct *VecFieldSrf,
                                 double U,
                                 double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfDerive( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfDeriveScalar( CagdSrfStruct *Srf,
                                   CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfIntegrate( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfMoebiusTransform( CagdSrfStruct *Srf,
                                       double c,
                                       CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvFromSrf( CagdSrfStruct *Srf,
                              double t,
                              CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvFromMesh( CagdSrfStruct *Srf,
                               int Index,
                               CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct **CagdBndryCrvsFromSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdBndryCrvFromSrf( CagdSrfStruct *Srf,
                                   CagdSrfBndryType Bndry);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvToMesh( CagdCrvStruct *Crv,
                   int Index,
                   CagdSrfDirType Dir,
                   CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfSubdivAtParam( CagdSrfStruct *Srf,
                                    double t,
                                    CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfSubdivAtAllDetectedLocations( CagdSrfStruct *Srf,
                                                  CagdSrfTestingFuncType
                                                                 SrfTestFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfRegionFromSrf( CagdSrfStruct *Srf,
                                    double t1,
                                    double t2,
                                    CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfRefineAtParams( CagdSrfStruct *Srf,
                                     CagdSrfDirType Dir,
                                     int Replace,
                                     double *t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdSrfTangent( CagdSrfStruct *Srf,
                              double u,
                              double v,
                              CagdSrfDirType Dir,
                              int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdSrfNormal( CagdSrfStruct *Srf,
                             double u,
                             double v,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtUVType *CagdSrfUVDirOrthoE3( CagdSrfStruct *Srf,
                                 IrtUVType *UV,
                                 IrtUVType *UVDir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfDegreeRaise( CagdSrfStruct *Srf,
                                  CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfDegreeRaiseN( CagdSrfStruct *Srf,
                                   int NewUOrder,
                                   int NewVOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfReverse( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfReverseDir( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfReverse2( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfMakeBoundryIndexUMin( CagdSrfStruct *Srf,
                                           int BndryIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdSrf2CtrlMesh( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdSrf2KnotLines( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdSrf2KnotCurves( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdMergeSrfSrf( CagdSrfStruct *CSrf1,
                                CagdSrfStruct *CSrf2,
                               CagdSrfDirType Dir,
                               int SameEdge,
                               int InterpolateDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdMergeSrfList( CagdSrfStruct *SrfList,
                                CagdSrfDirType Dir,
                                int SameEdge,
                                int InterpolateDiscont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfAvgArgLenMesh( CagdSrfStruct *Srf,
                               double *AvgULen,
                               double *AvgVLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdExtrudeSrf( CagdCrvStruct *Crv,
                               CagdVecStruct *Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdZTwistExtrudeSrf( CagdCrvStruct *CCrv,
                                    int Rational,
                                    double ZPitch);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSurfaceRev( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSurfaceRevAxis( CagdCrvStruct *Crv, IrtVecType* Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSurfaceRev2( CagdCrvStruct *Crv,
                               int PolyApprox,
                               double StartAngle,
                               double EndAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSurfaceRev2Axis( CagdCrvStruct *Crv,
                                   int PolyApprox,
                                   double StartAngle,
                                   double EndAngle,
                                    IrtVecType* Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSurfaceRevPolynomialApprox( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSweepSrf(CagdCrvStruct *CrossSection,
                            CagdCrvStruct *Axis,
                             CagdCrvStruct *ScalingCrv,
                            double Scale,
                             void * Frame,
                            int FrameIsCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdSweepAxisRefine( CagdCrvStruct *Axis,
                                    CagdCrvStruct *ScalingCrv,
                                   int RefLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvOrientationFrame(CagdCrvStruct *Crv,
                                  double CrntT,
                                  CagdVecStruct *Tangent,
                                  CagdVecStruct *Normal,
                                  int FirstTime);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdBoolSumSrf( CagdCrvStruct *CrvLeft,
                               CagdCrvStruct *CrvRight,
                               CagdCrvStruct *CrvTop,
                               CagdCrvStruct *CrvBottom);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdOneBoolSumSrf( CagdCrvStruct *BndryCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdReorderCurvesInLoop(CagdCrvStruct *UVCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfFromNBndryCrvs( CagdCrvStruct *Crvs,
                                     int MinimizeSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdRuledSrf( CagdCrvStruct *Crv1,
                             CagdCrvStruct *Crv2,
                            int OtherOrder,
                            int OtherLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdBilinearSrf( CagdPtStruct *Pt00,
                                CagdPtStruct *Pt01,
                                CagdPtStruct *Pt10,
                                CagdPtStruct *Pt11);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPromoteCrvToSrf( CagdCrvStruct *Crv,
                                   CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfFromCrvs( CagdCrvStruct *CrvList,
                               int OtherOrder,
                               CagdEndConditionType OtherEC,
                               double *OtherParamVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdSrfInterpolateCrvsChordLenParams( CagdCrvStruct *CrvList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfInterpolateCrvs( CagdCrvStruct *CrvList,
                                      int OtherOrder,
                                      CagdEndConditionType OtherEC,
                                      CagdParametrizationType OtherParam,
                                      double *OtherParamVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdMakeSrfsCompatible(CagdSrfStruct **Srf1,
                                 CagdSrfStruct **Srf2,
                                 int SameUOrder,
                                 int SameVOrder,
                                 int SameUKV,
                                 int SameVKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdMakeSrfsCompatible2(CagdSrfStruct **Srf1,
                                  CagdSrfStruct **Srf2,
                                  int SameUOrder,
                                  int SameVOrder,
                                  int SameUKV,
                                  int SameVKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdEditSingleSrfPt(CagdSrfStruct *Srf,
                                   CagdCtlPtStruct *CtlPt,
                                   int UIndex,
                                   int VIndex,
                                   int Write);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfBBox( CagdSrfStruct *Srf, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfListBBox( CagdSrfStruct *Srfs, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfIsConstant( CagdSrfStruct *Srf, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfMinMax( CagdSrfStruct *Srf,
                   int Axis,
                   double *Min,
                   double *Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdSrfAverageValue( CagdSrfStruct *Srf, int Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdPolyApproxMaxErr( CagdSrfStruct *Srf,
                                CagdPolygonStruct *Polys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *CagdPolyApproxErrs( CagdSrfStruct *Srf,
                               CagdPolygonStruct *Polys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdPolyApproxErrEstimate( CagdPolyErrEstimateType Method,
                              int Samples);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCubicHermiteSrf( CagdCrvStruct *CPos1Crv,
                                    CagdCrvStruct *CPos2Crv,
                                    CagdCrvStruct *CDir1Crv,
                                    CagdCrvStruct *CDir2Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdBlendTwoSurfaces( CagdSrfStruct *Srf1,
                                     CagdSrfStruct *Srf2,
                                    int BlendDegree,
                                    double TanScale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIsClosedSrf( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdIsZeroLenSrfBndry( CagdSrfStruct *Srf,
                                CagdSrfBndryType Bdnry,
                                double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSameCorners( CagdSrfStruct *Srf1,
                               CagdSrfStruct *Srf2,
                              double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSameFuncSpace( CagdSrfStruct *Srf1,
                                 CagdSrfStruct *Srf2,
                                double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSame( CagdSrfStruct *Srf1,
                        CagdSrfStruct *Srf2,
                       double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSame2( CagdSrfStruct *Srf1,
                         CagdSrfStruct *Srf2,
                        double Eps,
                        int *Modified);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSame3( CagdSrfStruct *Srf1,
                         CagdSrfStruct *Srf2,
                        double Eps,
                        int *Modified);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvUpdateLength(CagdCrvStruct *Crv, int NewLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfUpdateLength(CagdSrfStruct *Srf,
                                   int NewLength,
                                   CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfBndryType CagdSrfIsPtIndexBoundary(CagdSrfStruct *Srf, int PtIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfBndryType CagdCrvonSrfBndry( CagdCrvStruct *Crv,
                                    CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfsSameUptoRigidScl2D( CagdSrfStruct *Srf1,
                                      CagdSrfStruct *Srf2,
                                     IrtPtType* Trans,
                                     double *Rot,
                                     double *Scl,
                                     double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfEffiNrmlPrelude( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *CagdSrfEffiNrmlEval(double u,
                                   double v,
                                   int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdSrfEffiNrmlPostlude();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvNew(int Length, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *PwrCrvEvalAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvDerive( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvDeriveScalar( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvIntegrate( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvDegreeRaiseN( CagdCrvStruct *Crv, int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *PwrCrvDegreeRaise( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *PwrSrfNew(int ULength, int VLength, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *PwrSrfDegreeRaiseN( CagdSrfStruct *Srf,
                                  int NewUOrder,
                                  int NewVOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *PwrSrfDegreeRaise( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvNew(int Length, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BzrCrvEvalBasisFunc(int i, int k, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BzrCrvEvalBasisFuncs(int k, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BzrCrvEvalVecAtParam( double *Vec,
                               int VecInc,
                               int Order,
                               double t,
                               double *BasisFuncs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BzrCrvEvalAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrCrvSetCache(int FineNess, int EnableCache);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvCreateArc( CagdPtStruct *Start,
                                CagdPtStruct *Center,
                                CagdPtStruct *End);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrCrvSubdivCtlPoly(double *  *Points,
                         double **LPoints,
                         double **RPoints,
                         int Length,
                         CagdPointType PType,
                         double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrCrvSubdivCtlPolyStep(double *  *Points,
                             double **LPoints,
                             double **RPoints,
                             int Length,
                             CagdPointType PType,
                             double t,
                             int Step);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvSubdivAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvDegreeRaise( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvDegreeRaiseN( CagdCrvStruct *Crv, int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvDegreeReduce( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvDerive( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvDeriveScalar( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvIntegrate( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvMoebiusTransform( CagdCrvStruct *Crv, double c);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrCrvTangent( CagdCrvStruct *Crv,
                             double t,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrCrvBiNormal( CagdCrvStruct *Crv,
                              double t,
                              int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrCrvNormal( CagdCrvStruct *Crv,
                            double t,
                            int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *BzrCrv2Polyline( CagdCrvStruct *Crv,
                                    int SamplesPerCurve);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrCrvInterp2(double *Result,  double *Input, int Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *BzrZeroSetNoSubdiv( CagdCrvStruct *Crv,
                                 int Axis,
                                 double NumericTol,
                                 double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfNew(int ULength, int VLength, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BzrSrfEvalAtParam( CagdSrfStruct *Srf,
                             double u,
                             double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrSrfCrvFromSrf( CagdSrfStruct *Srf,
                                double t,
                                CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrSrfCrvFromMesh( CagdSrfStruct *Srf,
                                 int Index,
                                 CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BzrSrfSubdivCtlMesh(double *  *Points,
                         double **LPoints,
                         double **RPoints,
                         int ULength,
                         int VLength,
                         CagdPointType PType,
                         double t,
                         CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfSubdivAtParam( CagdSrfStruct *Srf,
                                   double t,
                                   CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfDegreeRaise( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfDegreeRaiseN( CagdSrfStruct *Srf,
                                  int NewUOrder,
                                  int NewVOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfDerive( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfDeriveScalar( CagdSrfStruct *Srf,
                                  CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfIntegrate( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfMoebiusTransform( CagdSrfStruct *Srf,
                                      double c,
                                      CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrSrfTangent( CagdSrfStruct *Srf,
                             double u,
                             double v,
                             CagdSrfDirType Dir,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrSrfNormal( CagdSrfStruct *Srf,
                            double u,
                            double v,
                            int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BzrSrfMeshNormals( CagdSrfStruct *Srf,
                                 int UFineNess,
                                 int VFineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrSrf2PolygonsSamples( CagdSrfStruct *Srf,
                           int FineNess,
                           int ComputeNormals,
                           int ComputeUV,
                           double **PtWeights,
                           CagdPtStruct **PtMesh,
                           CagdVecStruct **PtNrml,
                           CagdUVStruct **UVMesh,
                           int *FineNessU,
                           int *FineNessV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrSrf2PolygonsSamplesNuNv( CagdSrfStruct *Srf,
                               int Nu,
                               int Nv,
                               int ComputeNormals,
                               int ComputeUV,
                               double **PtWeights,
                               CagdPtStruct **PtMesh,
                               CagdVecStruct **PtNrml,
                               CagdUVStruct **UVMesh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *BzrSrf2Polygons( CagdSrfStruct *Srf,
                                   int FineNess,
                                   int ComputeNormals,
                                   int FourPerFlat,
                                   int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *BzrSrf2PolygonsN( CagdSrfStruct *Srf,
                                    int Nu,
                                    int Nv,
                                    int ComputeNormals,
                                    int FourPerFlat,
                                    int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvHasBezierKV( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfHasBezierKVs( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotHasBezierKV( double *KnotVector, int Len, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvHasOpenEC( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfHasOpenEC( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfHasOpenECDir( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotHasOpenEC( double *KnotVector, int Len, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotParamInDomain( double *KnotVector,
                               int Len,
                               int Order,
                               int Periodic,
                               double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotLastIndexLE( double *KnotVector, int Len, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotLastIndexL( double *KnotVector, int Len, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotFirstIndexG( double *KnotVector, int Len, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotMultiplicity( double *KnotVector, int Len, int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotUniformPeriodic(int Len, int Order, double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotUniformFloat(int Len, int Order, double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotUniformOpen(int Len, int Order, double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotDiscontUniformOpen(int Len, int Order, double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdEndConditionType BspIsKnotUniform(int Len,
                                      int Order,
                                       double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdEndConditionType BspIsKnotDiscontUniform(int Len,
                                             int Order,
                                              double *KnotVector);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotDegreeRaisedKV( double *KV,
                                 int Len,
                                 int Order,
                                 int NewOrder,
                                 int *NewLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotSubtrTwo( double *KnotVector1,
                           int Len1,
                            double *KnotVector2,
                           int Len2,
                           int *NewLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotMergeTwo( double *KnotVector1,
                           int Len1,
                            double *KnotVector2,
                           int Len2,
                           int Mult,
                           int *NewLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotContinuityMergeTwo( double *KnotVector1,
                                     int Len1,
                                     int Order1,
                                      double *KnotVector2,
                                     int Len2,
                                     int Order2,
                                     int ResOrder,
                                     int *NewLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotDoubleKnots( double *KnotVector, 
                              int *Len,
                              int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotAverage( double *KnotVector, int Len, int Ave);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotNodes( double *KnotVector, int Len, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BspKnotNode( double *KnotVector, int i, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotPeriodicNodes( double *KnotVector,
                                int Len,
                                int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BspCrvMaxCoefParam( CagdCrvStruct *Crv,
                             int Axis,
                             double *MaxVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspSrfMaxCoefParam( CagdSrfStruct *Srf,
                              int Axis,
                              double *MaxVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotPrepEquallySpaced(int n, double Tmin, double Tmax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotReverse( double *KnotVector, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotScale(double *KnotVector, int Len, double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotTranslate(double *KnotVector, int Len, double Trans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAffineTrans(double *KnotVector,
                        int Len,
                        double Translate,
                        double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAffineTrans2(double *KnotVector,
                         int Len,
                         double MinVal,
                         double MaxVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAffineTransOrder(double *KnotVector,
                             int Order,
                             int Len,
                             double Translate,
                             double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAffineTransOrder2(double *KnotVector,
                              int Order,
                              int Len,
                              double MinVal,
                              double MaxVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotCopy(double *DstKV,  double *SrcKV, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern BspKnotAlphaCoeffStruct *BspKnotEvalAlphaCoef(int k,
                                              double *KVT,
                                              int LengthKVT,
                                              double *KVt,
                                              int LengthKVt,
                                              int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern BspKnotAlphaCoeffStruct *BspKnotEvalAlphaCoefMerge(int k,
                                                   double *KVT,
                                                   int LengthKVT,
                                                   double *NewKV,
                                                   int LengthNewKV,
                                                   int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern BspKnotAlphaCoeffStruct *BspKnotCopyAlphaCoef( BspKnotAlphaCoeffStruct
                                                                          *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotFreeAlphaCoef(BspKnotAlphaCoeffStruct *A);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAlphaLoopBlendNotPeriodic( BspKnotAlphaCoeffStruct *A,
                                      int IMin,
                                      int IMax,
                                       double *OrigPts,
                                      double *RefPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAlphaLoopBlendPeriodic( BspKnotAlphaCoeffStruct *A,
                                   int IMin,
                                   int IMax,
                                    double *OrigPts,
                                   int OrigLen,  
                                   double *RefPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotAlphaLoopBlendStep( BspKnotAlphaCoeffStruct *A,
                               int IMin,
                               int IMax,
                                double *OrigPts,
                               int OrigPtsStep,
                               int OrigLen,  
                               double *RefPts,
                               int RefPtsStep);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotInsertOne( double *KnotVector,
                            int Order,
                            int Len,
                            double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotInsertMult( double *KnotVector,
                             int Order,
                             int *Len,
                             double t,
                             int Mult);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotFindMult( double *KnotVector,
                    int Order,
                    int Len,
                    double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotsMultiplicityVector( double *KnotVector,
                               int Len,
                               double *KnotValues,
                               int *KnotMultiplicities,
                               double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotC0Discont( double *KnotVector,
                           int Order,
                           int Length,
                           double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotC1Discont( double *KnotVector,
                           int Order,
                           int Length,
                           double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotC2Discont( double *KnotVector,
                           int Order,
                           int Length,
                           double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotAllC0Discont( double *KnotVector,
                               int Order,
                               int Length,
                               int *n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotAllC1Discont( double *KnotVector,
                               int Order,
                               int Length,
                               int *n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspKnotParamValues(double PMin,
                              double PMax,
                              int NumSamples,
                              double *C1Disconts,
                              int NumC1Disconts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotMakeRobustKV(double *KV, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotVectorsSame( double *KV1,
                              double *KV2,
                             int Len,
                             double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspKnotVerifyPeriodicKV(double *KV, int Order, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspKnotVerifyKVValidity(double *KV, int Order, int Len, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspVecSpreadEqualItems(double *Vec, int Len, double MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspGenBasisFuncsAsCurves(int Order,
                                        int Length,
                                         double *KV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspGenKnotsGeometryAsCurves(int Order,
                                           int Length,
                                            double *KV,
                                           double SizeOfKnot);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvNew(int Length, int Order, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspPeriodicCrvNew(int Length,
                                 int Order,
                                 int Periodic,
                                 CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspCrvDomain( CagdCrvStruct *Crv, double *TMin, double *TMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspCrvCoxDeBoorBasis( double *KnotVector,
                                int Order,
                                int Len,
                                int Periodic,
                                double t,
                                int *IndexFirst);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvCoxDeBoorIndexFirst( double *KnotVector,
                              int Order,
                              int Len,
                              double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspCrvEvalCoxDeBoor( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdBspBasisFuncEvalStruct *BspBasisFuncMultEval( double *KnotVector,
                                                 int KVLength,
                                                 int Order,
                                                 int Periodic,
                                                 double *Params,
                                                 int NumOfParams,
                                                 CagdBspBasisFuncMultEvalType
                                                                     EvalType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspBasisFuncMultEvalPrint( CagdBspBasisFuncEvalStruct *Evals,
                               int Order,
                               double *Params,
                               int NumOfParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspBasisFuncMultEvalFree(CagdBspBasisFuncEvalStruct *Evals,
                              int NumOfEvals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BspCrvEvalVecAtParam( double *Vec,
                               int VecInc,
                                double *KnotVector,
                               int Order,
                               int Len,
                               int Periodic,
                               double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspCrvEvalAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateCircle( CagdPtStruct *Center,
                                  double Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateUnitCircle();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreatePCircle( CagdPtStruct *Center,
                                   double Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateUnitPCircle();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateUnitPCircleQuadTol(double Tol, int Cont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateUnitPCircleCubicTol(double Tol, int Cont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreatePCircleTol( CagdPtStruct *Center,
                                      double Radius,
                                      int Order,
                                      int Cont,
                                      double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateApproxSpiral(double NumOfLoops,
                                        double Pitch,
                                        int Sampling,
                                        int CtlPtsPerLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateApproxHelix(double NumOfLoops,
                                       double Pitch,
                                       double Radius,
                                       int Sampling,
                                       int CtlPtsPerLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvCreateApproxSine(double NumOfCycles,
                                      int Sampling,
                                      int CtlPtsPerCycle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvKnotInsert( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvKnotInsertNSame( CagdCrvStruct *Crv,
                                     double t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvKnotInsertNDiff( CagdCrvStruct *Crv,
                                     int Replace,
                                     double *t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspCrvSubdivCtlPoly( CagdCrvStruct *Crv,
                         double **LPoints,
                         double **RPoints,
                         int LLength,
                         int RLength,
                         double t,
                         int Mult);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvSubdivAtParam( CagdCrvStruct *Crv, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvOpenEnd( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvKnotC0Discont( CagdCrvStruct *Crv, double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvKnotC1Discont( CagdCrvStruct *Crv, double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvKnotC2Discont( CagdCrvStruct *Crv, double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspMeshC1PtsCollinear( IrtPtType* Pt0,
                                 IrtPtType* Pt1,
                                 IrtPtType* Pt2,
                                double *LenRatio,
                                int First);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvMeshC0Continuous( CagdCrvStruct *Crv,
                                 int Idx,
                                 double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvMeshC1Continuous( CagdCrvStruct *Crv,
                                 int Idx,
                                 double *CosAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvDegreeRaise( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvDegreeRaiseN( CagdCrvStruct *Crv, int NewOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvDerive( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvDeriveScalar( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvIntegrate( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvMoebiusTransform( CagdCrvStruct *Crv, double c);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspCrvTangent( CagdCrvStruct *Crv,
                             double t,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspCrvBiNormal( CagdCrvStruct *Crv,
                              double t,
                              int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspCrvNormal( CagdCrvStruct *Crv,
                            double t,
                            int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *BspCrv2Polyline( CagdCrvStruct *Crv,
                                    int SamplesPerCurve,
                                    BspKnotAlphaCoeffStruct *A,
                                    int OptiLin);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvInterpPts( CagdPtStruct *PtList,
                               int Order,
                               int CrvSize,
                               CagdParametrizationType ParamType,
                               int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvInterpPts2( CagdCtlPtStruct *PtList,
                                int Order,
                                int CrvSize,
                                CagdParametrizationType ParamType,
                                int Periodic,
                                int EndPtInterp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvInterpPts3( CagdCtlPtStruct *PtList,
                                 double *Params,
                                 double *KV,
                                int Length,
                                int Order,
                                int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspCrvInterpBuildKVs( CagdCtlPtStruct *PtList,
                          int Order,
                          int CrvSize,
                          CagdParametrizationType ParamType,
                          int Periodic,
                          double **RetPtKnots,
                          double **RetKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvInterpolate( CagdCtlPtStruct *PtList,
                                  double *Params,
                                  double *KV,
                                 int Length,
                                 int Order,
                                 int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvFitLstSqr( CagdCrvStruct *Crv,
                               int Order,
                               int Size,
                               int Periodic,
                               CagdParametrizationType ParamType,
                               int EndPtInterp,
                               int EvalPts,
                               double *Err);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspPtSamplesToKV( double *PtsSamples,
                            int NumPts,
                            int CrvOrder,
                            int CrvLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BspCrvInterpPtsError( CagdCrvStruct *Crv,
                                CagdPtStruct *PtList,
                               CagdParametrizationType ParamType,
                               int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspMakeReparamCurve( CagdPtStruct *PtsList,
                                   int Order,
                                   int DegOfFreedom);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdLineFitToPts(CagdPtStruct *PtList,
                           IrtVecType* LineDir,
                           IrtPtType* LinePos);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdPlaneFitToPts( CagdPtStruct *PtList,
                            IrtPlnType* Pln,
                            IrtVecType* MVec,
                            IrtPtType* Cntr,
                            double *CN);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdPlaneFitToPts2(double *  *Points,
                             int NumPts,
                             CagdPointType PType,
                             IrtPlnType* Pln,
                             IrtVecType* MVec,
                             IrtPtType* Cntr,
                             double *CN);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdPlaneFitToPts3(IrtPtType*   Points,
                             int NumPts,
                             IrtPlnType* Pln,
                             IrtVecType* MVec,
                             IrtPtType* Cntr,
                             double *CN);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspReparameterizeCrv(CagdCrvStruct *Crv,
                          CagdParametrizationType ParamType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvExtensionOneSide( CagdCrvStruct *OrigCrv,
                                      int MinDmn,
                                      double Epsilon,
                                      int RemoveExtraKnots);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvExtension( CagdCrvStruct *OrigCrv,
                                int *ExtDirs,
                               double Epsilon,
                               int RemoveExtraKnots);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvExtraKnotRmv( CagdCrvStruct *Crv, int RmvIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfNew(int ULength,
                         int VLength,
                         int UOrder,
                         int VOrder,
                         CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspPeriodicSrfNew(int ULength,
                                 int VLength,
                                 int UOrder,
                                 int VOrder,
                                 int UPeriodic,
                                 int VPeriodic,
                                 CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspSrfDomain( CagdSrfStruct *Srf,
                  double *UMin,
                  double *UMax,
                  double *VMin,
                  double *VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspSrfEvalAtParam( CagdSrfStruct *Srf,
                             double u,
                             double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *BspSrfEvalAtParam2( CagdSrfStruct *Srf,
                              double u,
                              double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspSrfCrvFromSrf( CagdSrfStruct *Srf,
                                double t,
                                CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspSrfC1DiscontCrvs( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfHasC1Discont( CagdSrfStruct *Srf, int E3C1Discont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfIsC1DiscontAt( CagdSrfStruct *Srf,
                              CagdSrfDirType Dir,
                              double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspSrfCrvFromMesh( CagdSrfStruct *Srf,
                                 int Index,
                                 CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfKnotInsert( CagdSrfStruct *Srf,
                                CagdSrfDirType Dir,
                                double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfKnotInsertNSame( CagdSrfStruct *Srf,
                                     CagdSrfDirType Dir,
                                     double t, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfKnotInsertNDiff( CagdSrfStruct *Srf,
                                     CagdSrfDirType Dir,
                                     int Replace,
                                     double *t,
                                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfSubdivAtParam( CagdSrfStruct *Srf,
                                   double t,
                                   CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfOpenEnd( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfKnotC0Discont( CagdSrfStruct *Srf,
                              CagdSrfDirType Dir,
                              double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfKnotC1Discont( CagdSrfStruct *Srf,
                              CagdSrfDirType Dir,
                              double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfMeshC1Continuous( CagdSrfStruct *Srf,
                                 CagdSrfDirType Dir,
                                 int Idx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfDegreeRaise( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfDegreeRaiseN( CagdSrfStruct *Srf,
                                  int NewUOrder,
                                  int NewVOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfDerive( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfDeriveScalar( CagdSrfStruct *Srf,
                                  CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfIntegrate( CagdSrfStruct *Srf, CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfMoebiusTransform( CagdSrfStruct *Srf,
                                      double c,
                                      CagdSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspSrfTangent( CagdSrfStruct *Srf,
                             double u,
                             double v,
                             CagdSrfDirType Dir,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspSrfNormal( CagdSrfStruct *Srf,
                            double u,
                            double v,
                            int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *BspSrfMeshNormals( CagdSrfStruct *Srf,
                                 int UFineNess,
                                 int VFineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfErrorFuncType BspSrf2PolygonSetErrFunc(CagdSrfErrorFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *BspSrf2Polygons( CagdSrfStruct *Srf,
                                   int FineNess,
                                   int ComputeNormals,
                                   int FourPerFlat,
                                   int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *BspSrf2PolygonsN( CagdSrfStruct *Srf,
                                    int Nu,
                                    int Nv,
                                    int ComputeNormals,
                                    int FourPerFlat,
                                    int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrf2PolygonsSamplesNuNv( CagdSrfStruct *Srf,
                               int Nu,
                               int Nv,
                               int ComputeNormals,
                               int ComputeUV,
                               double **PtWeights,
                               CagdPtStruct **PtMesh,
                               CagdVecStruct **PtNrml,
                               CagdUVStruct **UVMesh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspC1Srf2PolygonsSamples( CagdSrfStruct *Srf,
                             int FineNess,
                             int ComputeNormals,
                             int ComputeUV,
                             double **PtWeights,
                             CagdPtStruct **PtMesh,
                             CagdVecStruct **PtNrml,
                             CagdUVStruct **UVMesh,
                             int *FineNessU,
                             int *FineNessV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfInterpPts( CagdPtStruct **PtList,
                               int UOrder,
                               int VOrder,
                               int SrfUSize,
                               int SrfVSize,
                               CagdParametrizationType ParamType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfInterpolate( CagdCtlPtStruct *PtList,
                                 int NumUPts,
                                 int NumVPts,
                                  double *UParams,
                                  double *VParams,
                                  double *UKV,
                                  double *VKV,
                                 int ULength,
                                 int VLength,
                                 int UOrder,
                                 int VOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfFitLstSqr( CagdSrfStruct *Srf,
                               int UOrder,
                               int VOrder,
                               int USize,
                               int VSize, 
                               CagdParametrizationType ParamType,
                               double *Err);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfInterpScatPts( CagdCtlPtStruct *PtList,
                                   int UOrder,
                                   int VOrder,
                                   int USize,
                                   int VSize,
                                   double *UKV,
                                   double *VKV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfInterpScatPtsC0Bndry( CagdCtlPtStruct *PtList,
                                           CagdCrvStruct *UMinCrv,
                                           CagdCrvStruct *UMaxCrv,
                                           CagdCrvStruct *VMinCrv,
                                           CagdCrvStruct *VMaxCrv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfInterpScatPts2( CagdCtlPtStruct *PtList,
                                    int UOrder,
                                    int VOrder,
                                    int USize,
                                    int VSize,
                                    double *UKV,
                                    double *VKV,
                                    double *MatrixCondition);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BspReparameterizeSrf(CagdSrfStruct *Srf,
                          CagdSrfDirType Dir,
                          CagdParametrizationType ParamType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfExtension( CagdSrfStruct *OrigSrf,
                                int *ExtDirs,
                               double EpsilonU,
                               double EpsilonV,
                               int RemoveExtraKnots);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdPrimRectangleCrv(double MinX,
                                    double MinY,
                                    double MaxX,
                                    double MaxY,
                                    double ZLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneSrf(double MinX,
                                double MinY,
                                double MaxX,
                                double MaxY,
                                double ZLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneSrf2(IrtPtType* Cntr,
                                 IrtVecType* Vec1,
                                 IrtVecType* Vec2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneFromE3Crv( CagdCrvStruct *Crv,
                                       IrtPlnType* Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneSrfOrderLen(double MinX,
                                        double MinY,
                                        double MaxX,
                                        double MaxY,
                                        double ZLevel,
                                        int Order,
                                        int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneXZSrf(double MinX,
                                  double MinZ,
                                  double MaxX,
                                  double MaxZ,
                                  double YLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlaneYZSrf(double MinY,
                                  double MinZ,
                                  double MaxY,
                                  double MaxZ,
                                  double XLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimPlanePlaneSpanBBox( GMBBBboxStruct *BBox,
                                          IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimBoxSrf(double MinX,
                              double MinY,
                              double MinZ,
                              double MaxX,
                              double MaxY,
                              double MaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimSphereSrf( IrtVecType* Center,
                                 double Radius,
                                 int Rational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimCubeSphereSrf( IrtVecType* Center,
                                     double Radius,
                                     int Rational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimTorusSrf( IrtVecType* Center,
                                double MajorRadius,
                                double MinorRadius,
                                int Rational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimCone2Srf( IrtVecType* Center,
                                double MajorRadius,
                                double MinorRadius,
                                double Height,
                                int Rational,
                                CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimConeSrf( IrtVecType* Center,
                               double Radius,
                               double Height,
                               int Rational,
                               CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdPrimCylinderSrf( IrtVecType* Center,
                                   double Radius,
                                   double Height,
                                   int Rational,
                                   CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtPwr2BzrCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtBzr2PwrCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtBsp2BzrCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtBzr2BspCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtPwr2BzrSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtBzr2PwrSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtBzr2BspSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtBsp2BzrSrf( CagdSrfStruct *CSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtPeriodic2FloatCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtFloat2OpenCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtBsp2OpenCrv( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtPeriodic2FloatSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtFloat2OpenSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdCnvrtBsp2OpenSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCtlPtStruct *CagdCnvrtCrvToCtlPts( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdCnvrtPtList2Polyline( CagdPtStruct *Pts,
                                             CagdPolylineStruct **Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPtStruct *CagdCnvrtPolyline2PtList( CagdPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCnvrtPolyline2LinBspCrv( CagdPolylineStruct *Poly,
                                           CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *CagdCnvrtLinBspCrv2Polyline( CagdCrvStruct *Crv,
                                                int FilterIdentical);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdRayTraceBzrSrf(IrtPtType* StPt,
                             IrtVecType* Dir,
                              CagdSrfStruct *BzrSrf,
                             CagdUVStruct **IntrPrm,
                             CagdPtStruct **IntrPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdBsplineCrvFittingWithInitCrv(
                        IrtPtType* PtList,                   
                        int NumOfPoints,
                        CagdCrvStruct *InitCrv,
                        CagdBspFittingType AgorithmType,
                        int MaxIter,
                        double ErrorLimit,
                        double ErrorChangeLimit,
                        double Lambda);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdBsplineCrvFitting(IrtPtType* PtList,              
                                     int NumOfPoints,
                                     int Length, 
                                     int Order,
                                     int IsPeriodic,
                                     CagdBspFittingType AgorithmType,
                                     int MaxIter,
                                     double ErrorLimit,
                                     double ErrorChangeLimit,
                                     double Lambda);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdQuadCurveWeightedQuadrangulation(
                                           CagdCrvStruct *Crv,
                                         CagdQuadSrfWeightFuncType WeightFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdQuadCurveListWeightedQuadrangulation(
                                         CagdCrvStruct *CrvList,
                                        CagdQuadSrfWeightFuncType WeightFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdQuadSrfJacobianWeight( CagdSrfStruct *QuadSrf, 
                                    CagdCrvStruct *BoundaryCrv,
                                    CagdPolylineStruct *SampledPolygon, 
                                    int *VIndices, int numV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdQuadSrfConformWeight( CagdSrfStruct *QuadSrf, 
                                   CagdCrvStruct *BoundaryCrv,
                                   CagdPolylineStruct *SampledPolygon, 
                                   int *VIndices,
                                  int numV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdQuadSrfRegularPolyWeight( CagdSrfStruct *QuadSrf, 
                                       CagdCrvStruct *BoundaryCrv,
                                       CagdPolylineStruct *SampledPolygon,
                                       int *VIndices,
                                      int numV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CagdQuadSrfCombinedWeight( CagdSrfStruct *QuadSrf, 
                                    CagdCrvStruct *BoundaryCrv,
                                    CagdPolylineStruct *SampledPolygon, 
                                    int *VIndices,
                                   int numV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdQuadSetQuadSrfCombinedWeightFactors(double JacobianFactor,
                                             double ConformFactor,
                                             double RegularityFactor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdQuadGetQuadSrfCombinedWeightFactors(double *JacobianFactor,
                                             double *ConformFactor,
                                             double *RegularityFactor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvQuadTileStruct *CagdQuadCrvToQuadsLineSweep( CagdCrvStruct *Crv,
                                                   int OutputSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvQuadTileStruct *CagdCrvQuadTileCopy( CagdCrvQuadTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvQuadTileStruct *CagdCrvQuadTileCopyList(
                                         CagdCrvQuadTileStruct *Tiles);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvQuadTileFree(CagdCrvQuadTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdCrvQuadTileFreeList(CagdCrvQuadTileStruct *Tiles);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvQuadTileStruct *CagdCrvQuadTileAssumeSrf(CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvQuadTileStruct *CagdCrvQuadTileFromSrf( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSetErrorFuncType CagdSetFatalErrorFunc(CagdSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CagdFatalError(CagdFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *CagdDescribeError(CagdFatalErrorType ErrID);
    }
}
