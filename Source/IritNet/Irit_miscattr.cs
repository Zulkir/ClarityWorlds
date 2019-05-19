using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrGetIndexColor(int Color, int *Red, int *Green, int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetColor(IPAttributeStruct **Attrs, int Color);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetColor( IPAttributeStruct *Attrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRGBColor(IPAttributeStruct **Attrs, int Red, int Green, int Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetRGBColor( IPAttributeStruct *Attrs,
                    int *Red,
                    int *Green,
                    int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetRGBColor2( IPAttributeStruct *Attrs, 
                      byte *Name,
                     int *Red, 
                     int *Green, 
                     int *Blue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetWidth(IPAttributeStruct **Attrs, double Width);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetWidth( IPAttributeStruct *Attrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetIntAttrib(IPAttributeStruct **Attrs,  byte *Name, int Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetIntAttrib2(IPAttributeStruct **Attrs,
                       uint AttribNum,
                       int Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetIntAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttrGetIntAttrib2( IPAttributeStruct *Attrs, uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRealAttrib(IPAttributeStruct **Attrs,
                        byte *Name,
                       double Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRealAttrib2(IPAttributeStruct **Attrs,
                        uint AttribNum,
                        double Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetRealAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double AttrGetRealAttrib2( IPAttributeStruct *Attrs,
                            uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRealPtrAttrib(IPAttributeStruct **Attrs,
                           byte *Name,
                          double *Data,
                          int DataLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRealPtrAttrib2(IPAttributeStruct **Attrs,
                           uint AttribNum,
                           double *Data,
                           int DataLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *AttrGetRealPtrAttrib( IPAttributeStruct *Attrs,
                                byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double *AttrGetRealPtrAttrib2( IPAttributeStruct *Attrs,
                                uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetUVAttrib(IPAttributeStruct **Attrs,
                      byte *Name,
                     double U,
                     double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetUVAttrib2(IPAttributeStruct **Attrs,
                      uint AttribNum,
                      double U,
                      double V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float *AttrGetUVAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float *AttrGetUVAttrib2( IPAttributeStruct *Attrs, uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetPtrAttrib(IPAttributeStruct **Attrs,
                       byte *Name,
                      void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetPtrAttrib2(IPAttributeStruct **Attrs,
                       uint AttribNum,
                       void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetPtrAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetPtrAttrib2( IPAttributeStruct *Attrs,
                          uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRefPtrAttrib(IPAttributeStruct **Attrs,
                          byte *Name,
                         void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetRefPtrAttrib2(IPAttributeStruct **Attrs,
                          uint AttribNum,
                          void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetRefPtrAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * AttrGetRefPtrAttrib2( IPAttributeStruct *Attrs,
                             uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetStrAttrib(IPAttributeStruct **Attrs,
                       byte *Name,
                       byte *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrSetStrAttrib2(IPAttributeStruct **Attrs,
                       uint AttribNum,
                        byte *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *AttrGetStrAttrib( IPAttributeStruct *Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *AttrGetStrAttrib2( IPAttributeStruct *Attrs,
                              uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  IPAttributeStruct *AttrTraceAttributes(
                                           IPAttributeStruct *TraceAttrs,
                                           IPAttributeStruct *FirstAttrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *Attr2String( IPAttributeStruct *Attr, int DataFileFormat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrReverseAttributes(IPAttributeStruct *Attr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrFreeOneAttribute(IPAttributeStruct **Attrs,  byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrFreeAttributes(IPAttributeStruct **Attrs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrFindAttribute( IPAttributeStruct *Attrs,
                                      byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *_AttrMallocAttribute( byte *Name,
                                        IPAttributeType Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *_AttrMallocNumAttribute(uint AttribNum, 
                                           IPAttributeType Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _AttrFreeAttributeData(IPAttributeStruct *Attr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte **AttrCopyValidAttrList( byte **AttrNames);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrMergeAttributes(IPAttributeStruct *Orig,
                                        IPAttributeStruct *Src,
                                       int Replace);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *AttrGetAttribName( IPAttributeStruct *Attr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint AttrGetAttribNumber( byte *AttribName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPAttributeStruct *AttrFindNumAttribute( IPAttributeStruct *Attrs, 
                                        uint AttrNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrInitHashTbl();
    }
}
