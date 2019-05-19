using System;
using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void MdlIntersectionCBFunc (
                                                     CagdSrfStruct *Srf1,
                                                    CagdSrfStruct *Srf2, 
                                                   CagdSrfStruct **ModifiedSrf1, 
                                                   CagdSrfStruct **ModifiedSrf2,
                                                   void** InterCBData);
    public unsafe delegate void MdlPreIntersectionCBFunc ( MdlModelStruct *Mdl1,
                                                  MdlModelStruct *Mdl2,
                                                 void** InterCBData);
    public unsafe delegate void MdlPostIntersectionCBFunc (
                                                 MdlModelStruct *Mdl,
                                                void** InterCBData);
    public unsafe delegate void MdlSetErrorFuncType (MdlFatalErrorType ErrorFunc);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSegFree(MdlTrimSegStruct *MTSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSegFreeList(MdlTrimSegStruct *MTSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSegRefFree(MdlTrimSegRefStruct *MTSegRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSegRefFreeList(MdlTrimSegRefStruct *MTSegRefList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlLoopFree(MdlLoopStruct *MdlLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlLoopFreeList(MdlLoopStruct *MdlLoopList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSrfFree(MdlTrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlTrimSrfFreeList(MdlTrimSrfStruct *MdlTrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelFree(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelFreeList(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlTrimSegCopy( MdlTrimSegStruct *MdlTrimSeg,
                                  MdlTrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlTrimSegCopyList( MdlTrimSegStruct *MdlTrimSegList,
                                      MdlTrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlTrimSegRefCopy( MdlTrimSegRefStruct *SegRefList,
                                        MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlTrimSegRefCopyList( MdlTrimSegRefStruct *SegRefList,
                                            MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlLoopStruct *MdlLoopNew(MdlTrimSegRefStruct *MdlTrimSegRefList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlLoopStruct *MdlLoopCopy( MdlLoopStruct *MdlLoop, 
                            MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlLoopStruct *MdlLoopCopyList( MdlLoopStruct *MdlLoopList, 
                                MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSrfStruct *MdlTrimSrfNew(CagdSrfStruct *Srf,
                                MdlLoopStruct *LoopList, 
                                int HasTopLvlTrim,
                                int UpdateBackTSrfPtrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSrfStruct *MdlTrimSrfNew2(CagdSrfStruct *Srf,
                                  CagdCrvStruct **LoopList,
                                 int NumLoops,
                                 int HasTopLvlTrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSrfStruct *MdlTrimSrfCopy( MdlTrimSrfStruct *MdlTrimSrf, 
                                  MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSrfStruct *MdlTrimSrfCopyList( MdlTrimSrfStruct *MdlTrimSrfList, 
                                      MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlTrimSegNew(CagdCrvStruct *UVCrv1,
                                CagdCrvStruct *UVCrv2,
                                CagdCrvStruct *EucCrv1,
                                MdlTrimSrfStruct *SrfFirst,
                                MdlTrimSrfStruct *SrfSecond);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlTrimSegRemove( MdlTrimSegStruct *TSeg, MdlTrimSegStruct **SegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlTrimSegRefNew(MdlTrimSegStruct *MdlTrimSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlTrimSegRefRemove2( MdlTrimSegStruct *TSeg,
                         MdlLoopStruct *Loops,
                         int FreeRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlTrimSegRefRemove( MdlTrimSegStruct *TSeg,
                        MdlTrimSegRefStruct **TSegRefList,
                        int FreeRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlModelNew(CagdSrfStruct *Srf,
                            CagdCrvStruct **LoopList,
                            int NumLoops,
                            int HasTopLvlTrim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlModelNew2(MdlTrimSrfStruct *TrimSrfs,
                             MdlTrimSegStruct *TrimSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlModelCopy( MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlModelCopyList( MdlModelStruct *ModelList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelTransform(MdlModelStruct *Model,
                        double *Translate,
                       double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelMatTransform(MdlModelStruct *Model, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlModelsSame( MdlModelStruct *Model1,
                         MdlModelStruct *Model2,
                        double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlTrimSrfChainTrimSegs(MdlTrimSrfStruct *TSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlGetOtherSegRef( MdlTrimSegRefStruct *SegRef,
                                        MdlTrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlGetOtherSegRef2( MdlTrimSegRefStruct *SegRef,
                                         MdlTrimSrfStruct *TSrf,
                                        MdlTrimSrfStruct **OtherTSrf,
                                        MdlLoopStruct **OtherLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlPatchTrimmingSegPointers(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MdlGetLoopSegIndex( MdlTrimSegRefStruct *TrimSeg,
                                       MdlTrimSegStruct *TrimSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MdlGetSrfIndex( MdlTrimSrfStruct *Srf,
                                   MdlTrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelBBox( MdlModelStruct *Mdl, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelListBBox( MdlModelStruct *Mdls, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlModelTSrfTCrvsBBox( MdlTrimSrfStruct *TSrf, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlTwoTrimSegsSameEndPts( MdlTrimSegStruct *TSeg1,
                              MdlTrimSegStruct *TSeg2,
                             double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegRefStruct *MdlGetSrfTrimSegRef( MdlTrimSrfStruct *TSrf,
                                          MdlTrimSegStruct *TSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlGetModelTrimSegRef( MdlModelStruct *Mdl,
                           MdlTrimSegStruct *TSeg,
                          MdlTrimSegRefStruct **TSegRef1,
                          MdlTrimSrfStruct **TSrf1,
                          MdlTrimSegRefStruct **TSegRef2,
                          MdlTrimSrfStruct **TSrf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MdlGetTrimmingCurves( MdlTrimSrfStruct *TrimSrf,
                                    int ParamSpace,
                                    int EvalEuclid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlStitchModel(MdlModelStruct *Mdl, double StitchTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimPlane(double MinX,
                             double MinY,
                             double MaxX,
                             double MaxY,
                             double ZLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimPlaneSrfOrderLen(double MinX,
                                        double MinY,
                                        double MaxX,
                                        double MaxY,
                                        double ZLevel,
                                        int Order,
                                        int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimListOfSrfs2Model(CagdSrfStruct *Srfs, int *n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimBox(double MinX,
                           double MinY,
                           double MinZ,
                           double MaxX,
                           double MaxY,
                           double MaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimSphere( IrtVecType* Center,
                              double Radius,
                              int Rational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimTorus( IrtVecType* Center,
                             double MajorRadius,
                             double MinorRadius,
                             int Rational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimCone2( IrtVecType* Center,
                             double MajorRadius,
                             double MinorRadius,
                             double Height,
                             int Rational,
                             CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimCone( IrtVecType* Center,
                            double Radius,
                            double Height,
                            int Rational,
                            CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlPrimCylinder( IrtVecType* Center,
                                double Radius,
                                double Height,
                                int Rational,
                                CagdPrimCapsType Caps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlStitchSelfSrfPrims(int Stitch);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlCreateCubeSpherePrim(int CubeTopoSphere);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MdlExtructReversUVCrv( MdlTrimSrfStruct *MdlSrf, 
                                      MdlTrimSegStruct *MdlSeg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlBooleanSetTolerances(double SubdivTol,
                             double NumerTol,
                             double TraceTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlBooleanUnion(
                                  MdlModelStruct *Model1, 
                                  MdlModelStruct *Model2,
                                  MvarSrfSrfInterCacheStruct *SSICache, 
                                 MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlBooleanIntersection(
                                  MdlModelStruct *Model1, 
                                  MdlModelStruct *Model2,
                                  MvarSrfSrfInterCacheStruct *SSICache,
                                 MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlBooleanSubtraction(
                                  MdlModelStruct *Model1, 
                                  MdlModelStruct *Model2,
                                  MvarSrfSrfInterCacheStruct *SSICache,
                                 MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlBooleanCut(
                                  MdlModelStruct *Model1,
                                  MdlModelStruct *Model2,
                                  MvarSrfSrfInterCacheStruct *SSICache,
                                 MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlBooleanMerge( MdlModelStruct *Model1,
                                        MdlModelStruct *Model2,
                                       int StitchBndries);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlBooleanMerge2( MdlModelStruct *Model1,
                                  MdlModelStruct *Model2,
                                 int StitchBndries);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MdlBooleanInterCrv( MdlModelStruct *Model1,
                                   MdlModelStruct *Model2,
                                  int InterType,
                                  MdlModelStruct **InterModel,
                                  MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlBoolSetOutputInterCrv(int OutputInterCurve);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlBoolSetOutputInterCrvType(int OutputInterCurveType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlModelNegate( MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlBoolCleanRefsToTSrf(MdlModelStruct *Model, MdlTrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlBoolResetAllTags(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlBoolCleanUnusedTrimCrvsSrfs(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlBoolClipTSrfs2TrimDomain(MdlModelStruct *Model, 
                                 int ExtendSrfDomain);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MdlInterSrfByPlane( CagdSrfStruct *Trf,
                                   IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *MdlClipSrfByPlane( CagdSrfStruct *Srf,
                                  IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *MdlClipTrimmedSrfByPlane( TrimSrfStruct *TSrf,
                                         IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlClipModelByPlane( MdlModelStruct *Mdl,
                                     IrtPlnType* Pln,
                                    MdlBooleanType BoolOp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  MdlBopsParams *MdlBoolOpParamsAlloc(
                                    int PertubrateSecondModel,
                                    int ExtendUVSrfResult,
                                    MdlIntersectionCBFunc InterCBFunc,
                                    MdlPreIntersectionCBFunc PreInterCBFunc,
                                    MdlPostIntersectionCBFunc PostInterCBFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlBoolOpParamsFree( MdlBopsParams *BopsParams);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlSplitTrimCrv(MdlTrimSegStruct *Seg,
                     CagdPtStruct *Pts,
                    int Idx,
                    double Eps,
                    int *Proximity);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlDivideTrimCrv(MdlTrimSegStruct *Seg,
                                    CagdPtStruct *Pts,
                                   int Idx,
                                   double Eps,
                                   int *Proximity);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlTrimSegStruct *MdlFilterOutCrvs(MdlTrimSegStruct *TSegs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlIsPointInsideTrimSrf( MdlTrimSrfStruct *TSrf,
                                  IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlIsPointInsideTrimLoop( MdlTrimSrfStruct *TSrf,
                              MdlLoopStruct *Loop,
                             IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlIsLoopNested( MdlLoopStruct *L,  MdlTrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlGetUVLocationInLoop( MdlLoopStruct *L,
                            MdlTrimSrfStruct *TSrf,
                           IrtUVType UV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlEnsureMdlTrimCrvsPrecision(MdlModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlEnsureTSrfTrimCrvsPrecision(MdlTrimSrfStruct *MdlTrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlEnsureTSrfTrimLoopPrecision(MdlLoopStruct *Loop,
                                   MdlTrimSrfStruct *MdlTrimSrf,
                                   double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlModelIsClosed( MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtPtType* MdlGetTrimmingCurvesEndPts(MdlModelStruct *Mdl, int *N);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *MdlCnvrtMdl2TrimmedSrfs( MdlModelStruct *Model,
                                       double TrimCrvStitchTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *MdlCnvrtMdls2TrimmedSrfs( MdlModelStruct *Models,
                                        double TrimCrvStitchTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlCnvrtSrf2Mdl( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlCnvrtTrimmedSrf2Mdl( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlAddSrf2Mdl( MdlModelStruct *Mdl,
                               CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlAddTrimmedSrf2Mdl( MdlModelStruct *Mdl,
                                      TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *MdlExtractUVCrv( MdlTrimSrfStruct *Srf,
                                MdlTrimSegStruct *Seg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlSplitDisjointComponents( MdlModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlSetErrorFuncType MdlSetFatalErrorFunc(MdlSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlFatalError(MdlFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *MdlDescribeError(MdlFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlDbg(void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlDbg2(void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlDbgDsp(void *Obj, double Trans, int Clear);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlDbgDsp2(void *Obj, double Trans, int Clear);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDbgMC( MdlModelStruct *Mdl, int Format);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDbgTC( MdlTrimSegStruct *TSegs, 
              MdlTrimSrfStruct *TSrf,
             int Format);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDbgSC( MdlTrimSrfStruct *TSrf, int Format);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDbgRC( MdlTrimSegRefStruct *Refs, int Format);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDbgRC2( MdlTrimSegRefStruct *Refs,
               MdlTrimSrfStruct *TSrf,
              int Format);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugHandleTCrvLoops( MdlTrimSrfStruct *TSrf,
                             MdlLoopStruct *Loops,
                             IrtPtType* Trans,
                            int Display,
                            int TrimEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugHandleTSrfCrvs( MdlTrimSegStruct *TCrvs,
                            MdlTrimSrfStruct *TSrf,
                            IrtPtType* Trans,
                           int Display,
                           int TrimEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugHandleTSrfRefCrvs( MdlTrimSegRefStruct *Refs,
                               MdlTrimSrfStruct *TSrf,
                               IrtPtType* Trans,
                              int Loop,
                              int Display,
                              int TrimEndPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugWriteTrimSegs( MdlTrimSegStruct *TSegs,
                           MdlTrimSrfStruct *TSrf,
                           IrtPtType* Trans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugVerifyTrimSeg( MdlTrimSegStruct *TSeg, int VerifyBackPtrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlDebugVerify( MdlModelStruct *Model, int Complete, int TestLoops);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *MdlDebugVisual( MdlModelStruct *Model,
                                        int TCrvs,
                                      int TSrfs,
                                      int Srfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MdlDbgVsl( MdlModelStruct *Model,
               int TCrvs,
               int TSrfs,
               int Srfs);
    }
}
