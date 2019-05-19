using System.Runtime.InteropServices;

namespace IritNet
{
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MCPolygonStruct *MCThresholdCube(MCCubeCornerScalarStruct *CCS,
                                 double Threshold);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *MCExtractIsoSurface( byte *FileName,
                                    int DataType,
                                    IrtPtType* CubeDim,
                                    int Width,
                                    int Height,
                                    int Depth,
                                    int SkipFactor,
                                    double IsoVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *MCExtractIsoSurface2( TrivTVStruct *TV,
                                     int Axis,
                                     int TrivarNormals,
                                     IrtPtType* CubeDim,
                                     int SkipFactor,
                                     double SamplingFactor,
                                     double IsoVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *MCExtractIsoSurface3(IPObjectStruct *ImageList,
                                     IrtPtType* CubeDim,
                                     int SkipFactor,
                                     double IsoVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MCExtractIsoSurfaceClean();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivLoadVolumeIntoTV( byte *FileName,
                                   int DataType,
                                   IrtVecType* VolSize,
                                   IrtVecType* Orders);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MCImprovePointOnIsoSrfPrelude( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MCImprovePointOnIsoSrfPostlude();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MCImprovePointOnIsoSrf(IrtPtType* Pt,
                            IrtPtType* CubeDim,
                           double IsoVal,
                           double Tolerance,
                           double AllowedError);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *TrivCoverIsoSurfaceUsingStrokes(TrivTVStruct *TV,
                                               int NumStrokes,
                                               int StrokeType,
                                               IrtPtType* MinMaxPwrLen,
                                               double StepSize,
                                               double IsoVal,
                                               IrtVecType* ViewDir);
    }
}
