using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelCopy( VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelCopyList( VMdlVModelStruct *Mdls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlVModelFree(VMdlVModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlVModelFreeList(VMdlVModelStruct *VMdls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlGlueVModels(VMdlVModelStruct *Mdl1,
                                  VMdlVModelStruct *Mdl2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlGlueVModelsAppend(VMdlVModelStruct **Mdl1, 
                                 VMdlVModelStruct *Mdl2, 
                                double SrfDiffEps,
                                int CalculateConnectivity,
                                MiscPHashMapStruct* TVHMap);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlPrintVModel(VMdlVModelStruct *VM);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlPrintVE(VMdlVModelStruct *VM);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlSplitVModelInDir(VMdlVModelStruct *VM,
                          double Dx,
                          double Dy,
                          double Dz);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlAllocVModel();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVolumeElementStruct *VMdlAllocTrimVolumeElem();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVolumeElementRefStruct *VMdlAllocTrimVolumeElemRef();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimPointStruct *VMdlAllocTrimInterPoint();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimSrfStruct *VMdlAllocInterTrimSrf();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimCurveSegStruct *VMdlAllocInterTrimCurveSeg();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimCurveSegRefStruct *VMdlAllocInterTrimCurveSegRef();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrivTVRefStruct *VMdlAllocInterTrivTVRef();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimSrfRefStruct *VMdlAllocInterTrimSrfRef();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimCurveSegLoopInSrfStruct *VMdlAllocInterTrimCurveSegLoopInSrf();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlInterTrimPointRefStruct *VMdlAllocInterTrimPointRef();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelFromVElement( VMdlVolumeElementStruct *VElem, 
                                         int UseVElemInPlace);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeTrimVolElem(VMdlVolumeElementStruct *TrimVolElem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeTrimVolumeElemRef(VMdlVolumeElementRefStruct *TrimVolElemRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimPnt(VMdlInterTrimPointStruct *IntJunctionList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimSrf(VMdlInterTrimSrfStruct *TrimSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimCurveSeg(VMdlInterTrimCurveSegStruct *CurveSegList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimCurveSegRef(VMdlInterTrimCurveSegRefStruct *CrvSegRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrivTVRef(VMdlInterTrivTVRefStruct *TrivTVRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimSrfRef(VMdlInterTrimSrfRefStruct *SrfRef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimCurveSegLoopInSrf(
                             VMdlInterTrimCurveSegLoopInSrfStruct *CrvSegLoop);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlFreeInterTrimPointRef(VMdlInterTrimPointRefStruct *Ref);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlIsPointInsideVModel( VMdlVModelStruct *VMdl,
                                   IrtPtType* Pnt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlIsUVWPnttInsideVModel( VMdlVModelStruct *VMdl,
                                     IrtPtType* UVWPnt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlGetVModelEnclosedVolume( VMdlVModelStruct *VMdl,
                                      double *EnclosedVol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlVModelBBox( VMdlVModelStruct *VMdl, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlVModelListBBox( VMdlVModelStruct *Mdls,
                        GMBBBboxStruct *CagdBbox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *VMdlGetBoundaryVModel( VMdlVModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *VMdlGetBoundarySurfaces2( VMdlVModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *VMdlGetOuterBoundarySurfacesVModel( VMdlVModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *VMdlGetBndryVElement(VMdlVolumeElementStruct *VCell);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *VMdlGetBoundaryCurves( VMdlVModelStruct *Mdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVolElementFromBoundaryModel(MdlModelStruct *InBMdl, 
                                                  VMdlInterTrivTVRefStruct 
                                                          *ElementTVsRefList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimBoxVMdl(double MinX,
                                  double MinY,
                                  double MinZ,
                                  double MaxX,
                                  double MaxY,
                                  double MaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimCubeSphereVMdl( IrtVecType* Center,
                                         double Radius,
                                         int Rational,
                                         double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimTorusVMdl( IrtVecType* Center,
                                    double MajorRadius,
                                    double MinorRadius,
                                    int Rational,
                                    double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimConeVMdl( IrtVecType* Center,
                                   double Radius,
                                   double Height,
                                   int Rational,
                                   double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimCone2VMdl( IrtVecType* Center,
                                    double MajorRadius,
                                    double MinorRadius,
                                    double Height,
                                    int Rational,
                                    double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlPrimCylinderVMdl( IrtVecType* Center,
                                       double Radius,
                                       double Height,
                                       int Rational,
                                       double InternalCubeEdge);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct* VMdlRuledTrimSrf( TrimSrfStruct *TSrf1,
                                    CagdSrfStruct *Srf2,
                                   int OtherOrder,
                                   int OtherLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlExtrudeTrimSrf( TrimSrfStruct *Section,
                                     CagdVecStruct *Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlTrimVModelBySurface( VMdlVModelStruct *VMdl,
                                   CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlRemoveTrimmingSurface( VMdlVModelStruct *VMdl,
                                     CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlVModelTransform(VMdlVModelStruct *VMdl, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *VMdlFetchTrivar( VMdlVolumeElementStruct *VMTrimmedTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *VMdlFetchTrimmingSrfs( VMdlVolumeElementStruct *VMTrimmedTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelIntersect( VMdlVModelStruct *VMdlA,
                                       VMdlVModelStruct *VMdlB,
                                       VMdlAttribBlendObj *AttribBlendObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelUnion( VMdlVModelStruct *VMdlA,
                                   VMdlVModelStruct *VMdlB,
                                  VMdlBoolOpType OpType,
                                   VMdlAttribBlendObj *AttrBlendObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelSubtract( VMdlVModelStruct *VMdlA,
                                      VMdlVModelStruct *VMdlB,
                                      VMdlAttribBlendObj *AttrBlendObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelSymDiff( VMdlVModelStruct *VMdlA,
                                     VMdlVModelStruct *VMdlB,
                                     VMdlAttribBlendObj *AttrBlendObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelNegate( VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlVModelCut( VMdlVModelStruct *VMdlA,
                                 VMdlVModelStruct *VMdlB,
                                 VMdlAttribBlendObj *AttrBlendObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlClipVModelByPlane( VMdlVModelStruct *Mdl,
                                         IrtPlnType* Pln,
                                        VMdlBoolOpType BoolOp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlSubdivideVElement(VMdlVolumeElementStruct *VElem,
                                         TrivTVStruct *TV,
                                        TrivTVDirType Dir,
                                        double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlSubdivideVModel(VMdlVModelStruct *VMdl,
                                      TrivTVDirType Dir,
                                      double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlSubdivideVElemToBezierVElements(
                                          VMdlVolumeElementStruct *VElem,
                                          TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlSubdivideVMdlToBezierVElements(
                                                 VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *VMdlGetPointVMdlAttribute( VMdlVModelStruct *VMdl,
                                              IrtPtType* UVW,
                                             int AttributeID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlCnvrtSrf2VMdl( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlCnvrtTrimmedSrf2VMdl( TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlCnvrtTrivar2VMdl( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlCnvrtTrivar2VMdlList(TrivTVStruct *TVList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *VMdlCnvrtVMdl2TrimmedSrfs( VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *VMdlCnvrtVMdl2Mdl( VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlAddTrimSrfToVMdl(VMdlVModelStruct *VMdl,
                           TrimSrfStruct *TSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlSetSplitPeriodicTV(int Split);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlExtractVElements( VMdlVModelStruct *VMdl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlSlicerInfoStruct *VMdlSlicerInitVElement(
                                       VMdlVolumeElementStruct *VolumeElement,
                                        VMdlSlicerParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlSlicerInfoStruct *VMdlSlicerInitTrivar(
                                        TrivTVStruct *Trivar, 
                                        VMdlSlicerParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  VMdlSlicerInfoStruct *VMdlSlicerInitTrivMdl(
                                        TrivTVStruct *Trivar,
                                        MdlModelStruct *BMdl,
                                        VMdlSlicerParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlSlicerFree( VMdlSlicerInfoStruct *Info);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlSlicerSetCurrSliceZ( VMdlSlicerInfoStruct *Info, double z);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlSlicerGetCurrSliceXY( VMdlSlicerInfoStruct *Info,
                             int x,
                             int y,
                             double *Params,
                             double *Pos);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlSlicerNormParams( VMdlSlicerInfoStruct *Info,
                          double *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMdlSlicerGetSliceSize( VMdlSlicerInfoStruct *Info, int *Size);
    }
}
