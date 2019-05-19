using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void BoolFatalErrorFuncType (BoolFatalErrorType ErrID);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolGenAdjacencies( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolGenAdjSetSrfBoundaries(double UMin,
                               double VMin,
                               double UMax,
                               double VMax);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BoolClnAdjacencies( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolMarkDisjointParts( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *BoolGetDisjointPart( IPObjectStruct *PObj, 
                                            int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPVertexStruct *BoolGetAdjEdge( IPVertexStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *Boolean2D( IPPolygonStruct *Pl1,
                                   IPPolygonStruct *Pl2,
                                  BoolOperType BoolOper);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Bool2DInterStruct *Boolean2DComputeInters( IPPolygonStruct *Pl1,
                                           IPPolygonStruct *Pl2,
                                          int HandlePolygons,
                                          int DetectIntr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolFilterCollinearities( IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanOR( IPObjectStruct *PObj1,
                                  IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanAND( IPObjectStruct *PObj1,
                                   IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanSUB( IPObjectStruct *PObj1,
                                   IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanNEG( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanCUT( IPObjectStruct *PObj1,
                                   IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanICUT( IPObjectStruct *PObj1,
                                    IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanMERGE( IPObjectStruct *PObj1,
                                     IPObjectStruct *PObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanSELF( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanCONTOUR( IPObjectStruct *PObj,
                                      IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *BooleanMultiCONTOUR( IPObjectStruct *PObj,
                                           double CntrLevel,
                                           int Axis,
                                           int Init,
                                           int Done);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *BoolInterPolyPoly( IPPolygonStruct *Pl1,
                                           IPPolygonStruct *Pl2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BoolDfltFatalError(BoolFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern BoolFatalErrorFuncType BoolSetFatalErrorFunc(BoolFatalErrorFuncType ErrFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *BoolDescribeError(BoolFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolSetOutputInterCurve(int OutputInterCurve);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double BoolSetPerturbAmount(double PerturbAmount);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolSetHandleCoplanarPoly(int HandleCoplanarPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolSetParamSurfaceUVVals(int HandleBoolParamSrfUVVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BoolSetPolySortAxis(int PolySortAxis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPVertexStruct *BoolCutPolygonAtRay( IPPolygonStruct *Pl,
                                           IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BoolDebugPrintAdjacencies( IPObjectStruct *PObj);
    }
}
