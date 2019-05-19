using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCagdPllns2IritPllns(CagdPolylineStruct *Polys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCagdPlgns2IritPlgns(CagdPolygonStruct *Polys,
                                       int ComputeUV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCurve2Polylines( CagdCrvStruct *Crv,
                                   double TolSamples,
                                   SymbCrvApproxMethodType Method);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *IPPolyline2Curve( IPPolygonStruct *Pl, int Order);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCurve2CtlPoly( CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPSurface2CtlMesh( CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern SymbPlErrorFuncType IPSrf2OptPolysSetUserTolFunc(SymbPlErrorFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSurface2PolygonsGenTriOnly(int OnlyTri);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSurface2PolygonsGenDegenPolys(int DegenPolys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPlgErrorFuncType IPPolygonSetErrFunc(CagdPlgErrorFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPSurface2Polygons(CagdSrfStruct *Srf,
                                    int FourPerFlat,
                                    double FineNess,
                                    int ComputeUV,
                                    int ComputeNrml,
                                    int Optimal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTrimSrf2CtlMesh(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTrimSrf2Polygons( TrimSrfStruct *TrimSrf,
                                    int FourPerFlat,
                                    double FineNess,
                                    int ComputeUV,
                                    int ComputeNrml,
                                    int Optimal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTrivar2Polygons(TrivTVStruct *Trivar,
                                   int FourPerFlat,
                                   double FineNess,
                                   int ComputeUV,
                                   int ComputeNrml,
                                   int Optimal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTrivar2CtlMesh(TrivTVStruct *Trivar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTriSrf2Polygons(TrngTriangSrfStruct *TriSrf,
                                   double FineNess,
                                   int ComputeUV,
                                   int ComputeNrml,
                                   int Optimal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPTriSrf2CtlMesh(TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPSetCurvesToCubicBzrTol(double Tolerance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *IPCurvesToCubicBzrCrvs(CagdCrvStruct *Crvs,
                                      IPPolygonStruct **CtlPolys,
                                      int DrawCurve,
                                      int DrawCtlPoly,
                                      double MaxArcLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *IPSurfacesToCubicBzrSrfs(CagdSrfStruct *Srfs,
                                        CagdSrfStruct **NoConvertionSrfs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPClosedPolysToOpen(IPPolygonStruct *Pls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPOpenPolysToClosed(IPPolygonStruct *Pls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPFreeformConvStateStruct IPFreeForm2PolysSetCState( 
                                          IPFreeformConvStateStruct *CState);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPFreeForm2Polygons(IPFreeFormStruct *FreeForms,
                                    int Talkative,
                                    int FourPerFlat,
                                    double FineNess,
                                    int ComputeUV,
                                    int ComputeNrml,
                                    int Optimal,
                                    int BBoxGrid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPConvertFreeForm(IPObjectStruct *PObj,
                                  IPFreeformConvStateStruct *State);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPMapObjectInPlace(IPObjectStruct *PObj, IrtHmgnMatType* Mat);
    }
}
