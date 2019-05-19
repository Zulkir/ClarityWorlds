using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IPObjTypeAsString( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeObjectSlots(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeObjectGeomData(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeObjectBase(IPObjectStruct *O);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPAllocVertex(byte Tags,
                              IPPolygonStruct *PAdj,
                              IPVertexStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPAllocVertex2(IPVertexStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPAllocPolygon(byte Tags,
                                IPVertexStruct *V,
                                IPPolygonStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPAllocObject( byte *Name,
                              IPObjStructType ObjType,
                              IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeVertex(IPVertexStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreePolygon(IPPolygonStruct *P);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeObject(IPObjectStruct *O);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeVertexList(IPVertexStruct *VFirst);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreePolygonList(IPPolygonStruct *PPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFreeObjectList(IPObjectStruct *O);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolyVrtxIdxStruct *IPPolyVrtxIdxNew(int NumVrtcs, int NumPlys);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolyVrtxIdxStruct *IPPolyVrtxIdxNew2(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPolyVrtxIdxFree(IPPolyVrtxIdxStruct *PVIdx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPIsFreeObject(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPIsConsistentFreeObjList();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPListObjectLength( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPListObjectFind( IPObjectStruct *PObjList,
                      IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPListObjectInsert(IPObjectStruct *PObjList,
                        int Index,
                        IPObjectStruct *PObjItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPListObjectInsert2(IPObjectStruct *PObj,
                         int Index,
                         IPObjectStruct *PObjItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPListObjectAppend(IPObjectStruct *PObjList, IPObjectStruct *PObjItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPListObjectDelete(IPObjectStruct *PObj, int Index, int FreeItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPListObjectDelete2(IPObjectStruct *PObj,
                         IPObjectStruct *PObjToDel,
                         int FreeItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPListObjectGet( IPObjectStruct *PObjList, int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPropagateObjectName(IPObjectStruct *Obj,
                            byte *ObjName,
                           int ForceRename);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPReallocNewTypeObject(IPObjectStruct *PObj, IPObjStructType ObjType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPolyObject( byte *Name,
                                IPPolygonStruct *Pl,
                                IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPOLYObject(IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPolylineObject( byte *Name,
                                    IPPolygonStruct *Pl,
                                    IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPOLYLINEObject(IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPointListObject( byte *Name,
                                     IPPolygonStruct *Pl,
                                     IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPOINTLISTObject(IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenCrvObject( byte *Name,
                               CagdCrvStruct *Crv,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenCRVObject(CagdCrvStruct *Crv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenSrfObject( byte *Name,
                               CagdSrfStruct *Srf,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenSRFObject(CagdSrfStruct *Srf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTrimSrfObject( byte *Name,
                                   TrimSrfStruct *TrimSrf,
                                   IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTRIMSRFObject(TrimSrfStruct *TrimSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTrivarObject( byte *Name,
                                  TrivTVStruct *Triv,
                                  IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTRIVARObject(TrivTVStruct *Triv);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTriSrfObject( byte *Name,
                                  TrngTriangSrfStruct *TriSrf,
                                  IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenTRISRFObject(TrngTriangSrfStruct *TriSrf);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenModelObject( byte *Name,
                                 MdlModelStruct *Model,
                                 IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenMODELObject(MdlModelStruct *Model);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenVModelObject( byte *Name,
                                  VMdlVModelStruct *VModel,
                                  IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenVMODELObject(VMdlVModelStruct *VModel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenMultiVarObject( byte *Name,
                                    MvarMVStruct *MultiVar,
                                    IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenMULTIVARObject(MvarMVStruct *MultiVar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenInstncObject( byte *Name,
                                   byte *InstncName,
                                   IrtHmgnMatType* Mat,
                                  IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenINSTNCObject( byte *InstncName,
                                   IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenCtlPtObject( byte *Name,
                                 CagdPointType PtType,
                                  double *Coords,
                                 IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenCTLPTObject(CagdPointType PtType,  double *Coords);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenNumObject( byte *Name,
                                double *R,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenNUMObject( double *R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenNUMValObject(double R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPtObject( byte *Name,
                               double *Pt0,
                               double *Pt1,
                               double *Pt2,
                              IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPTObject( double *Pt0,
                               double *Pt1,
                               double *Pt2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenVecObject( byte *Name,
                                double *Vec0,
                                double *Vec1,
                                double *Vec2,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenVECObject( double *Vec0,
                                double *Vec1,
                                double *Vec2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenStrObject( byte *Name,
                                byte *Str,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenSTRObject( byte *Str);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenListObject( byte *Name,
                                IPObjectStruct *First,
                                IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenLISTObject(IPObjectStruct *First);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPlaneObject( byte *Name,
                                  double *Plane0,
                                  double *Plane1,
                                  double *Plane2,
                                  double *Plane3,
                                 IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenPLANEObject( double *Plane0,
                                  double *Plane1,
                                  double *Plane2,
                                  double *Plane3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenMatObject( byte *Name,
                               IrtHmgnMatType* Mat,
                               IPObjectStruct *Pnext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGenMATObject(IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPCopyObjectAuxInfo(IPObjectStruct *Dest,  IPObjectStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCopyObject(IPObjectStruct *Dest,
                              IPObjectStruct *Src,
                             int CopyAll);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCopyObjectGeomData(IPObjectStruct *Dest,
                                      IPObjectStruct *Src,
                                     int CopyAll);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetCopyObjectReferenceCount(int RefCount);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCopyObjectList( IPObjectStruct *PObjs, int CopyAll);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCopyPolygon( IPPolygonStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCopyPolygonList( IPPolygonStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPCopyVertex( IPVertexStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPCopyVertexList( IPVertexStruct *Src);
    }
}
