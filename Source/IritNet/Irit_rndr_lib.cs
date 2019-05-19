using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void IRndrZCmpPolicyFuncType (int x,
                                                    int y,
                                                    double OldZ,
                                                    double NewZ);
    public unsafe delegate void IRndrPixelClbkFuncType (int x,
                                               int y,
                                               IRndrColorType Color,
                                               double Z,
                                               void * ClbkData);
    public unsafe delegate void IRndrImgSetTypeFuncType ( byte *ImageType);
    public unsafe delegate void IRndrImgOpenFuncType ( byte **argv,
                                             byte *FName,
                                            int Alpha,
                                            int XSize,
                                            int YSize);
    public unsafe delegate void IRndrImgWriteLineFuncType (byte *Alpha,
                                                  IrtImgPixelStruct *Pixels);
    public unsafe delegate void IRndrImgCloseFuncType ();
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrStruct* IRndrInitialize(int SizeX,
                             int SizeY,
                             int SuperSampSize,
                             int ColorQuantization,
                             byte UseTransparency,
                             byte BackfaceCulling,
                             IRndrColorType BackgrCol,
                             double AmbientLight,
                             int VisMap);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrDestroy(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrClearDepth(IRndrStruct* Rend, float ClearZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrClearStencil(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrClearColor(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrAddLightSource(IRndrStruct* Rend,
                         IRndrLightType Type,
                         IrtPtType* Where,
                         IRndrColorType Color);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSetFilter(IRndrStruct* Rend,
                    byte *FilterName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrShadingType IRndrSetShadeModel(IRndrStruct* Rend,
                                    IRndrShadingType ShadeModel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSetViewPrsp(IRndrStruct* Rend,
                      IrtHmgnMatType* ViewMat,
                      IrtHmgnMatType* PrspMat,
                      IrtHmgnMatType* ScrnMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetViewPrsp(IRndrStruct* Rend,
                      IrtHmgnMatType* ViewMat,
                      IrtHmgnMatType* PrspMat,
                      IrtHmgnMatType* ScrnMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSetPllParams(IRndrStruct* Rend,
                       double MinWidth,
                       double MaxWidth,
                       double ZNear,
                       double ZFar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte IRndrSetRawMode(IRndrStruct* Rend, byte UseRawMode);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrZCmpPolicyFuncType IRndrSetZCmpPolicy(IRndrStruct* Rend,
                                           IRndrZCmpPolicyFuncType ZCmpPol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrZBufferCmpType IRndrSetZCmp(IRndrStruct* Rend, IRndrZBufferCmpType ZCmp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrPixelClbkFuncType IRndrSetPreZCmpClbk(IRndrStruct* Rend,
                                           IRndrPixelClbkFuncType PixelClbk);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSetPostZCmpClbk(IRndrStruct* Rend,
                          IRndrPixelClbkFuncType ZPassClbk,
                          IRndrPixelClbkFuncType ZFailClbk);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrStencilCmpFunc(IRndrStruct* Rend,
                         IRndrStencilCmpType SCmp,
                         int Ref,
                          int Mask);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrStencilOp(IRndrStruct* Rend,
                    IRndrStencilOpType Fail,
                    IRndrStencilOpType ZFail,
                    IRndrStencilOpType ZPass);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrBeginObject(IRndrStruct* Rend,
                      IPObjectStruct *Object,
                      int NoShading);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrPutTriangle(IRndrStruct* Rend, IPPolygonStruct *Triangle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrEndObject(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrBeginPll(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrPutPllVertex(IRndrStruct* Rend, IPVertexStruct *Vertex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrEndPll(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrPutPixel(IRndrStruct* Rend,
                   int x,
                   int y,
                   double z,
                   double Transparency,
                   IRndrColorType Color,
                   IPPolygonStruct *Triangle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetPixelColorAlpha(IRndrStruct* Rend,
                             int x,
                             int y,
                             IRndrColorType *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetPixelDepth(IRndrStruct* Rend,
                        int x,
                        int y,
                        double *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetPixelStencil(IRndrStruct* Rend,
                          int x,
                          int y,
                          int *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetLineColorAlpha(IRndrStruct* Rend,
                            int y,
                            IRndrColorType *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetLineDepth(IRndrStruct* Rend, int y, double *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetLineStencil(IRndrStruct* Rend, int y, int *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrGetClippingPlanes(IRndrStruct* Rend, IrtPlnType* ClipPlanes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSetZBounds(IRndrStruct* Rend, double ZNear, double ZFar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSaveFileCB(IRndrStruct* Rend,
                     IRndrImgSetTypeFuncType ImgSetType,
                     IRndrImgOpenFuncType ImgOpen,
                     IRndrImgWriteLineFuncType ImgWriteLine,
                     IRndrImgCloseFuncType ImgClose);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSaveFile(IRndrStruct* Rend,
                    byte *BaseDirectory,
                    byte *OutFileName,
                    byte *Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSaveFileDepth(IRndrStruct* Rend,
                         byte *BaseDirectory,
                         byte *OutFileName,
                         byte *Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSaveFileStencil(IRndrStruct* Rend,
                           byte *BaseDirectory,
                           byte *OutFileName,
                           byte *Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrSaveFileVisMap(IRndrStruct* Rend,
                          byte *BaseDirectory,
                          byte *OutFileName,
                          byte *Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IRndrSceneStruct *IRndrGetScene(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrZBuffer1DStruct* IRndr1DInitialize(int ZBuf1DSize,
                                        double XMin,
                                        double XMax,
                                        double ZMin,
                                        double ZMax,
                                        int BottomMaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DClearDepth(IRndrZBuffer1DStruct* Rend, double ClearZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrZBufferCmpType IRndr1DSetZCmp(IRndrZBuffer1DStruct* Rend,
                                   IRndrZBufferCmpType ZCmp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DDestroy(IRndrZBuffer1DStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DPutPolyline(IRndrZBuffer1DStruct* Rend, IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DPutLine(IRndrZBuffer1DStruct* Rend,
                    double x1,
                    double z1,
                    double x2,
                    double z2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DPutPixel(IRndrZBuffer1DStruct* Rend, int x, double z);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DGetPixelDepth(IRndrZBuffer1DStruct* Rend, int x, double *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndr1DGetLineDepth(IRndrZBuffer1DStruct* Rend,
                         int x1,
                         int x2,
                         double *ZValues);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IRndr1DUpperEnvAsPolyline(IRndrZBuffer1DStruct* Rend,
                                           int MergeInters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IRndr1DFilterCollinearEdges(IRndrZBuffer1DStruct* Rend,
                                             IPPolygonStruct *Pl,
                                             int MergeInters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern INCZBufferStruct* INCRndrInitialize(int ZBufSizeX,
                                    int ZBufSizeY,
                                    int GridSizeX,
                                    int GridSizeY,
                                    IrtPtType* XYZMin,
                                    IrtPtType* XYZMax,
                                    int BottomMaxZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IRndrZBufferCmpType INCRndrSetZCmp(INCZBufferStruct* Rend,
                                   IRndrZBufferCmpType ZCmp);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrDestroy(INCZBufferStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrBeginObject(INCZBufferStruct* Rend, IPObjectStruct *Object);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrPutTriangle(INCZBufferStruct* Rend, IPPolygonStruct *Triangle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double INCRndrPutMask(INCZBufferStruct* Rend,
                        int *PosXY,
                        double PosZ,
                        double *Mask,
                        int MaskXSize,
                        int MaskYSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrEndObject(INCZBufferStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrPutPixel(INCZBufferStruct* Rend, int x, int y, double z);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrGetPixelDepth(INCZBufferStruct* Rend,
                          int x,
                          int y,
                          double *Result);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrGetLineDepth(INCZBufferStruct* Rend,
                         int x1,
                         int x2,
                         int y,
                         double *ZValues);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void INCRndrGetZbufferGridCellMaxSize(INCZBufferStruct* Rend,
                                      int *GridSizeX,
                                      int *GridSizeY,
                                      int *GridCellXSize,
                                      int *GridCellYSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INCRndrGetZbufferGridCell(INCZBufferStruct* Rend,
                              int GridCellX,
                              int GridCellY,
                              double *ZValues,
                              int *XMin,
                              int *YMin,
                              int *XMax,
                              int *YMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INCRndrMapPixelsToCells(INCZBufferStruct* Rend, int *X, int *Y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int INCRndrGetActiveCells(INCZBufferStruct* Rend,
                          int *MinCellX,
                          int *MinCellY,
                          int *MaxCellX,
                          int *MaxCellY,
                          double *ZPixelsRemoved);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IRndrVisMapEnable(IRndrStruct* Rend,
                      IPObjectStruct* Objects,
                      int SuperSize,
                      int UVBackfaceCulling);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVisMapScan(IRndrStruct* Rend);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVisMapSetTanAngle(IRndrStruct* Rend, double CosAng);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVisMapSetCriticAR(IRndrStruct* Rend, double CriticAR);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVisMapSetDilation(IRndrStruct* Rend, int Dilation);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IRndrVisMapGetObjDomain(IPObjectStruct *PObj, 
                            double *UMin, 
                            double *UMax, 
                            double *VMin, 
                            double *VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IRndrVisMapPrepareUVValuesOfGeoObj(IPObjectStruct *PObj, 
                                       int MapWidth, 
                                       int MapHeight,
                                       IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVisMapSetScanOnUV(IRndrStruct* Rend, int IsSacnOnUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IRndrVertexTransform(IRndrStruct* Rend,
                          IPVertexStruct *Vertex,                          
                          double *Result);
    }
}
