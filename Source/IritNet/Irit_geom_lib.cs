using System;
using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void GMPolyOffsetAmountFuncType (double *Coord);
    public unsafe delegate void GeomSetErrorFuncType (GeomFatalErrorType ErrorFunc);
    public unsafe delegate void GMZBufferUpdateFuncType (void * ZbufferID, int x, int y);
    public unsafe delegate void GMDistEnergy1DFuncType (double a);
    public unsafe delegate void GMScanConvertApplyFuncType (int x, int y);
    public unsafe delegate void GMTransObjUpdateFuncType (  IPObjectStruct *OldPObj,
                                                  IPObjectStruct *NewPObj,
                                                 IrtHmgnMatType* Mat,
                                                 int AnimUpdate);
    public unsafe delegate void GMFetchVertexPropertyFuncType ( IPVertexStruct *V,
                                                           IPPolygonStruct *Pl);
    public unsafe delegate void GMSphConeQueryCallBackFuncType ( IPVertexStruct *V);
    public unsafe delegate void GMSphConeQueryDirFuncType (IrtVecType* Vec, double ConeAngle);
    public unsafe delegate void GMPolyAdjacncyVertexFuncType ( IPVertexStruct *V1,
                                                      IPVertexStruct *V2,
                                                      IPPolygonStruct *Pl1,
                                                      IPPolygonStruct *Pl2);
    public unsafe delegate void GMTransObjUpdateAnimCrvsFuncType (
                                                         IPObjectStruct *PAnim,
                                                        IrtHmgnMatType* Mat);
    public unsafe delegate void GMMergePolyVrtxCmpFuncType ( IPVertexStruct *V1,
                                                   IPVertexStruct *V2,
                                                  double Eps);
    public unsafe delegate void GMMergeGeomInitFuncType (void * Entty);
    public unsafe delegate void GMMergeGeomDistFuncType (void * Entty1, void * Entty2);
    public unsafe delegate void GMMergeGeomKeyFuncType (void * Entty);
    public unsafe delegate void GMMergeGeomMergeFuncType (void **Entty1, void **Entty2);
    public unsafe delegate void GMIdentifyTJunctionFuncType ( IPVertexStruct *V0,
                                                     IPVertexStruct *V1,
                                                     IPVertexStruct *V2,
                                                     IPPolygonStruct *Pl0,
                                                     IPPolygonStruct *Pl1,
                                                     IPPolygonStruct *Pl2);
    public unsafe delegate void GMPointDeformVrtxDirFuncType (  IPVertexStruct
                                                                                 *V);
    public unsafe delegate void GMPointDeformVrtxFctrFuncType ( IPVertexStruct *V);
    public unsafe delegate void GMQuadWeightFuncType (  CagdPolylineStruct *P,
                                                 int *VIndices, 
                                                 int numV);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMBasicSetEps(double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVecCopy(IrtVecType* Vdst,  IrtVecType* Vsrc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVecNormalize(IrtVecType* V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMVecLength( IrtVecType* V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMVecMinAbsValueIndex( IrtVecType* V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMVecMaxAbsValueIndex( IrtVecType* V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVecCrossProd(IrtVecType* Vres,  IrtVecType* V1,  IrtVecType* V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMVecVecAngle( IrtVecType* V1,
                        IrtVecType* V2,
                       int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPlanarVecVecAngle( IrtVecType* V1,
                              IrtVecType* V2,
                             int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMOrthogonalVector( IrtVecType* V, IrtVecType* OV, int UnitLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCollinear3Pts( IrtPtType* Pt1,
                     IrtPtType* Pt2,
                     IrtPtType* Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCollinear3PtsInside( IrtPtType* Pt1,
                           IrtPtType* Pt2,
                           IrtPtType* Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCoplanar4Pts( IrtPtType* Pt1,
                    IrtPtType* Pt2,
                    IrtPtType* Pt3,
                    IrtPtType* Pt4);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMVecDotProd( IrtVecType* V1,  IrtVecType* V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVecReflectPlane(IrtVecType* Dst, IrtVecType* Src, IrtVecType* PlaneNormal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectRotX( double *Degree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectRotY( double *Degree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectRotZ( double *Degree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectTrans( IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectScale( IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGetMatTransPortion(
                                       IPObjectStruct *MatObj,
                                     int TransPortion);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMTransformPolyList(  IPPolygonStruct *Pls,
                                     IrtHmgnMatType* Mat,
                                     int IsPolygon);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMTransObjUpdateFuncType GMTransObjSetUpdateFunc(GMTransObjUpdateFuncType
                                                                   UpdateFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMTransObjUpdateAnimCrvsFuncType GMTransObjSetAnimCrvUpdateFunc(
                              GMTransObjUpdateAnimCrvsFuncType AnimUpdateFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMTransformObjectInPlace( IPObjectStruct *PObj,
                                                IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMTransformObject(  IPObjectStruct *PObj,
                                         IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMTransformObjectList(  IPObjectStruct *PObj,
                                             IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMTransObjUpdateAnimCrvs( IPObjectStruct *PAnim,
                                                IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectZ2Dir( IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectZ2Dir2( IrtVecType* Dir,
                                             IrtVecType* Dir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectRotVec( IrtVecType* Vec,
                                             double *Degree);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatObjectV2V( IrtVecType* V1,
                                          IrtVecType* V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMGenMatrix3Pts2EqltrlTri( IrtPtType* Pt1,
                                           IrtPtType* Pt2,
                                           IrtPtType* Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPointPoint( IrtPtType* P1,  IrtPtType* P2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMLineFrom2Points(IrtLnType* Line,
                       IrtPtType* Pt1, 
                       IrtPtType* Pt2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMPointVecFromLine( IrtLnType* Line, IrtPtType* Pt, IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPlaneFrom3Points(IrtPlnType* Plane,
                        IrtPtType* Pt1,
                        IrtPtType* Pt2,
                        IrtPtType* Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPointFromPointLine( IrtPtType* Point,
                               IrtPtType* Pl,
                               IrtPtType* Vl,
                              IrtPtType* ClosestPoint);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPointLine( IrtPtType* Point,
                          IrtPtType* Pl,
                          IrtPtType* Vl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPointPlane( IrtPtType* Point,  IrtPlnType* Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointFromPointPlane( IrtPtType* Pt,
                           IrtPlnType* Plane,
                          IrtPtType* ClosestPoint);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointFromLinePlane( IrtPtType* Pl,
                          IrtPtType* Vl,
                          IrtPlnType* Plane,
                         IrtPtType* InterPoint,
                         double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointFromLinePlane01( IrtPtType* Pl,
                            IrtPtType* Vl,
                            IrtPlnType* Plane,
                           IrtPtType* InterPoint,
                           double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointFromPlanarLineLine( IrtPtType* Pl1,
                               IrtPtType* Vl1,
                               IrtPtType* Pl2,
                               IrtPtType* Vl2,
                              IrtPtType* Pi,
                              double *t1,
                              double *t2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GM2PointsFromLineLine( IrtPtType* Pl1,
                           IrtPtType* Vl1,
                           IrtPtType* Pl2,
                           IrtPtType* Vl2,
                          IrtPtType* Pt1,
                          double *t1,
                          IrtPtType* Pt2,
                          double *t2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistLineLine( IrtPtType* Pl1,
                         IrtPtType* Vl1,
                         IrtPtType* Pl2,
                         IrtPtType* Vl2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointFrom3Planes( IrtPlnType* Pl1,
                        IrtPlnType* Pl2,
                        IrtPlnType* Pl3,
                       IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMLineFrom2Planes( IrtPlnType* Pl1,
                        IrtPlnType* Pl2,
                       IrtPtType* Pt,
                       IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPolyPt(  IPPolygonStruct *Pl,
                      IrtPtType* Pt,
                        IPVertexStruct **ExtremeV,
                      int MaxDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPolyPt2(  IPPolygonStruct *Pl,
                       int IsPolyline,
                       IrtPtType* Pt,
                       double *ExtremePt,
                       int MaxDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMDistPolyPoly(  IPPolygonStruct *Pl1,
                          IPPolygonStruct *Pl2,
                         IPVertexStruct **V1,
                         IPVertexStruct **V2,
                        int TagIgnoreV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonPlaneInter(  IPPolygonStruct *Pl,
                         IrtPlnType* Pln,
                        double *MinDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSplitPolygonAtPlane( IPPolygonStruct *Pl,  IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyPlaneClassify(  IPPolygonStruct *Pl,
                              IrtPlnType* Pln);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMTrianglePointInclusion( double *V1,
                              double *V2,
                              double *V3,
                              IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonPointInclusion(  IPPolygonStruct *Pl,
                             IrtPtType* Pt,
                            double OnBndryEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMAreaSphericalTriangle( IrtVecType* Dir1,
                                  IrtVecType* Dir2,
                                  IrtVecType* Dir3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMAngleSphericalTriangle( IrtVecType* Dir,
                                   IrtVecType* ODir1,
                                   IrtVecType* ODir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonPointInclusion3D(  IPPolygonStruct *Pl,
                               IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonRayInter(  IPPolygonStruct *Pl,
                       IrtPtType* PtRay,
                      int RayAxes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonRayInter2(  IPPolygonStruct *Pl,
                        IrtPtType* PtRay,
                       int RayAxes,
                        IPVertexStruct **FirstInterV,
                       double *FirstInterP,
                       double *AllInters);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolygonRayInter3D(  IPPolygonStruct *Pl,
                         IrtPtType* PtRay,
                        int RayAxes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyHierarchy2SimplePoly(
                                             IPPolygonStruct *Root,
                                             IPPolygonStruct *Islands);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenTransMatrixZ2Dir(IrtHmgnMatType* Mat,
                            IrtVecType* Trans,
                            IrtVecType* Dir,
                           double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixX2Dir(IrtHmgnMatType* Mat,  IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixY2Dir(IrtHmgnMatType* Mat,  IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixZ2Dir(IrtHmgnMatType* Mat,  IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenTransMatrixZ2Dir2(IrtHmgnMatType* Mat,
                             IrtVecType* Trans,
                             IrtVecType* Dir,
                             IrtVecType* Dir2,
                            double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixZ2Dir2(IrtHmgnMatType* Mat,
                        IrtVecType* Dir,
                        IrtVecType* Dir2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMatFromPosDir( IrtPtType* Pos,
                     IrtVecType* Dir,
                     IrtVecType* UpDir,
                    IrtHmgnMatType* M);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixRotVec(IrtHmgnMatType* Mat,
                        IrtVecType* Vec,
                       double Angle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenMatrixRotV2V(IrtHmgnMatType* Mat,
                        IrtVecType* V1,
                        IrtVecType* V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMGenMatrix4Pts2Affine4Pts( IrtPtType* P0,
                                IrtPtType* P1,
                                IrtPtType* P2,
                                IrtPtType* P3,
                                IrtPtType* Q0,
                                IrtPtType* Q1,
                                IrtPtType* Q2,
                                IrtPtType* Q3,
                               IrtHmgnMatType* Trans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenReflectionMat( IrtPlnType* ReflectPlane, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GM3Pts2EqltrlTriMat( IrtPtType* Pt1Orig,
                         IrtPtType* Pt2Orig,
                         IrtPtType* Pt3Orig,
                        IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *GMBaryCentric3Pts2D( IrtPtType* Pt1,
                               IrtPtType* Pt2,
                               IrtPtType* Pt3,
                               IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *GMBaryCentric3Pts( IrtPtType* Pt1,
                             IrtPtType* Pt2,
                             IrtPtType* Pt3,
                             IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GM2PointsFromCircCirc( IrtPtType* Center1,
                          double Radius1,
                           IrtPtType* Center2,
                          double Radius2,
                          IrtPtType* Inter1,
                          IrtPtType* Inter2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GM2PointsFromCircCirc3D( IrtPtType* Cntr1,
                             IrtVecType* Nrml1,
                            double Rad1,
                             IrtPtType* Cntr2,
                             IrtVecType* Nrml2,
                            double Rad2,
                            IrtPtType* Inter1,
                            IrtPtType* Inter2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCircleFrom3Points(IrtPtType* Center,
                        double *Radius,
                         IrtPtType* Pt1,
                         IrtPtType* Pt2,
                         IrtPtType* Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCircleFrom2Pts2Tans(IrtPtType* Center,
                          double *Radius,
                           IrtPtType* Pt1,
                           IrtPtType* Pt2,
                           IrtVecType* Tan1,
                           IrtVecType* Tan2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCircleFromLstSqrPts(IrtPtType* Center,
                          double *Radius,
                           IrtPtType* Pts,
                          int PtsSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsPtInsideCirc( double *Point,
                      double *Center,
                     double Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsPtOnCirc( double *Point,
                  double *Center,
                 double Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMAreaOfTriangle( double *Pt1,
                           double *Pt2,
                           double *Pt3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMRayCnvxPolygonInter( IrtPtType* RayOrigin,
                           IrtVecType* RayDir,
                            IPPolygonStruct *Pl,
                          IrtPtType* InterPoint);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointInsideCnvxPolygon( IrtPtType* Pt,
                               IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPointOnPolygonBndry( IrtPtType* Pt,
                            IPPolygonStruct *Pl,
                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSolveQuadraticEqn(double A, double B, double *Sols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSolveQuadraticEqn2(double B,
                         double C,
                         double *RSols,
                         double *ISols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSolveCubicEqn(double A, double B, double C, double *Sols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSolveCubicEqn2(double A,
                     double B,
                     double C, 
                     double *RSols,
                     double *ISols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSolveQuarticEqn(double A,
                      double B,
                      double C,
                      double D, 
                      double *Sols);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMComplexRoot(double RealVal,
                   double ImageVal,
                   double *RealRoot,
                   double *ImageRoot);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyLength(  IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolyCentroid(  IPPolygonStruct *Pl, IrtPtType* Centroid);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolyObjectAreaSetSigned(int SignedArea);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyObjectArea(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyOnePolyArea(  IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyObjectVolume( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMSphConeSetConeDensity(int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IrtVecType* GMSphConeGetPtsDensity(int *n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMSphConeQueryInit( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMSphConeQueryFree(void * SphCone);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMSphConeQueryGetVectors(void * SphConePtr,
                              IrtVecType* Dir,
                              double Angle,
                              GMSphConeQueryCallBackFuncType SQFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMSphConeQuery2GetVectors(void * SphConePtr,
                               GMSphConeQueryDirFuncType SQQuery,
                               GMSphConeQueryCallBackFuncType SQFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMConvexHull(IrtE2PtStruct *DTPts, int *NumOfPoints);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMonotonePolyConvex( IPVertexStruct *VHead, int Cnvx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMinSpanCirc(IrtE2PtStruct *DTPts,
                  int NumOfPoints,
                  IrtE2PtStruct *Center,
                  double *Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMinSpanConeAvg(IrtVecType* DTVecs,
                     int VecsNormalized,
                     int NumOfPoints,
                     IrtVecType* Center,
                     double *Angle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMinSpanCone(IrtVecType* DTVecs,
                  int VecsNormalized,
                  int NumOfPoints,
                  IrtVecType* Center,
                  double *Angle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMinSpanSphere(IrtE3PtStruct *DTPts,
                    int NumOfPoints,
                    IrtE3PtStruct *Center,
                    double *Radius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSphereWith3Pts(IrtE3PtStruct *Pts,
                     double *Center,
                     double *RadiusSqr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSphereWith4Pts(IrtE3PtStruct *Pts,
                     double *Center,
                     double *RadiusSqr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMSilPreprocessPolys( IPObjectStruct *PObj, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSilPreprocessRefine(void * PrepSils, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSilExtractSilDirect( IPObjectStruct *PObj,
                                             IrtHmgnMatType* ViewMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSilExtractSilDirect2( IPObjectStruct *PObjReg,
                                              IrtHmgnMatType* ViewMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSilExtractSil(void * PrepSils,
                                       IrtHmgnMatType* ViewMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSilExtractDiscont( IPObjectStruct *PObjReg,
                                           double MinAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSilExtractBndry( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMSilProprocessFree(void * PrepSils);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMSilOrigObjAlive(int ObjAlive);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimResetAnimStruct(GMAnimationStruct *Anim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimGetAnimInfoText(GMAnimationStruct *Anim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimHasAnimation(  IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimHasAnimationOne(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimAffineTransAnimation(  IPObjectStruct *PObjs,
                               double Trans,
                               double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimAffineTransAnimationOne(  IPObjectStruct *PObj,
                                  double Trans,
                                  double Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimAffineTransAnimation2(  IPObjectStruct *PObjs,
                                double Min,
                                double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimAffineTransAnimationOne2(  IPObjectStruct *PObj,
                                   double Min,
                                   double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimFindAnimationTimeOne(GMAnimationStruct *Anim,
                                  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimFindAnimationTime(GMAnimationStruct *Anim,
                               IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimSaveIterationsToFiles(GMAnimationStruct *Anim,
                                  IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimSaveIterationsAsImages(GMAnimationStruct *Anim,
                                   IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMExecuteAnimationEvalMat( IPObjectStruct *AnimationP,
                                   double Time,
                                   IrtHmgnMatType* ObjMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimDoAnimation(GMAnimationStruct *Anim,  IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimSetReverseHierarchyMatProd(int ReverseHierarchyMatProd);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimSetAnimInternalNodes(int AnimInternalNodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimEvalAnimation(double t,  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimEvalAnimationList(double t,  IPObjectStruct *PObjList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMAnimEvalObjAtTime(double t,
                                            IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMAnimDoSingleStep(GMAnimationStruct *Anim,  IPObjectStruct *PObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMAnimCheckInterrupt(GMAnimationStruct *Anim);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMBBSetBBoxPrecise(int Precise);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBComputeBboxObject(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBComputeBboxObjectList(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern   IPObjectStruct *GMBBSetGlblBBObjList(  IPObjectStruct
                                                                   *BBObjList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBComputeOnePolyBbox(  IPPolygonStruct *PPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBComputePolyListBbox(  IPPolygonStruct *PPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBComputePointBbox( double *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMBBBboxStruct *GMBBMergeBbox( GMBBBboxStruct *Bbox1,
                               GMBBBboxStruct *Bbox2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMConvexPolyNormals(int HandleNormals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMConvexRaysToVertices(int RaysToVertices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMConvexNormalizeNormal(int NormalizeNormals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvexPolyObjectN(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMConvexPolyObject( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsConvexPolygon2(  IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsConvexPolygon( IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMSplitNonConvexPoly( IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenRotateMatrix(IrtHmgnMatType* Mat,  IrtVecType* Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMTriangulatePolygon(  CagdPolylineStruct
                                                                          *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMTriangulatePolygon2(  IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMTriangulatePolygonList(  IPPolygonStruct
                                                                    *PlgnList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMQuadrangulatePolygon( 
                                                        CagdPolylineStruct *Pl,
                                               GMQuadWeightFuncType WF);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMQuadrangulatePolygon2(  
                                                           IPPolygonStruct *Pl,
                                                GMQuadWeightFuncType WF);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMQuadrangulatePolygonList(  
                                                     IPPolygonStruct *PlgnList,
                                                   GMQuadWeightFuncType WF);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMQuadAreaPerimeterRatioWeightFunc(  CagdPolylineStruct *P,
                                             int *VIndices, 
                                            int numV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMDecimateObject( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMDecimateObjSetDistParam(double d);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMDecimateObjSetPassNumParam(int p);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMDecimateObjSetDcmRatioParam(int r);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMDecimateObjSetMinAspRatioParam(double a);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * HDSCnvrtPObj2QTree( IPObjectStruct *PObjects, int Depth);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *HDSThreshold(void * Qt, double Threshold);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *HDSTriBudget(void * Qt, int TriBudget);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HDSFreeQTree(void * Qt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int HDSGetActiveListCount(void * Qt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int HDSGetTriangleListCount(void * Qt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int HDSGetDismissedTrianglesCount(void * Qt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMUpdateVerticesByInterp( IPPolygonStruct *PlList,
                                IPPolygonStruct *OriginalPl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMUpdateVertexByInterp( IPVertexStruct *VUpdate,
                              IPVertexStruct *V,
                              IPVertexStruct *VNext,
                            int DoRgb,
                            int DoUV,
                            int DoNrml);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMCollinear3Vertices(  IPVertexStruct *V1,
                           IPVertexStruct *V2,
                           IPVertexStruct *V3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMEvalWeightsVFromPl( double *Coord,
                           IPPolygonStruct *Pl,
                         double *Wgt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMInterpVrtxNrmlBetweenTwo( IPVertexStruct *V,
                                  IPVertexStruct *V1,
                                  IPVertexStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMInterpVrtxNrmlBetweenTwo2(IrtPtType* Pt,
                                 IrtVecType* Normal,
                                   IPVertexStruct *V1,
                                   IPVertexStruct *V2,
                                 int Normalize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMInterpVrtxNrmlFromPl( IPVertexStruct *V,
                             IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMInterpVrtxRGBBetweenTwo( IPVertexStruct *V,
                                IPVertexStruct *V1,
                                IPVertexStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMInterpVrtxRGBFromPl( IPVertexStruct *V,
                            IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMInterpVrtxUVBetweenTwo( IPVertexStruct *V,
                               IPVertexStruct *V1,
                               IPVertexStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMInterpVrtxUVFromPl( IPVertexStruct *V,
                           IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMBlendNormalsToVertices( IPPolygonStruct *PlList,
                              double MaxAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMFixOrientationOfPolyModel( IPPolygonStruct *Pls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMFixNormalsOfPolyModel( IPPolygonStruct *PlList, int TrustFixedPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMTwoPolySameGeom(  IPPolygonStruct *Pl1,
                        IPPolygonStruct *Pl2,
                      double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMCleanUpDupPolys( IPPolygonStruct **PPolygons,
                                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMCleanUpPolygonList(
                                            IPPolygonStruct **PPolygons,
                                           double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMCleanUpPolylineList(
                                           IPPolygonStruct **PPolylines,
                                          double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMCleanUpPolylineList2( IPPolygonStruct
                                                                 *PPolylines);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsPolygonPlanar(  IPPolygonStruct *Pl, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMVerifyPolygonsPlanarity( IPPolygonStruct *Pls,
                                                  double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVrtxListToCircOrLin( IPPolygonStruct *Pls, int DoCirc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMVrtxListToCircOrLinDup( IPPolygonStruct *Pls, int DoCirc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPVertexStruct *GMFilterInteriorVertices(
                                          IPVertexStruct *VHead,
                                         double MinTol,
                                         int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMClipPolysAgainstPlane( IPPolygonStruct *PHead,
                                          IPPolygonStruct **PClipped,
                                          IPPolygonStruct **PInter,
                                         IrtPlnType* Plane);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPVertexStruct *GMFindThirdPointInTriangle(
                                             IPPolygonStruct *Pl,
                                             IPVertexStruct *V,
                                             IPVertexStruct *VNext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMGetMaxNumVrtcsPoly( IPObjectStruct *PolyObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPolyHasCollinearEdges(  IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMConvertPolyToTriangles( IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMConvertPolyToTriangles2( IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvertPolysToNGons( IPObjectStruct *PolyObj,
                                             int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvertPolysToTriangles(  IPObjectStruct
                                                                    *PolyObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvertPolysToTriangles2(  IPObjectStruct
                                                                    *PolyObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvertPolysToTrianglesIntrrPt( IPObjectStruct
                                                                    *PolyObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMConvertPolysToRectangles( IPObjectStruct
                                                                    *PolyObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMLimitTrianglesEdgeLen(  IPPolygonStruct
                                                                     *OrigPls,
                                                double MaxLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMGenUVValsForPolys( IPObjectStruct *PObj,
                         double UTextureRepeat,
                         double VTextureRepeat,
                         double WTextureRepeat,
                         int HasXYZScale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMergeSameGeometry(void **GeomEntities,
                        int NumOfGEntities,
                        double IdenticalEps,
                        GMMergeGeomInitFuncType InitFunc,
                        GMMergeGeomDistFuncType DistSqrFunc,
                        GMMergeGeomKeyFuncType KeyFunc,
                        GMMergeGeomMergeFuncType MergeFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMMergeGeometry(void **GeomEntities,
                    int NumOfGEntities,
                    double Eps,
                    double IdenticalEps,
                    GMMergeGeomInitFuncType InitFunc,
                    GMMergeGeomDistFuncType DistSqrFunc,
                    GMMergeGeomKeyFuncType KeyFunc,
                    GMMergeGeomMergeFuncType MergeFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMMergeClosedLoopHoles( IPPolygonStruct *PlMain,
                                                IPPolygonStruct *PClosedPls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMMergePolylines( IPPolygonStruct *Polys,
                                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMMatchPointListIntoPolylines(
                                                IPObjectStruct *PtsList,
                                               double MaxTol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMPointCoverOfPolyObj( IPObjectStruct *PolyObj,
                                             int n,
                                             double *Dir,
                                             byte *PlAttr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMRegularizePolyModel(  IPObjectStruct *PObj,
                                             int SplitCollinear,
                                             double MinRefineDist);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMSplitPolysAtCollinearVertices(
                                                  IPPolygonStruct *Pls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMSplitPolyInPlaceAtVertex(
                                             IPPolygonStruct *Pl,
                                             IPVertexStruct *VHead);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMSplitPolyInPlaceAt2Vertices(
                                                IPPolygonStruct *Pl,
                                                IPVertexStruct *V1,
                                                IPVertexStruct *V2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMPolyOffsetAmountDepth( double *Coord);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyOffset(  IPPolygonStruct *Poly,
                                     int IsPolygon,
                                     double Ofst,
                                     GMPolyOffsetAmountFuncType AmountFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyOffset3D(  IPPolygonStruct *Poly,
                                       double Ofst,
                                       int ForceSmoothing,
                                       double MiterEdges,
                                       GMPolyOffsetAmountFuncType AmountFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrimSetGeneratePrimType(int PolygonalPrimitive);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrimSetSurfacePrimitiveRational(int SurfaceRational);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenBOXObject( IrtVecType* Pt,
                                        double WidthX,
                                        double WidthY,
                                        double WidthZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenBOXWIREObject( IrtVecType* Pt,
                                            double WidthX,
                                            double WidthY,
                                            double WidthZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenGBOXObject( IrtVecType* Pt,
                                          IrtVecType* Dir1,
                                          IrtVecType* Dir2,
                                          IrtVecType* Dir3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenCONEObject( IrtVecType* Pt,
                                          IrtVecType* Dir,
                                         double R,
                                         int Bases);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenCONE2Object( IrtVecType* Pt,
                                    IrtVecType* Dir,
                                          double R1,
                                          double R2,
                                          int Bases);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenCYLINObject( IrtVecType* Pt,
                                           IrtVecType* Dir,
                                          double R,
                                          int Bases);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenSPHEREObject( IrtVecType* Center,
                                           double R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenTORUSObject( IrtVecType* Center,
                                           IrtVecType* Normal,
                                          double Rmajor,
                                          double Rminor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenPOLYDISKObject( IrtVecType* Nrml,
                                              IrtVecType* Trns, 
                                             double R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenPOLYGONObject( IPObjectStruct *PObjList,
                                            int IsPolyline);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenObjectFromPolyList( IPObjectStruct
                                                                   *PObjList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenCROSSECObject(  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenSURFREVObject(  IPObjectStruct *Cross);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenSURFREVAxisObject( IPObjectStruct *Cross,
                                                 IrtVecType* Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenSURFREV2Object(  IPObjectStruct
                                                                      *Cross,
                                             double StartAngle,
                                             double EndAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenSURFREV2AxisObject( IPObjectStruct *Cross,
                                                 double StartAngle,
                                                 double EndAngle,
                                                  IrtVecType* Axis);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenEXTRUDEObject(  IPObjectStruct *Cross,
                                             IrtVecType* Dir,
                                            int Bases);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenRULEDObject(  IPObjectStruct *Cross1,
                                            IPObjectStruct *Cross2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *PrimGenPolygon4Vrtx( IrtVecType* V1,
                                             IrtVecType* V2,
                                             IrtVecType* V3,
                                             IrtVecType* V4,
                                             IrtVecType* Vin,
                                            int *VrtcsRvrsd,
                                             IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *PrimGenPolygon4Vrtx2( IrtVecType* V1,
                                              IrtVecType* V2,
                                              IrtVecType* V3,
                                              IrtVecType* V4,
                                              IrtVecType* Vin,
                                             int *VrtcsRvrsd,
                                             int *Singular,
                                       IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *PrimGenPolygon3Vrtx( IrtVecType* V1,
                                             IrtVecType* V2,
                                             IrtVecType* V3,
                                             IrtVecType* Vin,
                                            int *VrtcsRvrsd,
                                             IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenTransformController2D( GMBBBboxStruct *BBox,
                                                    int HasRotation,
                                                    int HasTranslation,
                                                    int HasScale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenTransformController2DCrvs(
                                                  GMBBBboxStruct *BBox);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenTransformControllerSphere(
                                                  GMBBBboxStruct *BBox, 
                                                 int HasRotation,
                                                 int HasTranslation,
                                                 int HasUniformScale,
                                                 double BoxOpacity,
                                                 double RelTesalate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenTransformControllerBox(
                                               GMBBBboxStruct *BBox, 
                                              int HasRotation,
                                              int HasTranslation,
                                              int HasUniformScale,
                                              double BoxOpacity,
                                              double RelTesalate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *PrimGenFrameController(double BBoxLen, 
                                       double NLeverLen, 
                                       double TLeverLen, 
                                        byte *HandleName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *PrimGenPolyline4Vrtx( IrtVecType* V1,
                                       IrtVecType* V2,
                                       IrtVecType* V3,
                                       IrtVecType* V4,
                                       IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrimSetResolution(int Resolution);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatToMat(GMQuatType q, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatMatToQuat(IrtHmgnMatType* Mat, GMQuatType q);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatRotationToQuat(double Xangle,
                          double Yangle, 
                          double Zangle,
                          GMQuatType q);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatToRotation(GMQuatType q, IrtVecType* Angles, int *NumSolutions);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatMul(GMQuatType q1, GMQuatType q2, GMQuatType QRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatAdd(GMQuatType q1, GMQuatType q2, GMQuatType QRes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMQuatIsUnitQuat(GMQuatType q);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatNormalize(GMQuatType q);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatInverse(GMQuatType SrcQ, GMQuatType DstQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatRotateVec(IrtVecType* OrigVec, GMQuatType RotQ, IrtVecType* DestVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatLog(GMQuatType SrcQ, IrtVecType* DstVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatExp(IrtVecType* SrcVec, GMQuatType DstQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatPow(GMQuatType MantisQ, double Expon, GMQuatType DstQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMQuatMatrixToAngles(IrtHmgnMatType* Mat, IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatMatrixToTranslation(IrtHmgnMatType* Mat, IrtVecType* Vec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMQuatMatrixToScale(IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMQuatMatrixToVector(IrtHmgnMatType* Mat, GMQuatTransVecType TransVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatVectorToMatrix(GMQuatTransVecType TransVec, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatVecToScaleMatrix(GMQuatTransVecType TransVec,
                            IrtHmgnMatType* ScaleMatrix);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatVecToRotMatrix(GMQuatTransVecType TransVec,
                          IrtHmgnMatType* RotMatrix);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMQuatVecToTransMatrix(GMQuatTransVecType TransVec,
                            IrtHmgnMatType* TransMatrix);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMMatrixToTransform(IrtHmgnMatType* Mat, 
                         IrtVecType* S,
                         GMQuatType R,
                         IrtVecType* T);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMPointCoverOfUnitHemiSphere(double HoneyCombSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferInit(int Width, int Height);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferFree(void * ZbufferID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferClear(void * ZbufferID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferClearSet(void * ZbufferID, double Depth);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMZTestsType GMZBufferSetZTest(void * ZbufferID, GMZTestsType ZTest);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GMZBufferUpdateFuncType GMZBufferSetUpdateFunc(void * ZbufferID,
                                               GMZBufferUpdateFuncType
                                                                   UpdateFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferInvert(void * ZbufferID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferRoberts(void * ZbufferID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferLaplacian(void * ZbufferID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMZBufferQueryZ(void * ZbufferID, int x, int y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferQueryInfo(void * ZbufferID, int x, int y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMZBufferUpdatePt(void * ZbufferID, int x, int y, double z);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMZBufferUpdateInfo(void * ZbufferID, int x, int y, void * Info);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferUpdateHLn(void * ZbufferID,
                        int x1,
                        int x2,
                        int y,
                        double z1,
                        double z2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferUpdateLine(void * ZbufferID,
                         int x1,
                         int y1,
                         int x2,
                         int y2,
                         double z1,
                         double z2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferUpdateTri(void * ZbufferID,
                        int x1,
                        int y1,
                        double z1,
                        int x2,
                        int y2,
                        double z2,
                        int x3,
                        int y3,
                        double z3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GMZBufferOGLInit(int Width,
                                    int Height,
                                    double ZMin,
                                    double ZMax,
                                    int OffScreen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferOGLClear();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferOGLSetColor(int Red, int Green, int Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferOGLMakeActive(IntPtr Id);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMZBufferOGLQueryZ(double x, double y);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferOGLQueryColor(double x,
                            double y,
                            int *Red,
                            int *Green,
                            int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMZBufferOGLFlush();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtPtType* GMSrfBilinearFit(IrtPtType* ParamDomainPts,
                            IrtPtType* EuclideanPts,
                            int FirstAtOrigin,
                            int NumPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtPtType* GMSrfQuadricFit(IrtPtType* ParamDomainPts,
                           IrtPtType* EuclideanPts,
                           int FirstAtOrigin,
                           int NumPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtPtType* GMSrfQuadricQuadOnly(IrtPtType* ParamDomainPts,
                                IrtPtType* EuclideanPts,
                                int FirstAtOrigin,
                                int NumEucDim,
                                int NumPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtPtType* GMSrfCubicQuadOnly(IrtPtType* ParamDomainPts,
                              IrtPtType* EuclideanPts,
                              int FirstAtOrigin,
                              int NumEucDim,
                              int NumPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *GMDistPoint1DWithEnergy(int N,
                                  double XMin,
                                  double XMax,
                                  int Resolution,
                                  GMDistEnergy1DFuncType EnergyFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolygonalMorphosis(  IPPolygonStruct *Pl1,
                                               IPPolygonStruct *Pl2,
                                             double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMLoadTextFont( byte *FName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMMakeTextGeometry( byte *Str,
                                           IrtVecType* Spacing,
                                           double *Scaling);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMPlCrvtrSetCurvatureAttr( IPPolygonStruct *PolyList,
                               int NumOfRings,
                               int EstimateNrmls);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMPlCrvtrSetFitDegree(int UseCubic);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMPlSilImportanceAttr( IPPolygonStruct *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPlSilImportanceRange( IPPolygonStruct
                                                                 *PolyList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyPropFetchAttribute( IPPolygonStruct *Pls,
                                           byte *PropAttr,
                                          double Value);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyPropFetchIsophotes( IPPolygonStruct *Pls,
                                           IrtVecType* ViewDir,
                                          double InclinationAngle);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyPropFetchCurvature( IPPolygonStruct *Pls,
                                          int CurvatureProperty,
                                          int NumOfRings,
                                          double CrvtrVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMPolyPropFetch( IPPolygonStruct *Pls,
                                 GMFetchVertexPropertyFuncType VertexProperty,
                                 double ConstVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPPolygonStruct *GMGenPolyline2Vrtx(IrtVecType* V1,
                                    IrtVecType* V2,
                                     IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GMFitEstimateRotationAxis(IrtPtType* PointsOnObject,
                                   IrtVecType* Normals,
                                    int NumberOfPoints, 
                                   IrtPtType* PointOnRotationAxis,
                                   IrtVecType* RotationAxisDirection);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * GMPolyAdjacncyGen( IPObjectStruct *PObj, double EqlEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMPolyAdjacncyVertex( IPVertexStruct *V,
                          void * PolyAdj,
                          GMPolyAdjacncyVertexFuncType AdjVertexFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMPolyAdjacncyFree(void * PolyAdj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIdentifyTJunctions( IPObjectStruct *PolyObj,
                         GMIdentifyTJunctionFuncType TJuncCB,
                         double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMRefineDeformedTriangle( IPPolygonStruct *Pl,
                             GMPointDeformVrtxFctrFuncType DeformVrtxFctrFunc,
                             GMPointDeformVrtxDirFuncType DeformVrtxDirFunc,
                             double DeviationTol,
                             double MaxEdgeLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMRefineDeformedTriangle2( IPPolygonStruct *Pl,
                              GMPointDeformVrtxFctrFuncType DeformVrtxFctrFunc,
                              byte Ref12,
                              byte Ref23,
                              byte Ref31);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMPolyMeshSmoothing(
                                IPObjectStruct *PolyObj,
                                 IPPolygonStruct *VerticesToRound,
                               int AllowBndryMove,
                               double RoundingRadius,
                               int NumIters,
                               double BlendFactor,
                               int CurvatureLimits);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GMFindUnConvexPolygonNormal(  IPVertexStruct *VL,
                                 IrtVecType* Nrml);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMFindPtInsidePolyKernel(  IPVertexStruct *VE,
                             IrtPtType* KrnlPt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsVertexBoundary(int Index,   IPPolyVrtxIdxStruct *PVIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMIsInterLinePolygon2D(  IPVertexStruct *VS, 
                            IrtPtType* V1, 
                            IrtPtType* V2, 
                           double *t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMComputeAverageVertex(  IPVertexStruct *VS, 
                           IrtPtType* CenterPoint, 
                           double BlendFactor);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GMComputeAverageVertex2( int *NS, 
                              IPPolyVrtxIdxStruct *PVIdx,
                            IrtPtType* CenterPoint, 
                            int CenterIndex,
                            double BlendFactor,
                            double DesiredRadius);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSubCatmullClark( IPObjectStruct *OriginalObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSubLoop( IPObjectStruct *OriginalObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPObjectStruct *GMSubButterfly( IPObjectStruct *OriginalObj, 
                                      double ButterflyWCoef);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern GeomSetErrorFuncType GeomSetFatalErrorFunc(GeomSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GeomFatalError(GeomFatalErrorType ErrID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *GeomDescribeError(GeomFatalErrorType ErrID);
    }
}
