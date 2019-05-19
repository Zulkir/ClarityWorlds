using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void TrngSetErrorFuncType (TrngFatalErrorType ErrorFunc);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double TrngIJChooseN(int i, int j, int N);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfNew(TrngGeomType GType,
                                   CagdPointType PType,
                                   int Length);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBspTriSrfNew(int Length,
                                      int Order,
                                      CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBzrTriSrfNew(int Length, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngGrgTriSrfNew(int Length, CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfCopy( TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfCopyList( TrngTriangSrfStruct *TriSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfFree(TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfFreeList(TrngTriangSrfStruct *TriSrfList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngCnvrtBzr2BspTriSrf( TrngTriangSrfStruct
                                                                      *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngCnvrtGregory2BzrTriSrf(TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfTransform(TrngTriangSrfStruct *TriSrf,
                         double *Translate,
                         double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfMatTransform(TrngTriangSrfStruct *TriSrf,
                            IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngCoerceTriSrfsTo( TrngTriangSrfStruct *TriSrf,
                                         CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngCoerceTriSrfTo( TrngTriangSrfStruct *TriSrf,
                                        CagdPointType PType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfDomain( TrngTriangSrfStruct *TriSrf,
                      double *UMin,
                      double *UMax,
                      double *VMin,
                      double *VMax,
                      double *WMin,
                      double *WMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngParamsInDomain( TrngTriangSrfStruct *TriSrf,
                             double u,
                             double v,
                             double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrngTriSrfEval( TrngTriangSrfStruct *TriSrf,
                          double u,
                          double v,
                          double w);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *TrngTriSrfEval2( TrngTriangSrfStruct *TriSrf,
                           double u,
                           double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdVecStruct *TrngTriSrfNrml( TrngTriangSrfStruct *TriSrf,
                              double u,
                              double v);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfBBox( TrngTriangSrfStruct *TriSrf, GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngTriSrfListBBox( TrngTriangSrfStruct *TriSrfs,
                        GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolylineStruct *TrngTriSrf2CtrlMesh( TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngBspTriSrfHasOpenEC( TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBspTriSrfOpenEnd( TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPolygonStruct *TrngTriSrf2Polygons( TrngTriangSrfStruct *TriSrf,
                                       int FineNess,
                                       int ComputeNormals,
                                       int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBzrTriSrfDirecDerive( TrngTriangSrfStruct *TriSrf,
                                              IrtVecType* DirecDeriv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfDerive( TrngTriangSrfStruct *TriSrf,
                                      TrngTriSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBzrTriSrfDerive( TrngTriangSrfStruct *TriSrf,
                                         TrngTriSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBspTriSrfDerive( TrngTriangSrfStruct *TriSrf,
                                         TrngTriSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrngCrvFromTriSrf( TrngTriangSrfStruct *TriSrf,
                                 double t,
                                 TrngTriSrfDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngTriSrfsSame( TrngTriangSrfStruct *Srf1,
                           TrngTriangSrfStruct *Srf2,
                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngDbg(void *Obj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngGregory2Bezier4(double **Qt, double **Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngGregory2Bezier5(double **Qt, double **Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngGregory2Bezier6(double **Qt, double **Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngSetErrorFuncType TrngSetFatalErrorFunc(TrngSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrngFatalError(TrngFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *TrngDescribeError(TrngFatalErrorType ErrID);
    }
}
