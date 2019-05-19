using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void IGDrawUpdateFuncType ();
    public unsafe delegate void IGDrawObjectFuncType (IPObjectStruct *PObj);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGRedrawViewWindow();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawObjectFuncType IGDSetDrawPolyFunc(IGDrawObjectFuncType DrawPolyFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGConfirmConvexPolys(IPObjectStruct *PObj, int Depth);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGLoadGeometry( byte **FileNames, int NumFiles);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGSaveCurrentMat(int ViewMode, byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGActiveListFreePolyIsoAttribute(IPObjectStruct *PObjs,
                                      int FreePolygons,
                                      int FreeIsolines,
                                      int FreeSketches,
                                      int FreeCtlMesh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGActiveListFreeNamedAttribute(IPObjectStruct *PObjs, byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGActiveFreePolyIsoAttribute(IPObjectStruct *PObj,
                                  int FreePolygons,
                                  int FreeIsolines,
                                  int FreeSketches,
                                  int FreeCtlMesh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGActiveFreeNamedAttribute(IPObjectStruct *PObj, byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGUpdateObjectBBox(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGUpdateViewConsideringScale(IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IGFindMinimalDist(IPObjectStruct *PObj,
                           IPPolygonStruct **MinPl,
                           IrtPtType* MinPt,
                           int *MinPlIsPolyline,
                           IrtPtType* LinePos,
                           IrtVecType* LineDir,
                           double *HitDepth);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolygonSketches(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGGenPolygonSketches(IPObjectStruct *PObj, double FineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolySilhBndry(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolySilhouette(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolyBoundary(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolyDiscontinuities(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGDrawPolyContoursSetup(double x, double y, double z, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolyContours(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGDrawPolyIsophotesSetup(double x, double y, double z, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawPolyIsophotes(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGGetObjIsoLines(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGGetObjPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGInitSrfTexture(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGInitSrfMovie(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGClearObjTextureMovieAttr(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGDefaultProcessEvent(IGGraphicEventType Event, double *ChangeFactor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGDefaultStateHandler(int State, int StateStatus, int Refresh);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGUpdateFPS(int Start);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawCurve(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawCurveGenPolylines(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawModel(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawModelGenPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawVModel(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawVModelGenPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawString(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IGCnvrtChar2Raster( byte c);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawSurface(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawSurfaceGenPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawSurfaceAIso(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTriangSrf(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTriangGenSrfPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTrimSrf(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTrimSrfGenPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTrivar(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGDrawTrivarGenSrfPolygons(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGSketchDrawSurface(IPObjectStruct *PObjSketches);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGSketchGenSrfSketches(CagdSrfStruct *Srf,
                                       double FineNess,
                                       IPObjectStruct *PObj,
                                       int Importance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGSketchDrawPolygons(IPObjectStruct *PObjSketches);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGSketchGenPolySketches(IPObjectStruct *PlObj,
                                        double FineNess,
                                        int Importance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IGSketchGenPolyImportanceSketches(IPObjectStruct *PObj,
                                            IGSketchParamStruct *SketchParams,
                                            double FineNess);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSrfPolysPreFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSrfPolysPostFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSrfWirePreFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSrfWirePostFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSketchPreFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetSketchPostFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetCtlMeshPreFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IGDrawUpdateFuncType IGSetCtlMeshPostFunc(IGDrawUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGCGDrawDTexture(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IGCGFreeDTexture(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IGCGFfdDraw(IPObjectStruct *PObj);
    }
}
