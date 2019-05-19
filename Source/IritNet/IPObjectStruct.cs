using System;
using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe struct IPObjectStruct
    {
        public struct Union
        {
            public struct LstStruct
            {
                public IPObjectStruct **PObjList;            /* List of objects. */
                public int ListMaxLen;           /* Maximum number of elements in list. */
            }

            public IPPolygonStruct *Pl { get { var loc = this; return *(IPPolygonStruct**)&loc; } set { var loc = this; *(IPPolygonStruct**)&loc = value; this = loc; } }
            public CagdCrvStruct *Crvs { get { var loc = this; return *(CagdCrvStruct**)&loc; } set { var loc = this; *(CagdCrvStruct**)&loc = value; this = loc; } }
            public CagdSrfStruct *Srfs { get { var loc = this; return *(CagdSrfStruct**)&loc; } set { var loc = this; *(CagdSrfStruct**)&loc = value; this = loc; } }
            public TrimSrfStruct *TrimSrfs { get { var loc = this; return *(TrimSrfStruct**)&loc; } set { var loc = this; *(TrimSrfStruct**)&loc = value; this = loc; } }
            public TrivTVStruct *Trivars { get { var loc = this; return *(TrivTVStruct**)&loc; } set { var loc = this; *(TrivTVStruct**)&loc = value; this = loc; } }
            public TrngTriangSrfStruct *TriSrfs { get { var loc = this; return *(TrngTriangSrfStruct**)&loc; } set { var loc = this; *(TrngTriangSrfStruct**)&loc = value; this = loc; } }
            public IPInstanceStruct *Instance { get { var loc = this; return *(IPInstanceStruct**)&loc; } set { var loc = this; *(IPInstanceStruct**)&loc = value; this = loc; } }
            public MdlModelStruct *Mdls { get { var loc = this; return *(MdlModelStruct**)&loc; } set { var loc = this; *(MdlModelStruct**)&loc = value; this = loc; } }
            public VMdlVModelStruct *VMdls { get { var loc = this; return *(VMdlVModelStruct**)&loc; } set { var loc = this; *(VMdlVModelStruct**)&loc = value; this = loc; } }
            public MvarMVStruct *MultiVars { get { var loc = this; return *(MvarMVStruct**)&loc; } set { var loc = this; *(MvarMVStruct**)&loc = value; this = loc; } }
            public double R { get { var loc = this; return *(double*)&loc; } set { var loc = this; *(double*)&loc = value; this = loc; } }
            public IrtPtType Pt { get { var loc = this; return *(IrtPtType*)&loc; } set { var loc = this; *(IrtPtType*)&loc = value; this = loc; } }
            public IrtVecType Vec { get { var loc = this; return *(IrtVecType*)&loc; } set { var loc = this; *(IrtVecType*)&loc = value; this = loc; } }
            public IrtPlnType Plane { get { var loc = this; return *(IrtPlnType*)&loc; } set { var loc = this; *(IrtPlnType*)&loc = value; this = loc; } }
            public CagdCtlPtStruct CtlPt;                        /* Control point data. */
            public IrtHmgnMatType *Mat { get { var loc = this; return *(IrtHmgnMatType**)&loc; } set { var loc = this; *(IrtHmgnMatType**)&loc = value; this = loc; } }
            public LstStruct Lst { get { var loc = this; return *(LstStruct*)&loc; } set { var loc = this; *(LstStruct*)&loc = value; this = loc; } }
            public byte *Str { get { var loc = this; return *(byte**)&loc; } set { var loc = this; *(byte**)&loc = value; this = loc; } }
            public void* *VPtr { get { var loc = this; return *(void***)&loc; } set { var loc = this; *(void***)&loc = value; this = loc; } }
        }

        public IPObjectStruct* Pnext;                       /* To next in chain. */
        public IPAttributeStruct* Attr;                  /* Object's attributes. */
        public IPODObjectDpndncyStruct* Dpnds;   /* Dependencies and parameters. */
        public int Count;                                  /* Reference Count. */
        public int Tags;                                   /* Some attributes. */
        public IPObjStructType ObjType;        /* Object Type: Numeric, Geometric, etc. */
        public IrtBboxType BBox;                     /* BBox of object. */
        public Union U;
        public byte* ObjName;		                          /* Name of object. */

        public string _cs_Name => Marshal.PtrToStringAnsi((IntPtr)ObjName);
    }
}