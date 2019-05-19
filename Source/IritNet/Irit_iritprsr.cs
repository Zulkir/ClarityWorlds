using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void IPSetErrorFuncType (IPFatalErrorType ErrorFunc);
    public unsafe delegate void IPPrintFuncType ( byte *PrintFunc);
    public unsafe delegate void IPProcessLeafObjType (IPObjectStruct *PObj);
    public unsafe delegate void IPStreamReadCharFuncType (int Handler);
    public unsafe delegate void IPStreamWriteBlockFuncType (int Handler,
                                                  void * Block,
                                                  int Size);
    public unsafe delegate void IPApplyObjFuncType (IPObjectStruct *PObj, IrtHmgnMatType* Mat);
    public unsafe delegate void IPApplyObjFunc2Type (IPObjectStruct *PObj,
                                            IrtHmgnMatType* Mat,
                                            void *Data);
    public unsafe delegate void IPNCGCodeRectangleToolSweepFuncType (IrtPtType* Pt1,
                                                            IrtPtType* Pt2,
                                                            IrtPtType* Pt3,
                                                            IrtPtType* Pt4);
    public unsafe delegate void IPNCGCodeEvalMRRFuncType (void * Data);
    public unsafe delegate void IPNCGCodeParserErrorFuncType (byte *Line);
    public unsafe delegate void IPNCGCodeIndexUpdateFuncType ();
    public unsafe delegate void IPForEachObjCallBack (IPObjectStruct *PObj, 
                                                        void *Param);
    public unsafe delegate void IPForEachPolyCallBack (IPPolygonStruct *Pl, 
                                                           void *Param);
    public unsafe delegate void IPForEachVertexCallBack (IPVertexStruct *V, 
                                                           void *Param);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPIgesLoadFileSetDefaultParameters(int ClipTrimmedSrf,
                                        int DumpAll,
                                        int IgnoreGrouping,
                                        int ApproxCoversion,
                                        int InverseProjCrvOnSrfs,
                                        int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPIgesLoadFile( byte *IgesFileName,
                                IPIgesLoadDfltFileParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPIgesSaveFile( IPObjectStruct *PObj,
                   IrtHmgnMatType* CrntViewMat,
                    byte *IgesFileName,
                   int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPIgesSaveEucTrimCrvs(int SaveEucTrimCrvs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPSTLLoadFileSetDefaultParameters(int BinarySTL,
                                       int EndianSwap,
                                       int NormalFlip,
                                       int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPSTLLoadFile( byte *STLFileName,
                               IPSTLLoadDfltFileParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSTLSaveFile( IPObjectStruct *PObj,
                  IrtHmgnMatType* CrntViewMat,
                  int RegularTriang,
                  int MultiObjSplit,
                   byte *STLFileName,
                  int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPSTLSaveSetVrtxEps(double SameVrtxEps);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPOBJLoadFileSetDefaultParameters(int WarningMsgs,
                                       int WhiteDiffuseTexture,
                                       int IgnoreFullTransp,
                                       int ForceSmoothing);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPOBJLoadFile( byte *OBJFileName,
                               IPOBJLoadDfltFileParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPOBJSaveFile( IPObjectStruct *PObj, 
                   byte *OBJFileName,
                  int WarningMsgs,
                  int UniqueVertices);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPDXFSaveFile( IPObjectStruct *PObj,
                   byte *DXFFileName,
                  int DumpFreeForms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPOpenVrmlFile( byte *FileName, int Messages, double Resolution);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPutVrmlObject(int Handler, IPObjectStruct *PObj, int Indent);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPutVrmlViewPoint(int Handler, IrtHmgnMatType* Mat, int Indent);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetVrmlExternalMode(int On);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPNCGCodeSaveFile( IPObjectStruct *PObj,
                      IrtHmgnMatType* CrntViewMat,
                       byte *NCGCODEFileName,
                      int Messages,
                      int Units);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPNCGCodeSaveFileSetTol(double Tol);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPNCGCodeLoadFileSetDefaultParameters(int ArcCentersRelative,
                                           int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPNCGCodeLoadFile( byte *NCGCODEFileName,
                                 IPGcodeLoadDfltFileParamsStruct *Params);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IPNCGCodeParserInit(int ArcCentersRelative,
                            double DefFeedRate,
                            double DefSpindleSpeed,
                            int DefToolNumber,
                            int ReverseZDir,
                            IPNCGCodeParserErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void * IPNCGCodeParserParseLine(void * IPNCGCodes,
                                  byte *NextLine,
                                 int LineNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPNCGCodeParserDone(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPNCGCodeParserNumSteps(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPNCGCodeLineStruct *IPNCGCodeParserSetStep(void * IPNCGCodes,
                                                int NewStep);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPNCGCodeLineStruct *IPNCGCodeParserGetNext(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPNCGCodeLineStruct *IPNCGCodeParserGetPrev(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPNCGCodeParserFree(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPNCGCode2Geometry(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPNCGCodeLength(void * IPNCGCodes, double *FastLength);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  GMBBBboxStruct *IPNCGCodeBBox(void * IPNCGCodes, int IgnoreG0Fast);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPNCGCodeTraverseInit(void * IPNCGCodes,
                               double InitTime,
                               double FastSpeedUpFactor,
                               double TriggerArcLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPNCGCodeTraverseTriggerAAL(void * IPNCGCodes,
                                IPNCGCodeEvalMRRFuncType EvalMRR,
                                void * MRRData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPNCGCodeTraverseTime(void * IPNCGCodes,
                               double Dt,
                               double *NewRealTime,
                               IrtPtType* NewToolPosition,
                               IPNCGCodeLineStruct **NewGC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double IPNCGCodeTraverseStep(void * IPNCGCodes,
                               double Step,
                               double *NewRealTime,
                               IrtPtType* NewToolPosition,
                               IPNCGCodeLineStruct **NewGC);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *IPNCGCodeGenToolGeom(IPNCGCToolType ToolType,
                                    double Diameter,
                                    double Height,
                                    double TorusRadius,
                                    CagdCrvStruct **ToolProfile,
                                    CagdSrfStruct **ToolBottom);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *IPNCUpdateCrvOffsetJoint(CagdCrvStruct *OrigCrv1,
                                        CagdCrvStruct *OrigCrv2,
                                        CagdCrvStruct **OffCrv1,
                                        CagdCrvStruct **OffCrv2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPNCGCodeSave2File(void * IPNCGCodes,  byte *FName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IPNCGCodeTraverseLines(void * IPNCGCodes, int Restart);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPNCGCodeResetFeedRates(void * IPNCGCodes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPNCGCodeIndexUpdateFuncType IPNCGCodeUpdateGCodeIndexCBFunc(
                                            void * IPNCGCodes,
                                           IPNCGCodeIndexUpdateFuncType Func);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPPutMatrixFile( byte *File,
                    IrtHmgnMatType* ViewMat,
                    IrtHmgnMatType* ProjMat,
                    int HasProjMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPOpenDataFile( byte *FileName, int Read, int Messages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPOpenStreamFromCallBackIO(IPStreamReadCharFuncType ReadFunc,
                               IPStreamWriteBlockFuncType WriteFunc,
                               int Read,
                               int IsBinary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPOpenStreamFromSocket(int Soc, int IsBinary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPCloseStream(int Handler, int Free);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetDataFiles(byte  *  *DataFileNames,
                               int NumOfDataFiles,
                               int Messages,
                               int MoreMessages);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetObjects(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPResolveInstances(IPObjectStruct *PObjects);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPStreamFormatType IPSenseFileType( byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSenseBinaryFile( byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPProcessReadObject(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPFlattenTree(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPFlattenTreeProcessFF(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPFlattenForrest(IPObjectStruct *PObjList, int ProcessFF);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPStdoutObject( IPObjectStruct *PObj, int IsBinary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPStderrObject( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPExportObjectToFile( byte *FName,
                           IPObjectStruct *PObj,
                          IPStreamFormatType FType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPutObjectToFile3( byte *FName,
                         IPObjectStruct *PObj,
                        int Indent);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPutObjectToHandler(int Handler,  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPInputUnGetC(int Handler, byte c);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetPolyListCirc(int Circ);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetFlattenObjects(int Flatten);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetPropagateAttrs(int Propagate);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPFlattenInvisibleObjects(int FlattenInvisib);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetReadOneObject(int OneObject);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPProcessLeafObjType IPSetProcessLeafFunc(IPProcessLeafObjType ProcessLeafFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPrintFuncType IPSetPrintFunc(IPPrintFuncType PrintFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSetFilterDegen(int FilterDegeneracies);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IPSetFloatFormat( byte *FloatFormat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPGetRealNumber( byte *StrNum, double *RealNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPGetMatrixFile( byte *File,
                    IrtHmgnMatType* ViewMat,
                    IrtHmgnMatType* ProjMat,
                    int *HasProjMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetBinObject(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPutBinObject(int Handler,  IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPProcessFreeForm(IPFreeFormStruct *FreeForms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPConcatFreeForm(IPFreeFormStruct *FreeForms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPEvalFreeForms(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPProcessModel2TrimSrfs(IPFreeFormStruct *FreeForms);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtHmgnMatType* IPGetViewMat(int *WasViewMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IrtHmgnMatType* IPGetPrspMat(int *WasPrspMat);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPUpdatePolyPlane(IPPolygonStruct *PPoly);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPUpdatePolyPlane2(IPPolygonStruct *PPoly,  IrtVecType* Vin);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPUpdateVrtxNrml(IPPolygonStruct *PPoly, IrtVecType* DefNrml);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPReverseListObj(IPObjectStruct *ListObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPReverseObjList(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPReversePlList(IPPolygonStruct *PPl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPReverseVrtxList(IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPReverseVrtxList2(IPVertexStruct *PVrtx);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetObjectByName( byte *Name,
                                  IPObjectStruct *PObjList,
                                  int TopLevel);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPSetSubObjectName(IPObjectStruct *PListObj,
                        int Index,
                         byte *Name);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetLastObj(IPObjectStruct *OList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPGetPrevObj(IPObjectStruct *OList, IPObjectStruct *O);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPAppendObjLists(IPObjectStruct *OList1,
                                 IPObjectStruct *OList2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPAppendListObjects(IPObjectStruct *ListObj1,
                                    IPObjectStruct *ListObj2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPObjLnkListToListObject(IPObjectStruct *ObjLnkList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPLnkListToListObject(void * LnkList,
                                      IPObjStructType ObjType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPLinkedListToObjList( IPObjectStruct *LnkList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void *IPListObjToLinkedList( IPObjectStruct *LObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPListObjToLinkedList2( IPObjectStruct *LObjs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPGetLastPoly(IPPolygonStruct *PList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPGetPrevPoly(IPPolygonStruct *PList,
                               IPPolygonStruct *P);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPAppendPolyLists(IPPolygonStruct *PList1,
                                   IPPolygonStruct *PList2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPGetLastVrtx(IPVertexStruct *VList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPGetPrevVrtx(IPVertexStruct *VList, IPVertexStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPAppendVrtxLists(IPVertexStruct *VList1,
                                  IPVertexStruct *VList2);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPObjListLen( IPObjectStruct *O);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPPolyListLen( IPPolygonStruct *P);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPVrtxListLen( IPVertexStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPForEachObj2(IPObjectStruct *OList, 
                              IPForEachObjCallBack CallBack,
                              void *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPForEachPoly2(IPPolygonStruct *PlList, 
                                IPForEachPolyCallBack CallBack,
                                void *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPForEachVertex2(IPVertexStruct *VList, 
                                 IPForEachVertexCallBack CallBack,
                                 void *Param);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPTraverseObjectCopy(int TraverseObjCopy);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPTraverseObjectAll(int TraverseObjAll);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPTraverseInvisibleObject(int TraverseInvObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPTraverseObjListHierarchy(IPObjectStruct *PObjList,
                                IrtHmgnMatType* CrntViewMat,
                                IPApplyObjFuncType ApplyFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPTraverseObjHierarchy(IPObjectStruct *PObj,
                            IPObjectStruct *PObjList,
                            IPApplyObjFuncType ApplyFunc,
                            IrtHmgnMatType* Mat,
                            int PrntInstance);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPTraverseObjListHierarchy2(IPObjectStruct *PObjList,
                                 IrtHmgnMatType* CrntViewMat,
                                 IPApplyObjFunc2Type ApplyFunc,
                                 void *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPTraverseObjHierarchy2(IPObjectStruct *PObj,
                             IPObjectStruct *PObjList,
                             IPApplyObjFunc2Type ApplyFunc,
                             IrtHmgnMatType* Mat,
                             int PrntInstance,
                             void *Data);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceGregoryToBezier( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceBezierToPower( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoercePowerToBezier( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceBezierToBspline( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceBsplineToBezier( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceTrimmedSrfToTrimmedBezier( IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceTrimmedSrfToUnTrimmedBezier( IPObjectStruct *PObj,
                                                    int ComposeE3);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPointType IPCoerceCommonSpace(IPObjectStruct *PtObjList,
                                  CagdPointType Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdPointType IPCoercePtsListTo(IPObjectStruct *PtObjList, CagdPointType Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceObjectPtTypeTo( IPObjectStruct *PObj,
                                       int NewType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPCoerceObjectTo( IPObjectStruct *PObj, int NewType);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPReverseObject(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocWriteBlock(int Handler, void *Block, int BlockLen);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPSocWriteOneObject(int Handler, IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocReadCharNonBlock(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte *IPSocReadLineNonBlock(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IPSocReadOneObject(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocSrvrInit();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocSrvrListen();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPSocHandleClientEvent(int Handler, IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocClntInit();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocExecAndConnect( byte *Program, int IsBinary);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSocDisConnectAndKill(int Kill, int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPSocEchoInput(int Handler, int EchoInput);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPCnvDataToIrit( IPObjectStruct *PObjects);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPCnvDataToIritOneObject( byte *Indent,
                               IPObjectStruct *PObject,
                              int Level);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPCnvDataToIritAttribs( byte *Indent,
                             byte *ObjName,
                             IPAttributeStruct *Attr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IPCnvrtReal2Str(double R);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPrintFuncType IPCnvSetPrintFunc(IPPrintFuncType CnvPrintFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCnvSetLeastSquaresFit(int MinLenFit, int Percent, double MaxError);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte IPCnvSetDelimitChar(byte Delimit);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCnvSetCompactList(int CompactList);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCnvSetDumpAssignName(int DumpAssignName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *IPCnvPolyVrtxNeighbors( IPPolyVrtxIdxStruct *PVIdx, int VIdx, int Ring);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPVertexStruct *IPCnvFindAdjacentEdge( IPPolyVrtxIdxStruct *PVIdx, 
                                      int ThisPolyIdx,
                                      int FirstVertexIndex, 
                                      int SecondVertexIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolygonStruct *IPCnvFindAdjacentPoly( IPPolyVrtxIdxStruct *PVIdx,
                                        IPVertexStruct *V,
                                        IPVertexStruct *VNext);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCnvIsVertexBoundary( IPPolyVrtxIdxStruct *PVIdx, int VertexIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCnvEstimateBndryVrtxPlaneNrml( IPPolyVrtxIdxStruct *PVIdx,
                                    int BndryVrtxIdx,
                                    IrtVecType* PlaneNrml);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPPolyVrtxIdxStruct *IPCnvPolyToPolyVrtxIdxStruct( IPObjectStruct *PObj,
                                                  int CalcPPolys,
                                                  int AttribMask);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvReadFromFile( byte *FileName,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *CagdCrvReadFromFile2(int Handler, byte **ErrStr, int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvWriteToFile( CagdCrvStruct *Crvs,
                        byte *FileName,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdCrvWriteToFile2( CagdCrvStruct *Crvs,
                        int Handler,
                        int Indent,
                         byte *Comment,
                        byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfReadFromFile( byte *FileName,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *CagdSrfReadFromFile2(int Handler, byte **ErrStr, int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfWriteToFile( CagdSrfStruct *Srfs,
                        byte *FileName,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CagdSrfWriteToFile2( CagdSrfStruct *Srfs,
                        int Handler,
                        int Indent,
                         byte *Comment,
                        byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvReadFromFile( byte *FileName,
                                  byte **ErrStr,
                                  int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BzrCrvReadFromFile2(int Handler,
                                   int NameWasRead,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrCrvWriteToFile( CagdCrvStruct *Crvs,
                       byte *FileName,
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrCrvWriteToFile2( CagdCrvStruct *Crvs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfReadFromFile( byte *FileName,
                                  byte **ErrStr,
                                  int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BzrSrfReadFromFile2(int Handler,
                                   int NameWasRead,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrSrfWriteToFile( CagdSrfStruct *Srfs,
                       byte *FileName,
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BzrSrfWriteToFile2( CagdSrfStruct *Srfs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvReadFromFile( byte *FileName,
                                  byte **ErrStr,
                                  int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdCrvStruct *BspCrvReadFromFile2(int Handler,
                                   int NameWasRead,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvWriteToFile( CagdCrvStruct *Crvs,
                       byte *FileName,
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspCrvWriteToFile2( CagdCrvStruct *Crvs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfReadFromFile( byte *FileName,
                                  byte **ErrStr,
                                  int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *BspSrfReadFromFile2(int Handler,
                                   int NameWasRead,
                                   byte **ErrStr,
                                   int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfWriteToFile( CagdSrfStruct *Srfs,
                       byte *FileName,
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BspSrfWriteToFile2( CagdSrfStruct *Srfs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVReadFromFile( byte *FileName,
                                 byte **ErrStr,
                                 int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivTVReadFromFile2(int Handler, byte **ErrStr, int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVReadFromFile( byte *FileName,
                                    byte **ErrStr,
                                    int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBzrTVReadFromFile2(int Handler,
                                     int NameWasRead,
                                     byte **ErrStr,
                                     int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVReadFromFile( byte *FileName,
                                    byte **ErrStr,
                                    int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivBspTVReadFromFile2(int Handler,
                                     int NameWasRead,
                                     byte **ErrStr,
                                     int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVWriteToFile( TrivTVStruct *TVs,
                       byte *FileName, 
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivTVWriteToFile2( TrivTVStruct *TVs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBzrTVWriteToFile( TrivTVStruct *TVs,
                          byte *FileName,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBzrTVWriteToFile2( TrivTVStruct *TVs,
                          int Handler,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBspTVWriteToFile( TrivTVStruct *TVs,
                          byte *FileName,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivBspTVWriteToFile2( TrivTVStruct *TVs,
                          int Handler,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimReadTrimmedSrfFromFile( byte *FileName,
                                          byte **ErrStr,
                                          int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrimSrfStruct *TrimReadTrimmedSrfFromFile2(int Handler,
                                           int NameWasRead,
                                           byte **ErrStr,
                                           int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimWriteTrimmedSrfToFile( TrimSrfStruct *TrimSrfs,
                               byte *FileName,
                              int Indent,
                               byte *Comment,
                              byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrimWriteTrimmedSrfToFile2( TrimSrfStruct *TrimSrfs,
                               int Handler,
                               int Indent,
                                byte *Comment,
                               byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfReadFromFile( byte *FileName,
                                            byte **ErrStr,
                                            int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngTriSrfReadFromFile2(int Handler,
                                             byte **ErrStr,
                                             int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBzrTriSrfReadFromFile( byte *FileName,
                                               byte **ErrStr,
                                               int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBzrTriSrfReadFromFile2(int Handler,
                                                int NameWasRead,
                                                byte **ErrStr,
                                                int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBspTriSrfReadFromFile( byte *FileName,
                                               byte **ErrStr,
                                               int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngBspTriSrfReadFromFile2(int Handler,
                                                int NameWasRead,
                                                byte **ErrStr,
                                                int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngGrgTriSrfReadFromFile( byte *FileName,
                                               byte **ErrStr,
                                               int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrngTriangSrfStruct *TrngGrgTriSrfReadFromFile2(int Handler,
                                                int NameWasRead,
                                                byte **ErrStr,
                                                int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngTriSrfWriteToFile( TrngTriangSrfStruct *TriSrfs,
                           byte *FileName,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngTriSrfWriteToFile2( TrngTriangSrfStruct *TriSrfs,
                           int Handler,
                           int Indent,
                            byte *Comment,
                           byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngBzrTriSrfWriteToFile( TrngTriangSrfStruct *TriSrfs,
                              byte *FileName,
                             int Indent,
                              byte *Comment,
                             byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngBzrTriSrfWriteToFile2( TrngTriangSrfStruct *TriSrfs,
                              int Handler,
                              int Indent,
                               byte *Comment,
                              byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngBspTriSrfWriteToFile( TrngTriangSrfStruct *TriSrfs,
                              byte *FileName,
                             int Indent,
                              byte *Comment,
                             byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngBspTriSrfWriteToFile2( TrngTriangSrfStruct *TriSrfs,
                              int Handler,
                              int Indent,
                               byte *Comment,
                              byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngGrgTriSrfWriteToFile( TrngTriangSrfStruct *TriSrfs,
                              byte *FileName,
                             int Indent,
                              byte *Comment,
                             byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrngGrgTriSrfWriteToFile2( TrngTriangSrfStruct *TriSrfs,
                              int Handler,
                              int Indent,
                               byte *Comment,
                              byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlReadModelFromFile( byte *FileName,
                                     byte **ErrStr,
                                     int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MdlModelStruct *MdlReadModelFromFile2(int Handler,
                                      int NameWasRead,
                                      byte **ErrStr,
                                      int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlWriteModelToFile( MdlModelStruct *Models,
                         byte *FileName,
                        int Indent,
                         byte *Comment,
                        byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MdlWriteModelToFile2( MdlModelStruct *Models,
                         int Handler,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlReadModelFromFile( byte *FileName,
                                        byte **ErrStr,
                                        int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern VMdlVModelStruct *VMdlReadModelFromFile2(int Handler,
                                         int NameWasRead,
                                         byte **ErrStr,
                                         int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlWriteModelToFile( VMdlVModelStruct *VModels,
                          byte *FileName,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMdlWriteModelToFile2( VMdlVModelStruct *VModels,
                          int Handler,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVReadFromFile( byte *FileName,
                                 byte **ErrStr,
                                 int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarMVReadFromFile2(int Handler, byte **ErrStr, int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVReadFromFile( byte *FileName,
                                    byte **ErrStr,
                                    int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBzrMVReadFromFile2(int Handler,
                                     int NameWasRead,
                                     byte **ErrStr,
                                     int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVReadFromFile( byte *FileName,
                                    byte **ErrStr,
                                    int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern MvarMVStruct *MvarBspMVReadFromFile2(int Handler,
                                     int NameWasRead,
                                     byte **ErrStr,
                                     int *ErrLine);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVWriteToFile( MvarMVStruct *MVs,
                       byte *FileName,
                      int Indent,
                       byte *Comment,
                      byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarMVWriteToFile2( MvarMVStruct *MVs,
                       int Handler,
                       int Indent,
                        byte *Comment,
                       byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBzrMVWriteToFile( MvarMVStruct *MVs,
                          byte *FileName,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBzrMVWriteToFile2( MvarMVStruct *MVs,
                          int Handler,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVWriteToFile( MvarMVStruct *MVs,
                          byte *FileName,
                         int Indent,
                          byte *Comment,
                         byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int MvarBspMVWriteToFile2( MvarMVStruct *MVs,
                          int Handler,
                          int Indent,
                           byte *Comment,
                          byte **ErrStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IpcSetQuantization(int Handler, float QntError);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IpcCompressObjToFile( byte *FileName,
                         IPObjectStruct *PObj,
                         float QntError);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IpcCompressObj(int Handler, IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IpcDecompressObjFromFile( byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *IpcDecompressObj(int Handler);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPSenseCompressedFile( byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPSetErrorFuncType IPSetFatalErrorFunc(IPSetErrorFuncType ErrorFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPHasError( byte **ErrorDesc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *IPDescribeError(IPFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPFatalError(IPFatalErrorType ErrorNum);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPDbg();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPVertexDbg(IPVertexStruct *V);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPPolygonDbg(IPPolygonStruct *Pl);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IPDbgDisplayObject(IPObjectStruct *PObj);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AttrDbg( IPAttributeStruct *Attr);
    }
}
