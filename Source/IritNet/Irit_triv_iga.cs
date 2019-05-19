using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void TrivIGANeighboringConstraintCallBackType (
                                                     TrivIGAArrangementStruct *H,
                                                    int IsRational,
                                                    int MaxCoord,
                                                     int *FirstIDs,
                                                    int FirstIDsSize,
                                                    int SecondID,
                                                    int SecondIdx,
                                                     CagdSrfStruct **Mat,
                                                    void *CallBackData);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGANewArrangement(int *NewArgmntID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGANewField(int ArgmntID,
                     byte *FieldAttributes);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGATVStruct *TrivIGANewTV(int ArgmntID,
                                     TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGATVStruct *TrivIGAUpdateTV(int ArgmntID,
                                        TrivTVStruct *ExistingTV,
                                        TrivTVStruct *NewTV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAArrangementComplete(int ArgmntID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAPrintTVContent(int ArgmntID,
                           TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetGlblMaxIDs(int ArgmntID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetCtlPtIDRange(int ArgmntID,
                             TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAGetNumBzrElements(int ArgmntID,
                              TrivTVStruct *TV,
                             int *NumU,
                             int *NumV,
                             int *NumW);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivIGACtrlPtStruct *TrivIGAGetBzrElementCtrlPts(
                                           int ArgmntID,
                                            TrivTVStruct *TV,
                                           int IndexU,
                                           int IndexV,
                                           int IndexW);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  double *TrivIGAGetKnotInterval(int ArgmntID,
                                         TrivTVStruct *TV,
                                        TrivTVDirType Dir,
                                        int BzrIntervalIndex);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAUpdateCtrlPtsPositions(int ArgmntID,
                                  int NumCtrlPts,
                                   TrivIGACtrlPtStruct *DeltaVals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGASetCtrlPtsPositions(int ArgmntID,
                               int NumCtrlPts,
                                TrivIGACtrlPtStruct *Vals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGACtrlPtStruct *TrivIGATVEval(int ArgmntID,
                                          TrivTVStruct *TV,
                                         TrivIGAEvalType EvalType,
                                         int IndexU,
                                         int IndexV,
                                         int IndexW,
                                         double U,
                                         double V,
                                         double W);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  double *TrivIGATVEvalBasis(int ArgmntID,
                                     TrivTVStruct *TV,
                                    TrivIGAEvalType EvalType,
                                    TrivTVDirType Dir,
                                    int Index,
                                    double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAFreeArrangement(int ArgmntID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGASetDefaultSeeding(int ArgmntID,
                             TrivTVDirType Dir,
                             double Alpha,
                             int NumIntervals);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGASetDefaultDomain(int ArgmntID,
                            TrivTVDirType Dir,
                            double Min,
                            double Max);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAAddTrivar(int ArgmntID,
                              TrivTVStruct *TV,
                             int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAExtrudeTV(int ArgmntID, 
                              CagdSrfStruct *Srf,
                              IrtVecType* Vec,
                             int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAExtrudeTV2(int ArgmntID,
                               CagdSrfStruct *Srf,
                               CagdCrvStruct *Crv,
                              int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGATVofRevol(int ArgmntID,
                              CagdSrfStruct *Srf,
                              IrtPtType* AxisPoint,
                              IrtVecType* AxisVector,
                             double StartAngleRad, 
                             double EndAngleRads,
                             int IsRational,
                             int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGATVFromSurfaces(int ArgmntID,
                                   CagdSrfStruct *SrfList,
                                  int OtherOrder,
                                  int IsInterpolating,
                                  int ID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGATVStruct *TrivIGATVRefine(int ArgmntID,
                                        int TVID,
                                        TrivTVDirType Dir,
                                        double t);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGATVStruct *TrivIGATDegreeRaise(int ArgmntID,
                                            int TVID,
                                            TrivTVDirType Dir);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGADataManagerGetTrivID( TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivIGADataManagerGetTrivariate(int TVID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  TrivIGATVStruct *TrivIGADataManagerGetIGATrivariate(int TVID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAGetFaceNeighboringTVs(int ArgmntID,
                                  TrivTVStruct *TV,
                                 TrivIGAAdjacencyInfoStruct *AdjInfo);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetEdgeNeighboringTVs(int ArgmntID,
                                   TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetVrtxNeighboringTVs(int ArgmntID,
                                   TrivTVStruct *TV);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetAllTVs(int ArgmntID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivTVStruct *TrivIGAGetTV(int ArgmntID, int TVID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetTVFaceCtlPtsIDs(int ArgmntID,
                               int TVID,
                               int FaceID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern CagdSrfStruct *TrivIGAGetTVFaceAsSrf(int ArgmntID,
                                     int TVID,
                                     int FaceID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetTVCtlPtsIndices(int ArgmntID,
                               int TVID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  CagdCtlPtStruct *TrivIGAGetCtlPt(int ArgmntID,
                                       int Index);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAGetMaterial(int ArgmntID,
                                     int TVID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TrivIGAGenNeighboringConstraints(int ArgmntID,
                                      void *CallbackData,
                                      TrivIGANeighboringConstraintCallBackType
                                               NeighboringConstraintCallBack);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIgaGenOneFaceNeighboringConstraints(
                                     int ArgmntID,
                                     TrivIGANeighboringConstraintCallBackType
                                               NeighboringConstraintCallBack,
                                      TrivTVStruct *TV1,
                                     int FaceID1,
                                      TrivTVStruct *TV2,
                                     int FaceID2,
                                     void *CallbackData);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGANewMaterial(int ArgmntID,
                                      byte *MaterialStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivIGAMaterialStruct *TrivIGAParseMaterial( byte *MaterialStr);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAAddMaterial(int ArgmntID,
                                     TrivIGAMaterialStruct *Material);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGALoadMaterialXML( byte *FileName,
                           TrivIGAMaterialStruct **Materials,
                           int *NumMaterials);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAExportToXML(int ArgmntID,
                        byte *FileName,
                        byte *TemplateFileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGALoadMaterialFromXML(int ArgmntID,
                                byte *FileName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAAddBoundaryFace(int ArgmntID,
                            TrivTVStruct *TV,
                           TrivTVBndryType BoundaryType,
                           TrivIGANodeBoundaryType NodeBoundaryType,
                            byte *BoundaryAxisConditions,
                           double Value);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAAddBoundaryFace2(int ArgmntID,
                             TrivTVStruct *TV,
                            TrivTVBndryType BoundaryType,
                            TrivIGANodeBoundaryType NodeBoundaryType,
                             byte *BoundaryAxisConditions,
                            double Value);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TrivIGAAddBoundaryFaceByPt(int ArgmntID,
                                TrivTVStruct *TV,
                                IrtPtType* Pt,
                               TrivIGANodeBoundaryType NodeBoundary,
                                byte *BoundaryAxisConditions,
                               double Value);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int *TrivIGAGetBoundaryFaceByPt(int ArgmntID,
                                 TrivTVStruct *TV,
                                 IrtPtType* Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TrivIGAErrorType TrivIGAGetLastError(int ArgmntID,
                                     int Reset);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *TrivIGADescribeError(TrivIGAErrorType ErrorNum);
    }
}
