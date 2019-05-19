using System;

namespace IritNet
{
    public unsafe struct IRndrZBufferStruct
    {
        public IRndrZListStruct** z;
        public int SizeX;
        public int SizeY;
        public int TargetSizeX;
        public int TargetSizeY;
        public IRndrZBufferCmpType ZBufCmp;

        public FastAllocStruct* PointsAlloc;
        public byte ColorsValid;
        public int AccessMode;
        public IRndrFilterType *Filter;
        public int UseTransparency;
        public IRndrColorType BackgroundColor;
        public double BackgroundDepth;
        public IRndrSceneStruct* Scene;
        public int ColorQuantization;
        public IRndrVMStruct *VisMap;
        public int DoVisMapScan;   /* Flags if scan convention is done over UV coords. */
        public int ScanContinuousDegenTriangleForVisMap;

        /* Temporary line calculation buffers. */
        public IRndrColorType* LineColors;
        public byte* LineAlpha;
        public IrtImgPixelStruct* LinePixels;

        /* Callbacks. */
        public IntPtr ZPol;
        public IntPtr PreZCmpClbk;
        public IntPtr ZPassClbk;
        public IntPtr ZFailClbk;

        public IRndrStencilCfgStruct StencilCfg;

        public IntPtr ImgSetType;
        public IntPtr ImgOpen;
        public IntPtr ImgWriteLine;
        public IntPtr ImgClose;
    }
}