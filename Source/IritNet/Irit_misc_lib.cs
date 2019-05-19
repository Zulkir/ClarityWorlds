using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void IritE2TExprNodeParamFuncType ( byte *ParamName);
    public unsafe delegate void IritFatalMsgFuncType ( byte *Msg);
    public unsafe delegate void IritWarningMsgFuncType ( byte *Msg);
    public unsafe delegate void IritInfoMsgFuncType ( byte *Msg);
    public unsafe delegate void MiscSetErrorFuncType (MiscFatalErrorType ErrorFunc,
                                              byte *ErrorDescription);
    public unsafe delegate void IritPQCompFuncType (void * p1, void * p2);
    public unsafe delegate void IritHashCmpFuncType (void * Data1, void * Data2);
    public unsafe delegate void MiscHashFuncType (void * Elem, 
                                                   int ElementParam, 
                                                   int KeySizeBits);
    public unsafe delegate void MiscHashCopyFuncType (void *Elem,  int ElementParam);
    public unsafe delegate void MiscHashFreeFuncType (void *Elem);
    public unsafe delegate void MiscHashCompFuncType (void *Elem1, 
                                            void *Elem2, 
                                             int ElementParam);
    public unsafe delegate void MiscListCopyFuncType (void *elmt,  int size);
    public unsafe delegate void MiscListFreeFuncType (void *elmt);
    public unsafe delegate void MiscListCompFuncType (void *e1mt1, void *elmt2,  int l);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritMalloc( int Size,
                    byte *ObjType,
                    byte *FileName,
                   int LineNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritFree(void * p);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritDynMemoryDbgTestAll();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritDynMemoryDbgInitTest();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritDynMemoryDbgInitTest2(int DebugMalloc, int DebugSearchAllocID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritDynMemoryDbgCheckMark(double *Start,
                               double *KeepStackStep,
                               double *TrackAllocID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritDynMemoryDbgMallocSearchID(int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *IritDynMemoryDbgNoReport(void *p);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritRealloc(void * p,  int OldSize,  int NewSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritFree2UnixFree(void *p);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IritConfig( byte *PrgmName,
                        IritConfigStruct *SetUp,
                       int NumVar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritConfigPrint( IritConfigStruct *SetUp, int NumVar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritConfigSave( byte *FileName,
                    IritConfigStruct *SetUp,
                   int NumVar);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *GAStringErrMsg(int Error, byte *OutStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GAPrintErrMsg(int Error);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *GAStringHowTo( byte *CtrlStr, byte *OutStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GAPrintHowTo( byte *CtrlStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenUnitMat(IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatIsUnitMatrix(IrtHmgnMatType* Mat, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatIsWeightAffected(IrtHmgnMatType* Mat, double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatTrans(double Tx, double Ty, double Tz, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatUnifScale(double Scale, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatScale(double Sx, double Sy, double Sz, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotX1(double Teta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotX(double CosTeta, double SinTeta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotY1(double Teta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotY(double CosTeta, double SinTeta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotZ1(double Teta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGenMatRotZ(double CosTeta, double SinTeta, IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMultTwo4by4(IrtHmgnMatType* MatRes,
                    IrtHmgnMatType* Mat1,
                    IrtHmgnMatType* Mat2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatAddTwo4by4(IrtHmgnMatType* MatRes,
                   IrtHmgnMatType* Mat1,
                   IrtHmgnMatType* Mat2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatSubTwo4by4(IrtHmgnMatType* MatRes,
                   IrtHmgnMatType* Mat1,
                   IrtHmgnMatType* Mat2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatScale4by4(IrtHmgnMatType* MatRes,
                  IrtHmgnMatType* Mat,
                   double *Scale);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatSameTwo4by4(IrtHmgnMatType* Mat1,
                   IrtHmgnMatType* Mat2,
                   double Eps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMultVecby4by4(IrtVecType* VecRes,
                       IrtVecType* Vec,
                      IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatMultPtby4by4(IrtPtType* PtRes,
                      IrtPtType* Pt,
                     IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MatDeterminantMatrix(IrtHmgnMatType* Mat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatInverseMatrix(IrtHmgnMatType* M, IrtHmgnMatType* InvM);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatTranspMatrix(IrtHmgnMatType* M, IrtHmgnMatType* TranspM);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MatScaleFactorMatrix(IrtHmgnMatType* M);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  double *MatScaleFactorMatrix2(IrtHmgnMatType* M);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatRotateFactorMatrix(IrtHmgnMatType* M, IrtHmgnMatType* RotMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatRotSclFactorMatrix(IrtHmgnMatType* M, IrtHmgnMatType* RotSclMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatTranslateFactorMatrix(IrtHmgnMatType* M, IrtVecType* Trans);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlCopy(double* Dst, double* Src, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlUnitMat(double* Mat, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatGnrlIsUnitMatrix(double* Mat, double Eps, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlMultTwoMat(double* MatRes,
                       double* Mat1,
                       double* Mat2,
                       int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlAddTwoMat(double* MatRes,
                      double* Mat1,
                      double* Mat2,
                      int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlSubTwoMat(double* MatRes,
                      double* Mat1,
                      double* Mat2,
                      int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlScaleMat(double* MatRes,
                     double* Mat,
                     double *Scale,
                     int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlMultVecbyMat(double* VecRes,
                         double* Mat,
                        double* Vec,
                         int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlMultVecbyMat2(double* VecRes,
                        double* Vec,
                          double* Mat,
                          int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatGnrlInverseMatrix(double* M,
                         double* InvM,
                         int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatGnrlTranspMatrix(double* M,
                         double* TranspM,
                         int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double MatGnrlDetMatrix(double* M, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MatGnrlOrthogonalSubspace(double* M, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mat2x2Determinant(double a11,
                           double a12, 
                           double a21,
                           double a22);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mat3x3Determinant(double a11,
                           double a12,
                           double a13,
                           double a21,
                           double a22,
                           double a23,
                           double a31,
                           double a32,
                           double a33);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritQRFactorization(double *A,
                        int n,
                        int m,
                        double *Q,
                        double *R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritSolveUpperDiagMatrix( double *A,
                             int n,
                              double *b,
                             double *x);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritSolveLowerDiagMatrix( double *A,
                             int n,
                              double *b,
                             double *x);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritQRUnderdetermined(double *A,
                          double *x,
                           double *b,
                          int m,
                          int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritGaussJordan(double *A, double *B,  int N,  int M);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  int IritLevenMarSetMaxIterations( int NewVal);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritPQInit(IritPriorQue **PQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritPQEmpty(IritPriorQue *PQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritPQCompFunc(IritPQCompFuncType NewCompFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritPQFirst(IritPriorQue **PQ, int Delete);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritPQInsert(IritPriorQue **PQ, void * NewItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritPQDelete(IritPriorQue **PQ, void * NewItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritPQFind(IritPriorQue *PQ, void * OldItem);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritPQNext(IritPriorQue *PQ, void * CmpItem, void * BiggerThan);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritPQSize(IritPriorQue *PQ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritPQFree(IritPriorQue *PQ, int FreeItems);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritHashTableStruct *IritHashTableCreate(double MinKeyVal,
                                         double MaxKeyVal,
                                         double KeyEps,
                                         int VecSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritHashTableInsert(IritHashTableStruct *IHT,
                        void * Data,
                        IritHashCmpFuncType HashCmpFunc,
                        double Key,
                        int RplcSame);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritHashTableFind(IritHashTableStruct *IHT,
                          void * Data,
                          IritHashCmpFuncType HashCmpFunc,
                          double Key);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritHashTableRemove(IritHashTableStruct *IHT,
                        void * Data,
                        IritHashCmpFuncType HashCmpFunc,
                        double Key);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritHashTableFree(IritHashTableStruct *IHT);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MiscListStruct* MiscListNewEmptyList(MiscListCopyFuncType CopyFunc,
                                     MiscListFreeFuncType FreeFunc,
                                     MiscListCompFuncType CompFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiscListCompLists(MiscListStruct* L1, MiscListStruct* L2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiscListFreeList(MiscListStruct* List);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MiscListIteratorStruct* MiscListGetListIterator(MiscListStruct* List);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiscListFreeListIterator(MiscListIteratorStruct* It);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MiscListIteratorFirst(MiscListIteratorStruct* It);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MiscListIteratorNext(MiscListIteratorStruct* It);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiscListIteratorAtEnd(MiscListIteratorStruct* It);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *MiscListIteratorValue(MiscListIteratorStruct* It);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgReadImageXAlign(int Alignment);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgReadImage( byte *ImageFileName,
                                   int *MaxX,
                                   int *MaxY,
                                   int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgReadImage2( byte *ImageFileName,
                                    int *MaxX,
                                    int *MaxY,
                                    int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgReadImage3( byte *ImageFileName,
                                    int *MaxX,
                                    int *MaxY,
                                    int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgGetImageSize( byte *ImageFileName,
                       int *Width,
                       int *Height);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgReadUpdateCache( byte *ImageFileName,
                          int MaxX,
                          int MaxY,
                          int Alpha,
                          byte *Image);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IrtImgReadClrCache();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IrtImgReadClrOneImage( byte *ImageName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgImageType IrtImgWriteSetType( byte *ImageType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgWriteOpenFile( byte **argv,
                         byte *FName,
                        int Alpha,
                        int XSize,
                        int YSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IrtImgWritePutLine(byte *Alpha, IrtImgPixelStruct *Pixels);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IrtImgWriteCloseFile();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgFlipXYImage( IrtImgPixelStruct *Img,
                                     int MaxX,
                                     int MaxY,
                                     int Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgNegateImage( IrtImgPixelStruct *InImage,
                                     int MaxX,
                                     int MaxY);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgFlipHorizontallyImage( IrtImgPixelStruct *Img,
                                               int MaxX,
                                               int MaxY,
                                               int Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct *IrtImgFlipVerticallyImage( IrtImgPixelStruct *Img,
                                             int MaxX,
                                             int MaxY,
                                             int Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgParsePTextureString( byte *PTexture,
                              byte *FName,
                              double *Scale,
                              int *Flip,
                              int *NewImage);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgParsePTextureString2( byte *PTexture,
                              byte *FName,
                              double *Scale,
                              int *Flip,
                              int *NewImage,
                              int *FlipHorizontally,
                              int *FlipVertically);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IrtImgDitherImage(IrtImgPixelStruct *Image,
                            int XSize,
                            int YSize,
                            int DitherSize,
                            byte ErrorDiffusion);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtImgDitherImage2( byte *InputImage,
                        byte *OututImage,
                       int DitherSize,
                       byte ErrorDiffusion);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct **IrtMovieReadMovie( byte *MovieFileName,
                                      int *MaxX,
                                      int *MaxY,
                                      int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct **IrtMovieReadMovie2( byte *MovieFileName,
                                       int *MaxX,
                                       int *MaxY,
                                       int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtImgPixelStruct **IrtMovieReadMovie3( byte *MovieFileName,
                                       int *MaxX,
                                       int *MaxY,
                                       int *Alpha);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtMovieGetMovieProps( byte *MovieFileName,
                          int *Width,
                          int *Height);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IrtMovieReadClrCache();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IrtMovieParsePMovieString( byte *PMovie,
                              byte *FName,
                              double *Scale,
                              int *NewImage,
                              int *Flip,
                              int *Restart,
                              double *TimeSetup,
                              int *FlipHorizontally,
                              int *FlipVertically);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritSearch2DInit(double XMin,
                         double XMax,
                         double YMin,
                         double YMax,
                         double Tol,
                         int DataSize);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritSearch2DFree(void * S2D);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritSearch2DInsertElem(void * S2D,
                            double XKey,
                            double YKey,
                            void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritSearch2DFindElem(void * S2D,
                         double XKey,
                         double YKey,
                         void * Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IritRLNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritRLAdd(void * RLC, double l, double r, int attr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *IritRLFindCyclicCover(void * RLC, double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritRLDelete(void * RLC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritRLSetGaurdiansNumber(int g);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MiscBiPrWeightedMatchBipartite( double **Weight,
                                   IritBiPrWeightedMatchStruct *Match,
                                   int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritE2TExprNodeParamFuncType IritE2Expt2TreeSetFetchParamValueFunc
                           (IritE2TExprNodeParamFuncType FetchParamValueFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritE2TCmpTree( IritE2TExprNodeStruct *Root1,
                    IritE2TExprNodeStruct *Root2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritE2TExprNodeStruct *IritE2TCopyTree( IritE2TExprNodeStruct *Root);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritE2TExprNodeStruct *IritE2TDerivTree( IritE2TExprNodeStruct *Root,
                                        int Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritE2TPrintTree( IritE2TExprNodeStruct *Root, byte *Str);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IritE2TEvalTree( IritE2TExprNodeStruct *Root);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritE2TFreeTree(IritE2TExprNodeStruct *Root);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritE2TParamInTree( IritE2TExprNodeStruct *Root,
                        byte *ParamName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritE2TSetParamValue(double Value, int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritE2TParseError();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritE2TDerivError();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IritStrdup( byte *s);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IritStrUpper(byte *s);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IritStrLower(byte *s);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritSleep(int MiliSeconds);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IritRandom(double Min, double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritPseudoRandomInit( int Seed);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IritPseudoRandom();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IritCPUTime(int Reset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IritRealTimeDate();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IritApproxStrStrMatch( byte *Str1,
                                byte *Str2,
                               int IgnoreCase);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void movmem(void * Src, void * Dest, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *searchpath( byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int strnicmp( byte *s1,  byte *s2, int n);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int stricmp( byte *s1,  byte *s2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IritStrIStr( byte *s,  byte *Pattern);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IritSubstStr( byte *S,
                    byte *Src,
                    byte *Dst,
                   int CaseInsensitive);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *strstr( byte *s,  byte *Pattern);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *getcwd(byte *s, int Len);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MiscSetErrorFuncType MiscSetFatalErrorFunc(MiscSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiscFatalError(MiscFatalErrorType ErrID, byte *ErrDesc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *MiscDescribeError(MiscFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IritLineHasCntrlChar( byte *Line);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritFatalMsgFuncType IritSetFatalErrorFunc(IritFatalMsgFuncType FatalMsgFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritFatalError( byte *Msg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritWarningMsgFuncType IritSetWarningMsgFunc(IritWarningMsgFuncType WrnMsgFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritWarningMsg( byte *Msg);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IritInfoMsgFuncType IritSetInfoMsgFunc(IritInfoMsgFuncType InfoMsgFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IritInformationMsg( byte *Msg);
    }
}
