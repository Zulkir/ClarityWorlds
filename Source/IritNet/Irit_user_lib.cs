using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void UserMicroFunctionalTileCBFuncType (
                                                            UserMicroTilingStruct* T,
                                                             int* TileIndex,
                                                             int* CPGlobalIndex,
                                                             int* CPIndexInTile);
    public unsafe delegate void UserMicroPreProcessTileCBFuncType 
                           (IPObjectStruct *Tile, UserMicroPreProcessTileCBStruct *d);
    public unsafe delegate void UserMicroPostProcessTileCBFuncType 
                          (IPObjectStruct *Tile, UserMicroPostProcessTileCBStruct *d);
    public unsafe delegate void UserSetErrorFuncType (UserFatalErrorType ErrorFunc);
    public unsafe delegate void UserRegisterTestConverganceFuncType (double CrntDist, int i);
    public unsafe delegate void UserCntrIsValidCntrPtFuncType ( CagdSrfStruct *Srf,
                                                     double U,
                                                     double V);
    public unsafe delegate void UserHCEditDrawCtlPtFuncType (int PtIndex,
                                                    int PtUniqueID,
                                                    double *Pos,
                                                    double *TanBack,
                                                    double *TanForward);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroTileNew(IPObjectStruct *Geom);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserMicroTileFree(UserMicroTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserMicroTileFreeList(UserMicroTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroTileCopy( UserMicroTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroTileCopyList( UserMicroTileStruct *Tile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroParseTileFromObj(IPObjectStruct *IPObject);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroTileTransform( UserMicroTileStruct *Tile,
                                            IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTileStruct *UserMicroReadTileFromFile( byte *FileName,
                                               int Messages,
                                               int MoreMessages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *UserMicroStructComposition( UserMicroParamStruct *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTilingStruct* UserMicroFunctionalTiling(
                                int Dim, 
                                 int *NumCells,
                                 int *Orders, 
                                 int *NumCPInTile,
                                double MinCPValue, 
                                double MaxCPValue, 
                                double Capping,
                                int ShellBits,
                                int IsC1,
                                UserMicroFunctionalTileCBFuncType CBValueFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserMicroTilingStruct* UserMicroFunctionalRandomTiling(
                                                    int Dim, 
                                                     int *NumCells,
                                                     int *Orders, 
                                                     int *NumCPInTile,
                                                    double MinVal, 
                                                    double MaxVal, 
                                                    double Capping,
                                                    int ShellBits,
                                                    int IsC1);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserMicroFunctionalFreeTiling(UserMicroTilingStruct* Tiling);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserMicroFunctionalEvaluateEucl(UserMicroTilingStruct* Tiling, 
                                           MvarMVStruct *DeformMV, 
                                           double *EuclideanPnt,
                                          double *ResValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserMicroFunctionalEvaluateUV(UserMicroTilingStruct* Tiling,
                                         MvarMVStruct *DeformMV, 
                                         double *UVPnt,
                                        double *ResValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *UserMicroFunctionalTilingIsoSurface(
                                              UserMicroTilingStruct* Tiling,
                                              int SamplingFactor,
                                               MvarMVStruct *DeformMV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserMicroFunctionalTilingVolume(UserMicroTilingStruct* Tiling, 
                                          double CubeSize,
                                          int PositiveVol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IntrSrfHierarchyPreprocessSrf( CagdSrfStruct *Srf,
                                      double FineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IntrSrfHierarchyFreePreprocess(void * Handle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IntrSrfHierarchyTestRay(void * Handle,
                                  IrtPtType* RayOrigin,
                                  IrtVecType* RayDir,
                                  IrtUVType InterUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IntrSrfHierarchyTestPt(void * Handle,
                                 IrtPtType* Pt,
                                 int Nearest,
                                 IrtUVType InterUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCntrSrfWithPlane( CagdSrfStruct *Srf,
                                              IrtPlnType* Plane,
                                             double FineNess,
                                             int UseSSI,
                                             int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCntrEvalToE3(
                               CagdSrfStruct *Srf,
                               IPPolygonStruct *Cntrs,
                              UserCntrIsValidCntrPtFuncType ValidCntrPtFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfDirType UserInterSrfAtAllKnots(CagdSrfStruct *Srfs,
                                      IrtPlnType* Pln,
                                      int Axis,
                                       double *KV,
                                      int MinKV,
                                      int MaxKV,
                                      double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideSrfAtInterCrvs( CagdSrfStruct *Srf,
                                         CagdCrvStruct *ICrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideOneSrfAtAllTVInteriorKnot(CagdSrfStruct *Srf,
                                                    TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideSrfsAtAllTVInteriorKnot(CagdSrfStruct *Srfs,
                                                  TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideOneSrfAtAllMVInteriorKnot(CagdSrfStruct *Srf,
                                                    MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideSrfsAtAllMVInteriorKnot(CagdSrfStruct *Srfs,
                                                  MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideTSrfAtAllKnots(TrimSrfStruct *TSrf,
                                        IrtPlnType* Pln,
                                        int Axis,
                                         double *KV,
                                        int MinKV,
                                        int MaxKV,
                                        double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideOneTSrfAtAllMVInteriorKnot( TrimSrfStruct *TSrf,
                                                     MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *UserDivideTSrfsAtAllMVInteriorKnot( TrimSrfStruct *TSrfs,
                                                   MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  MdlModelStruct *UserDivideMdlAtAllKnots( MdlModelStruct *Model,
                                        IrtPlnType* Pln,
                                        int Axis,
                                         double *KV,
                                        int MinKV,
                                        int MaxKV,
                                        double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  MdlModelStruct *UserDivideOneMdlAtAllMVInteriorKnot(
                                              MdlModelStruct *Model,
                                             MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  MdlModelStruct *UserDivideMdlsAtAllMVInteriorKnot(
                                              MdlModelStruct *Models,
                                             MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlVModelStruct *UserDivideVMdlAtAllKnots(
                                                VMdlVModelStruct *VModel,
                                               IrtPlnType* Pln,
                                               int Axis,
                                                double *KV,
                                               int MinKV,
                                               int MaxKV,
                                               double *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlVModelStruct *UserDivideOneVMdlAtAllMVInteriorKnot(
                                          VMdlVModelStruct *VModel,
                                         MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlVModelStruct *UserDivideVMdlsAtAllMVInteriorKnot(
                                          VMdlVModelStruct *VModels,
                                         MvarMVStruct *MV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserPolyline2LinBsplineCrv(  IPPolygonStruct *Poly,
                                          int FilterDups);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserPolylines2LinBsplineCrvs(
                                           IPPolygonStruct *Polys,
                                         int FilterDups);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCnvrtCagdPolyline2IritPolyline(
                                               CagdPolylineStruct *Poly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCnvrtCagdPolylines2IritPolylines(
                                              CagdPolylineStruct *Polys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *UserCnvrtIritPolyline2CagdPolyline(
                                            IPPolygonStruct *Plln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCnvrtLinBspCrv2IritPolyline(
                                                    CagdCrvStruct *Crv,
                                                   int FilterIdentical);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserCnvrtLinBspCrvs2IritPolylines(
                                                    CagdCrvStruct *Crvs,
                                                   int FilterIdentical);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserCnvrtObjApproxLowOrderBzr(IPObjectStruct *Obj, int ApproxLowOrder);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSrfVisibConeDecomp( CagdSrfStruct *Srf,
                                       double Resolution,
                                       double ConeSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimSrfStruct *UserVisibilityClassify(  IPObjectStruct *SclrSrf,
                                       TrimSrfStruct *TrimmedSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserViewingConeSrfDomains(
                                         CagdSrfStruct *Srf,
                                         CagdSrfStruct *NSrf,
                                          IPPolygonStruct *ConeDirs,
                                        double SubdivTol,
                                        double ConeSize,
                                        double Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserSrfTopoAspectGraph(CagdSrfStruct *PSrf,
                                               double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserMarchOnSurface(UserSrfMarchType MarchType,
                                            IrtUVType UVOrig,
                                            IrtVecType* DirOrig,
                                            CagdSrfStruct *Srf,
                                            CagdSrfStruct *NSrf,
                                            CagdSrfStruct *DuSrf,
                                            CagdSrfStruct *DvSrf,
                                           double Length,
                                           double FineNess,
                                           int ClosedInU,
                                           int ClosedInV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserMarchOnPolygons(
                                          IPObjectStruct *PObj,
                                        UserSrfMarchType MarchType,
                                          IPPolygonStruct *PlHead,
                                         IPVertexStruct *VHead,
                                        double Length);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserCrvViewMap( CagdCrvStruct *Crv,
                                       CagdCrvStruct *ViewCrv,
                                      double SubTol,
                                      double NumTol,
                                      int TrimInvisible);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserCrvAngleMap( CagdCrvStruct *Crv,
                                       double SubdivTol,
                                       double Angle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserCrvOMDiagExtreme( CagdCrvStruct *Crv,
                                              IPObjectStruct *OM,
                                            int DiagExtRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserCrvVisibleRegions( CagdCrvStruct *Crv,
                                      double *View,
                                     double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrimSrfStruct *UserMoldReliefAngle2Srf( CagdSrfStruct *Srf,
                                        IrtVecType* VDir,
                                       double Theta,
                                       int MoreThanTheta,
                                       double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *UserMoldRuledRelief2Srf( CagdSrfStruct *Srf,
                                        IrtVecType* VDir,
                                       double Theta,
                                       double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserMinDistLineBBox( IrtPtType* LinePos,
                              IrtVecType* LineDir,
                             IrtBboxType BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserMinDistLinePolygonList( IrtPtType* LinePos,
                                     IrtVecType* LineDir,
                                     IPPolygonStruct *Pls,
                                     IPPolygonStruct **MinPl,
                                    IrtPtType* MinPt,
                                    double *HitDepth,
                                    double *IndexFrac);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserMinDistLinePolylineList( IrtPtType* LinePos,
                                      IrtVecType* LineDir,
                                      IPPolygonStruct *Pls,
                                     int PolyClosed,
                                      IPPolygonStruct **MinPl,
                                     IrtPtType* MinPt,
                                     double *HitDepth,
                                     double *IndexFrac);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserMinDistPointPolylineList( IrtPtType* Pt,
                                       IPPolygonStruct *Pls,
                                       IPPolygonStruct **MinPl,
                                       IPVertexStruct **MinV,
                                      int *Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserSrfSrfInter( CagdSrfStruct *Srf1,
                     CagdSrfStruct *Srf2,
                    int Euclidean,
                    double Eps,
                    int AlignSrfs,
                    CagdCrvStruct **Crvs1,
                    CagdCrvStruct **Crvs2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserTwoObjMaxZRelMotion( IPObjectStruct *PObj1,
                                  IPObjectStruct *PObj2,
                                 double FineNess,
                                 int NumIters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserMake3DStatueFrom2Images(
                                             byte *Image1Name,
                                             byte *Image2Name,
                                            int DoTexture,
                                              IPObjectStruct *Blob,
                                            User3DSpreadType BlobSpreadMethod,
                                            UserImgShd3dBlobColorType
                                                             BlobColorMethod,
                                            int Resolution,
                                            int Negative,
                                            double Intensity,
                                            double MinIntensity,
                                            int MergePolys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserMake3DStatueFrom3Images(
                                             byte *Image1Name,
                                             byte *Image2Name,
                                             byte *Image3Name,
                                            int DoTexture,
                                              IPObjectStruct *Blob,
                                            User3DSpreadType BlobSpreadMethod,
                                            UserImgShd3dBlobColorType
                                                             BlobColorMethod,
                                            int Resolution,
                                            int Negative,
                                            double Intensity,
                                            double MinIntensity,
                                            int MergePolys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *User3DMicroBlobsFrom3Images(
                                             byte *Image1Name,
                                             byte *Image2Name,
                                             byte *Image3Name,
                                            User3DSpreadType BlobSpreadMethod,
                                            double Intensity,
                                             IrtVecType* MicroBlobSpacing,
                                             IrtVecType* RandomFactors,
                                            int Resolution,
                                            int Negative,
                                            double CubeSize,
                                            int MergePts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *User3DMicroBlobsTiling(
                                        double XZIntensity,
                                        double YZIntensity,
                                        double XYIntensity,
                                         IrtVecType* MicroBlobSpacing,
                                         IrtVecType* RandomFactors);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *User3DMicroBlobsTiling2(
                                         double XZIntensity,
                                         double YZIntensity,
                                         double XYIntensity,
                                          IrtVecType* MicroBlobSpacing,
                                          IrtVecType* RandomFactors);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *User3DMicroBlobsCreateRandomVector(int Size,
                                        User3DSpreadType BlobSpreadMethod,
                                        byte FirstVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int **User3DMicroBlobsCreateRandomMatrix(int Size,
                                         User3DSpreadType BlobSpreadMethod);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPVertexStruct *User3DDitherSetXYTranslations(
                                                 IPVertexStruct *Vrtcs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *User3DDither2Images( byte *Image1Name,
                                            byte *Image2Name,
                                           int DitherSize,
                                           int MatchWidth,
                                           int Negate,
                                           int AugmentContrast,
                                           User3DSpreadType SpreadMethod,
                                           double SphereRad,
                                           double *AccumPenalty);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *User3DDither3Images( byte *Image1Name,
                                            byte *Image2Name,
                                            byte *Image3Name,
                                           int DitherSize,
                                           int MatchWidth,
                                           int Negate,
                                           int AugmentContrast,
                                           User3DSpreadType SpreadMethod,
                                           double SphereRad,
                                           double *AccumPenalty);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserRegisterTestConvergance(double Dist, int i);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserRegisterTwoPointSets(int n1,
                                  IrtPtType* PtsSet1,
                                  int n2,
                                  IrtPtType* PtsSet2,
                                  double AlphaConverge,
                                  double Tolerance,
                                  UserRegisterTestConverganceFuncType
                                      RegisterTestConvergance,
                                  IrtHmgnMatType* RegMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserRegisterPointSetSrf(int n,
                                 IrtPtType* PtsSet,
                                  CagdSrfStruct *Srf,
                                 double AlphaConverge,
                                 double Tolerance,
                                 UserRegisterTestConverganceFuncType
                                                    RegisterTestConvergance,
                                 IrtHmgnMatType* RegMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserDDMPolysOverTrimmedSrf(
                                           TrimSrfStruct *TSrf,
                                           IPObjectStruct *Texture,
                                         double UDup,
                                         double VDup,
                                         int LclUV,
                                         int Random);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserDDMPolysOverSrf(
                                         CagdSrfStruct *Srf,
                                          IPObjectStruct *Texture,
                                        double UDup,
                                        double VDup,
                                        int LclUV,
                                        int Random);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserDDMPolysOverPolys(
                                          IPObjectStruct *PlSrf,
                                           IPObjectStruct *Texture,
                                         double UDup,
                                         double VDup,
                                         int LclUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSrfKernel( CagdSrfStruct *Srf,
                                     double SubdivTol,
                                     int SkipRate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSrfParabolicLines( CagdSrfStruct *Srf,
                                             double Step,
                                             double SubdivTol,
                                             double NumericTol,
                                             int Euclidean,
                                             int DecompSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSrfParabolicSheets( CagdSrfStruct *Srf,
                                              double Step,
                                              double SubdivTol,
                                              double NumericTol,
                                              double SheetExtent);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  MvarPtStruct *UserSrfUmbilicalPts( CagdSrfStruct *Srf,
                                         double SubTol,
                                         double NumTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSrfFixedCurvatureLines( CagdSrfStruct *Srf,
                                                  double k1,
                                                  double Step,
                                                  double SubdivTol,
                                                  double NumericTol,
                                                  int Euclidean);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserCrvCrvtrByOneCtlPt( CagdCrvStruct *Crv,
                                              int CtlPtIdx,
                                              double Min,
                                              double Max,
                                              double SubdivTol,
                                              double NumerTol,
                                              int Operation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int User2PolyMeshRoundEdge( IPPolygonStruct *Pl1,
                            IPPolygonStruct *Pl2,
                             IPPolygonStruct *Edge12,
                           double RoundRadius,
                           double RoundPower);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgScaleImage(IrtImgPixelStruct *InImage,
                                    int InMaxX,
                                    int InMaxY,
                                    int InAlpha,
                                    int OutMaxX,
                                    int OutMaxY,
                                    int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserWarpTextOnSurface(CagdSrfStruct *Srf,
                                              byte *Txt,
                                             double HSpace,
                                             double VBase,
                                             double VTop,
                                             double Ligatures);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * UserHCEditInit(double StartX, double StartY, int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * UserHCEditFromCurve( CagdCrvStruct *Crv, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditIsPeriodic(void * HC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserHCEditSetPeriodic(void * HC, int Periodic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditGetCtlPtCont(void * HC, int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserHCEditSetCtlPtCont(void * HC, int Index, int Cont);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserHCEditSetDrawCtlptFunc(void * HC,
                                UserHCEditDrawCtlPtFuncType CtlPtDrawFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserHCEditDelete(void * HC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * UserHCEditCopy(void * HC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditTranslate(void * HC, double Dx, double Dy);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditCreateAppendCtlpt(void * HC,
                                double x,
                                double y,
                                int MouseMode);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditCreateDone(void * HC, double StartX, double StartY);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditInsertCtlpt(void * HC, double x, double y, double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditDeleteCtlpt(void * HC, double x, double y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditUpdateCtl(void * HC,
                        int CtlIndex,
                        int IsPosition,
                        double NewX,
                        double NewY);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditMoveCtl(void * HC,
                      double OldX,
                      double OldY,
                      double NewX,
                      double NewY,
                      int MouseMode,
                      double *MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditMoveCtlPt(void * HC, 
                        double OldX,
                        double OldY,
                        double NewX,
                        double NewY,
                        int MouseMode);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditMoveCtlTan(void * HC,
                         double OldX,
                         double OldY,
                         double NewX,
                         double NewY,
                         int MouseMode);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditIsNearCrv(void * HC,
                        double x,
                        double y,
                        double *t,
                        double Eps,
                        int NormalizeZeroOne);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditIsNearCtlPt(void * HC,
                          double *x,
                          double *y,
                          int *Index,
                          int *UniqueID,
                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditIsNearCtlTan(void * HC,
                           double *x,
                           double *y,
                           int *Index,
                           int *UniqueID,
                           int *Forward,
                           double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserHCEditGetCrvRepresentation(void * HC, int ArcLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditGetCtlPtTan(void * HC, int Index, IrtPtType* Pos, IrtPtType* Tan);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditGetNumCtlPt(void * HC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditDrawCtlpts(void * HC, int DrawTans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditMatTrans(void * HC, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditTransform(void * HC, double *Dir, double Scl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditRelativeTranslate(void * HC, double *Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserHCEditEvalDefTans(void * HC, int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserNCContourToolPath(  IPObjectStruct *PObj,
                                             double Offset,
                                             double ZBaseLevel,
                                             double Tolerance,
                                             UserNCGCodeUnitType Units);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserNCPocketToolPath(  IPObjectStruct *PObj,
                                            double ToolRadius,
                                            double RoughOffset,
                                            double TPathSpace,
                                            double TPathJoin,
                                            UserNCGCodeUnitType Units,
                                            int TrimSelfInters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFEKElementStruct *UserFEKBuildMat(CagdSrfStruct *Srf,
                                      int IntegRes,
                                      double E,
                                      double Nu,
                                      int *Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFEKElementStruct *UserFEKBuildMat2(IrtPtType* Points,
                                       int ULength,
                                       int VLength,
                                       int UOrder,
                                       int VOrder,
                                       CagdEndConditionType EndCond,
                                       int IntegRes,
                                       double E,
                                       double Nu,
                                       int *Size);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserFEPointInsideSrf(CagdSrfStruct *Srf, IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFEInterIntervalStruct *UserFEGetInterInterval(CagdCrvStruct *Crv1,
                                                  CagdSrfStruct *Srf1,
                                                  CagdCrvStruct *Crv2,
                                                  CagdSrfStruct *Srf2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFECElementStruct *UserFEBuildC1Mat(CagdCrvStruct *Crv1,
                                       CagdSrfStruct *Srf1,
                                       CagdCrvStruct *Crv2,
                                       CagdSrfStruct *Srf2,
                                       int IntegRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFECElementStruct *UserFEBuildC1Mat2(IrtPtType* Crv1Pts,
                                        int Crv1Length,
                                        int Crv1Order,
                                        IrtPtType* Srf1Pts,
                                        int Srf1ULength,
                                        int Srf1VLength,
                                        int Srf1UOrder,
                                        int Srf1VOrder,
                                        IrtPtType* Crv2Pts,
                                        int Crv2Length,
                                        int Crv2Order,
                                        IrtPtType* Srf2Pts,
                                        int Srf2ULength,
                                        int Srf2VLength,
                                        int Srf2UOrder,
                                        int Srf2VOrder,
                                        CagdEndConditionType EndCond,
                                        int IntegRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFECElementStruct *UserFEBuildC2Mat(CagdCrvStruct *Crv1,
                                       CagdSrfStruct *Srf1,
                                       CagdCrvStruct *Crv2,
                                       CagdSrfStruct *Srf2,
                                       int IntegRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserFECElementStruct *UserFEBuildC2Mat2(IrtPtType* Crv1Pts,
                                        int Crv1Length,
                                        int Crv1Order,
                                        IrtPtType* Srf1Pts,
                                        int Srf1ULength,
                                        int Srf1VLength,
                                        int Srf1UOrder,
                                        int Srf1VOrder,
                                        IrtPtType* Crv2Pts,
                                        int Crv2Length,
                                        int Crv2Order,
                                        IrtPtType* Srf2Pts,
                                        int Srf2ULength,
                                        int Srf2VLength,
                                        int Srf2UOrder,
                                        int Srf2VOrder,
                                        CagdEndConditionType EndCond,
                                        int IntegRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserFEEvalRHSC(UserFECElementStruct *C,
                        CagdCrvStruct *Crv1,
                        CagdCrvStruct *Crv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserCrvArngmntStruct *UserCrvArngmntCreate(  IPObjectStruct *PCrvs,
                                           double EndPtEndPtTol,
                                           double PlanarityTol,
                                           int ProjectOnPlane,
                                           int InputMaskType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCAMergeCrvsAtAngularDev(UserCrvArngmntStruct *CA,
                                double AngularDeviation,
                                double PtPtEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCABreakLiNCrvsAtAngularDev(UserCrvArngmntStruct *CA,
                                   double AngularDeviation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntFilterDups(UserCrvArngmntStruct *CA,
                             int UpdateEndPts,
                             double EndPtEndPtTol,
                             double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntFilterTans(UserCrvArngmntStruct *CA, double FilterTans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntSplitAtPts(UserCrvArngmntStruct *CA,
                               IPObjectStruct *PtsObj,
                             double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntLinearCrvsFitC1(UserCrvArngmntStruct *CA, int FitSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntProcessIntersections(UserCrvArngmntStruct *CA,
                                       double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntProcessSpecialPts(UserCrvArngmntStruct *CA,
                                    double Tolerance,
                                    UserCASplitType CrvSplit);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntPrepEval(UserCrvArngmntStruct *CA);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntProcessEndPts(UserCrvArngmntStruct *CA);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntClassifyConnectedRegions(UserCrvArngmntStruct *CA,
                                           int IgnoreInteriorHangingCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserCrvArngmntGetCurves(UserCrvArngmntStruct *CA, int XYCurves);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntRegions2Curves( UserCrvArngmntStruct *CA,
                                 int Merge,
                                 int XYCurves,
                                 double ZOffset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntRegionsTopology( UserCrvArngmntStruct *CA,
                                  int XYCurves,
                                  double ZOffset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntIsContained( UserCrvArngmntStruct *CA,
                               CagdCrvStruct *InnerShape,
                               CagdCrvStruct *OuterLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntIsContained2( UserCrvArngmntStruct *CA,
                                IrtPtType* Pt,
                                CagdCrvStruct *Loop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserCrvArngmntReport( UserCrvArngmntStruct *CA,
                          int DumpCurves,
                          int DumpPts,
                          int DumpRegions,
                          int DumpXYData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntOutput( UserCrvArngmntStruct *CA,
                         int OutputStyle,
                         double Tolerance,
                         double ZOffset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserCrvArngmntStruct *UserCrvArngmntCopy( UserCrvArngmntStruct *CA);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserCrvArngmntFree(UserCrvArngmntStruct *CA);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserCrvs2DBooleans( CagdCrvStruct *Crvs1,
                                           CagdCrvStruct *Crvs2,
                                          BoolOperType BoolOper,
                                          int MergeLoops,
                                          int *ResultState);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserBeltCreate( IPVertexStruct *Circs,
                                      double BeltThickness,
                                      double BoundingArcs,
                                      int ReturnCrvs,
                                      int *Intersects,
                                       byte **Error);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserSCvrCoverSrf( CagdCrvStruct *DomainBndry, 
                                CagdCrvStruct *FillCrv, 
                                double CoverEps, 
                                double NumericTol, 
                                double SubdivTol, 
                                int TopK, 
                                double TopEps,
                                double IntrpBlndRatio);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserPkPackCircles(CagdCrvStruct *Bndry,
                                 double Radius,
                                 int NumIter,
                                 double NumericTol,
                                 double SubdivTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *UserRuledSrfFit( CagdSrfStruct *Srf,
                               CagdSrfDirType RulingDir,
                               double ExtndDmn,
                               int Samples,
                               double *Error,
                               double *MaxError);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserKnmtcsSolveMotion( UserKnmtcsStruct *System,
                          double NumTol,
                          double SubTol,
                          double Step,
                          int *SolDim,
                          int FilterSols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserKnmtcsNumOfSolPts(int PolyIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserKnmtcsEvalAtParams(int PolyIdx, int PtIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserKnmtcsEvalCrvTraces();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserKnmtcsFreeSol();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserKnmtcsSolveDone();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  UserDexelDxGridStruct *UserDexelGridNew(int GridType,
                                               double Ori0,
                                               double End0,
                                               double Ori1,
                                               double End1,
                                               int NumDx0,
                                               int NumDx1);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelDxGridCopy( UserDexelDxGridStruct *Dest,
                           UserDexelDxGridStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelDxGridSubtract( UserDexelDxGridStruct *GridA,
                               UserDexelDxGridStruct *GridB);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelDxGridUnion( UserDexelDxGridStruct *GridA,
                            UserDexelDxGridStruct *GridB);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelDxGridFree( UserDexelDxGridStruct *DxGrid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelDxClearGrid( UserDexelDxGridStruct *DxGrid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  UserDexelDxGridStruct *UserDexelGetDexelGridEnvelope0D(
                                  CagdPtStruct *EnvlPts,
                                  CagdPtStruct *EnvlNrmls,
                                   UserDexelDxGridStruct *Stock);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  UserDexelDxGridStruct *UserDexelGetDexelGridEnvelope1D(
                                  CagdPolylineStruct *PlaneEnvelope,
                                  CagdPolylineStruct *EnvlNrmls,
                                   UserDexelDxGridStruct *Stock);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelInitStock( UserDexelDxGridStruct *DxGrid,
                        double Top,
                        double Btm);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelInitStockSrf2( UserDexelDxGridStruct *DxGrid,
                            CagdSrfStruct *SrfList,
                            double BtmLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelInitStockSrf( UserDexelDxGridStruct *DxGrid,
                            CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserDexelWriteDexelGridToFile(byte *FileName, 
                                    UserDexelDxGridStruct *DxGrid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  UserDexelDxGridStruct *UserDexelReadDexelGridFromFile(byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *UserDexelColorTriangles(IPPolygonStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *UserDexelTriangulateDxGrid( UserDexelDxGridStruct
                                                                     *DxGrid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserSwpSecCnstrctToolGnrl( CagdCrvStruct *Profile);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserSwpSecElimRedundantToolShapes();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserSwpSecGetPlaneEnvelope(int PlnNrmlDir,
                                     double PlnValue,
                                     CagdPolylineStruct **PlnEnvl,
                                     CagdPolylineStruct **Nrmls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserSwpSecGetLineEnvelope(int PlnNrmlDir1,
                                    double PlnValue1,
                                    int PlnNrmlDir2,
                                    double PlnValue2,
                                    CagdPtStruct **EnvlPts,
                                    CagdPtStruct **Nrmls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *UserSwpSecGetSrfEnvelope();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *UserSwpSecMachiningSimulation( CagdCrvStruct *ToolProfile,
                                                IrtPtType* ToolOrigin,
                                                IPObjectStruct *MotionData,
                                               int DexelGridType,
                                                IrtPtType* GridOrigin,
                                                IrtPtType* GridEnd,
                                               int NumDexel0,
                                               int NumDexel1,
                                                CagdSrfStruct *StockSrf,
                                               double RectStockTopLevel,
                                               double RectStockBtmLevel,
                                                byte *OutputSavePath);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserSwpSecRenderTool();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserSweepSectionDone();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserText2OutlineCurves2DInit( byte *FName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserText2OutlineCurves2D( byte *Str,
                                                double Space,
                                                double ScaleFactor,
                                                double *Height);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern /* #define _FREETYPE_FONTS_                 On windows use native windows fonts. */

byte *UserWChar2Ascii( ushort* Str);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* UserAscii2WChar( byte *Str);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserFontConvertFontToBezier(
                                                 ushort* Text,
                                                 byte* FontName,
                                                UserFontStyleType FontStyle,
                                                double SpaceWidth,
                                                int MergeToBsp,
                                                 byte *RootObjName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern /* On other systems try to use the freetype library. */

 IPObjectStruct *UserFontFTStringOutline2BezierCurves(
                                                  ushort* Text,
                                                  byte* FontName,
                                                 double Spacing,
                                                 int MergeToBsp,
                                                  byte *RootObjName,
                                                  byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *UserFontBspCrv2Poly(CagdCrvStruct *BspCrv,
                                            double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *UserFontBzrList2BspList( IPObjectStruct *BzrListObj,
                                       byte *HaveHoles);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserFontBspList2Plgns( IPObjectStruct *BspListObj,
                                             double Tol,
                                              byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserFontBspList2TrimSrfs(
                                             IPObjectStruct *BspListObj,
                                            double Tol,
                                             byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *UserFontBspList2Solids(
                                            IPObjectStruct *BspListObj,
                                           UserFont3DEdgeType ExtStyle, 
                                           double ExtOffset, 
                                           double ExtHeight,
                                           double Tol,
                                           int GenTrimSrfs,
                                            byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserFontLayoutOverShapeFree( UserFontWordLayoutStruct *Words);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserSetErrorFuncType UserSetFatalErrorFunc(UserSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *UserDescribeError(UserFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserFatalError(UserFatalErrorType ErrID);
    }
}
