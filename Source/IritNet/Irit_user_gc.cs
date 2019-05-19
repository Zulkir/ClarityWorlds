using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void UserGCLoadRgbImageFromFileFuncType 
                                                                ( byte* FileName, 
                                                                 int *Width, 
                                                                 int *Height);
    public unsafe delegate void UserGCSaveRgbImageToFileFuncType ( byte *FileName, 
                                                        IrtImgPixelStruct *VisMap, 
                                                        int Width, 
                                                        int Height);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserGCSolveGeoProblem(UserGCProblemDefinitionStruct *Problem,
                          UserGCSolutionIndexStruct *** SolutionOps,
                          double *CoverPercentage);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *UserGCLoadVisMap(UserGCProblemDefinitionStruct *Problem, 
                                    int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserGCExposeCreatePrspMatrix(double ZAngle, 
                                  double XYAngle,
                                  IrtHmgnMatType* PrspMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserGCExposeCreateViewMatrix( UserGCObsPtSuggestionStruct *Op,
                                  IrtHmgnMatType* ViewMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserGCExposeCreateViewMatrix2( UserGCObsPtSuggestionStruct *Op,
                                   IrtHmgnMatType* ViewMat,
                                    IrtVecType* Up);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserGCExposeDivideOP(UserGCObsPtSuggestionStruct *ObsPt,
                          double OpeningInXY, 
                          double OpeningInZ,
                          int *ObsPtsNum,
                          UserGCObsPtSuggestionStruct *ObsPts,
                          double *OpeningOutXY,
                          double *OpeningOutZ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserGCExposePrepareScene(UserGCProblemDefinitionStruct *Problem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserGCExposeInterpretOPGroupsSuggestion(
                                       UserGCProblemDefinitionStruct *Problem);
    }
}
