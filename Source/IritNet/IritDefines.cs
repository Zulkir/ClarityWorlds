using System;
using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe static partial class Irit
    {
        public const int FALSE = 0;
        public const int TRUE = 1;

        public const int IP_ATTR_BAD_INT = -2147182588;
        public const double IP_ATTR_BAD_REAL = 1e30;
        public const int IP_ATTR_NO_COLOR = 999;
        public const double IP_ATTR_NO_WIDTH = 1e30;

        public static bool IP_ATTR_IS_BAD_INT(int i) { return ((i) == IP_ATTR_BAD_INT); }
        public static bool IP_ATTR_IS_BAD_REAL(double r) { return ((r) > IP_ATTR_BAD_REAL / 10.0); }
        public static bool IP_ATTR_IS_BAD_COLOR(int c) { return  ((c) == IP_ATTR_NO_COLOR); }
        public static bool IP_ATTR_IS_BAD_WIDTH(double w) { return ((w) > IP_ATTR_NO_WIDTH / 10.0); }

        public static bool IP_IS_FFGEOM_OBJ(IPObjectStruct* Obj)
        {
            return
                IP_IS_CRV_OBJ(Obj) ||
                IP_IS_SRF_OBJ(Obj) ||
                IP_IS_TRIMSRF_OBJ(Obj) ||
                IP_IS_TRIVAR_OBJ(Obj) ||
                IP_IS_TRISRF_OBJ(Obj) ||
                IP_IS_MODEL_OBJ(Obj) ||
                IP_IS_MVAR_OBJ(Obj) ||
                IP_IS_VMODEL_OBJ(Obj) ||
                IP_IS_INSTNC_OBJ(Obj);
        }

        public static bool IP_IS_CRV_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_CURVE; }
        public static bool IP_IS_SRF_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_SURFACE; }
        public static bool IP_IS_TRIMSRF_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_TRIMSRF; }
        public static bool IP_IS_TRIVAR_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_TRIVAR; }
        public static bool IP_IS_TRISRF_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_TRISRF; }
        public static bool IP_IS_MODEL_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_MODEL; }
        public static bool IP_IS_MVAR_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_MULTIVAR; }
        public static bool IP_IS_VMODEL_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_VMODEL; }
        public static bool IP_IS_INSTNC_OBJ(IPObjectStruct* obj) { return  obj->ObjType == IPObjStructType.IP_OBJ_INSTANCE; }

        public static bool IP_IS_POLYGON_OBJ(IPObjectStruct* obj) { return  (obj->Tags & 0x03) == 0; }
        public static bool IP_IS_POLYLINE_OBJ(IPObjectStruct* obj) { return  (obj->Tags & 0x03) == 1; }
        public static bool IP_IS_POINTLIST_OBJ(IPObjectStruct* obj) { return  (obj->Tags & 0x03) == 2; }

        public static bool IP_VALID_OBJ_NAME(IPObjectStruct* obj) { return  obj->ObjName != (void*) 0 && obj->ObjName[0] != 0; }
        public static string IP_GET_OBJ_NAME(IPObjectStruct* obj) { return  IP_VALID_OBJ_NAME(obj) ? Marshal.PtrToStringAnsi((IntPtr)obj->ObjName) : ""; }

        public const int GM_BBOX_MAX_DIM = 19;
        public const int IRIT_LINE_LEN = 256;

        public const int TRIV_IGA_MAX_MATERIAL_NAME_LEN = 256;
        public const int TRIV_IGA_MAX_FIELD_TYPE_LEN = 256;

        public const int CAGD_MAX_PT_SIZE = 19;
        public const int USER_MACRO_MAX_DIM = 3;
    }
}