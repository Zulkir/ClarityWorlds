using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectColor(IPObjectStruct *PObj, int Color);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetObjectColor( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRGBColor(IPObjectStruct *PObj, int Red, int Green, int Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetObjectRGBColor( IPObjectStruct *PObj,
                          int *Red,
                          int *Green,
                          int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetObjectRGBColor2( IPObjectStruct *PObj,
                            byte *Name,
                           int *Red,
                           int *Green,
                           int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRGBDoubleColor(IPAttributeStruct **Attrs,
                           double Red,
                           double Green,
                           double Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetRGBDoubleColor( IPAttributeStruct *Attrs,
                          double *Red,
                          double *Green,
                          double *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectWidth(IPObjectStruct *PObj, double Width);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetObjectWidth( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectIntAttrib(IPObjectStruct *PObj,  byte *Name, int Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectIntAttrib2(IPObjectStruct *PObj,
                             uint AttribNum,
                             int Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetObjectIntAttrib( IPObjectStruct *PObj,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetObjectIntAttrib2( IPObjectStruct *PObj,
                            uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRealAttrib(IPObjectStruct *PObj,
                              byte *Name,
                             double Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRealAttrib2(IPObjectStruct *PObj,
                              uint AttribNum,
                              double Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetObjectRealAttrib( IPObjectStruct *PObj,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetObjectRealAttrib2( IPObjectStruct *PObj,
                                  uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRealPtrAttrib(IPObjectStruct *PObj,
                                 byte *Name,
                                double *Data,
                                int DataLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRealPtrAttrib2(IPObjectStruct *PObj,
                                 uint AttribNum,
                                 double *Data,
                                 int DataLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *AttrGetObjectRealPtrAttrib( IPObjectStruct *PObj,
                                      byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *AttrGetObjectRealPtrAttrib2( IPObjectStruct *PObj,
                                      uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectUVAttrib(IPObjectStruct *PObj,
                            byte *Name,
                           double U,
                           double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectUVAttrib2(IPObjectStruct *PObj,
                            uint AttribNum,
                            double U,
                            double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float *AttrGetObjectUVAttrib( IPObjectStruct *PObj,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float *AttrGetObjectUVAttrib2( IPObjectStruct *PObj,
                              uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectPtrAttrib(IPObjectStruct *PObj,
                             byte *Name,
                            void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectPtrAttrib2(IPObjectStruct *PObj,
                             uint AttribNum,
                             void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetObjectPtrAttrib( IPObjectStruct *PObj,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetObjectPtrAttrib2( IPObjectStruct *PObj,
                                uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRefPtrAttrib(IPObjectStruct *PObj,
                                byte *Name,
                               void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectRefPtrAttrib2(IPObjectStruct *PObj,
                                uint AttribNum,
                                void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetObjectRefPtrAttrib( IPObjectStruct *PObj,
                                   byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetObjectRefPtrAttrib2( IPObjectStruct *PObj,
                                   uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectStrAttrib(IPObjectStruct *PObj,
                             byte *Name,
                             byte *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectStrAttrib2(IPObjectStruct *PObj,
                             uint AttribNum,
                              byte *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *AttrGetObjectStrAttrib( IPObjectStruct *PObj,
                                    byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *AttrGetObjectStrAttrib2( IPObjectStruct *PObj,
                                    uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectObjAttrib(IPObjectStruct *PObj,
                             byte *Name,
                            IPObjectStruct *Data,
                            int CopyData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjectObjAttrib2(IPObjectStruct *PObj,
                             uint AttribNum,
                             IPObjectStruct *Data,
                             int CopyData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjAttrib(IPAttributeStruct **Attrs,
                       byte *Name,
                      IPObjectStruct *Data,
                      int CopyData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetObjAttrib2(IPAttributeStruct **Attrs,
                       uint AttribNum,
                       IPObjectStruct *Data,
                       int CopyData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *AttrGetObjectObjAttrib( IPObjectStruct *PObj,
                                        byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *AttrGetObjectObjAttrib2( IPObjectStruct *PObj,
                                        uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *AttrGetObjAttrib( IPAttributeStruct *Attrs,
                                  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *AttrGetObjAttrib2( IPAttributeStruct *Attrs,
                                  uint AttribNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrFreeObjectAttribute(IPObjectStruct *PObj,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrCopyOneAttribute( IPAttributeStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrCopyAttributes( IPAttributeStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrPropagateAttr(IPObjectStruct *PObj,  byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrPropagateRGB2Vrtx(IPObjectStruct *PObj);
    }
}
