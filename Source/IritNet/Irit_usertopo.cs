using System.Runtime.InteropServices;

namespace IritNet
{
    public unsafe delegate void UserTopoFilterGridCBFuncType  (
                                                 UserTopoUnstrctGeomStruct *UG);
    public static unsafe partial class Irit
    {
        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoUnstrctGeomNew();

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserTopoUnstrctGeomPtCopyData(UserTopoUnstrctGeomPtStruct *Dest,
                                    UserTopoUnstrctGeomPtStruct *Src);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserTopoUnstrctGeomFree(UserTopoUnstrctGeomStruct *Ud);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserTopoSetPoints(UserTopoUnstrctGeomStruct *Ud,
                       UserTopoUnstrctGeomPtStruct *Pts,
                       int NumPt,
                       int MergePts,
                       int **IndxMap,
                       int **RealIDMap);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoAddPoints(
                                        UserTopoUnstrctGeomStruct *Ud,
                                        UserTopoUnstrctGeomPtStruct *Pts,
                                       int NumPt,
                                       int MergePts,
                                       int **CloneMap,
                                       int **RealIDMap);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoAddCell(UserTopoUnstrctGeomStruct *Ud,
                           int *PtIdVec,
                          int PtIdVecLen,
                          IPObjectStruct *Cell,
                          int *CellID);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoModifyPoint(UserTopoUnstrctGeomStruct *Ud,
                        int PtId,
                         UserTopoUnstrctGeomPtStruct *Pt);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointIntAttr(UserTopoUnstrctGeomStruct *Ud,
                            int PtId,
                            byte *AttrName,
                            int AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointIntAttrVec(UserTopoUnstrctGeomStruct *Ud,
                               int *PtIdVec,
                               int NumPtId,
                               byte *AttrName,
                               int *AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointRealAttr(UserTopoUnstrctGeomStruct *Ud,
                             int PtId,
                             byte *AttrName,
                             double AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointRealAttrVec(UserTopoUnstrctGeomStruct *Ud,
                                int *PtIdVec,
                                int NumPtId,
                                byte *AttrName,
                                double *AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointStrAttr(UserTopoUnstrctGeomStruct *Ud,
                            int PtId,
                            byte *AttrName,
                            byte* AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetPointStrAttrVec(UserTopoUnstrctGeomStruct *Ud,
                               int *PtIdVec,
                               int NumPtId,
                               byte *AttrName,
                               byte **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellIntAttr(UserTopoUnstrctGeomStruct *Ud,
                             IPObjectStruct *Cell,
                             byte *AttrName,
                             int AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellIntAttrVec(UserTopoUnstrctGeomStruct *Ud,
                              int *CellIdVec,
                              int NumCellId,
                              byte *AttrName,
                              int *AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellRealAttr(UserTopoUnstrctGeomStruct *Ud,
                            IPObjectStruct *Cell,
                            byte *AttrName,
                            double AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellRealAttrVec(UserTopoUnstrctGeomStruct *Ud,
                              int *CellIdVec,
                               int NumCellId,
                               byte *AttrName,
                               double *AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellStrAttr(UserTopoUnstrctGeomStruct *Ud,
                           IPObjectStruct *Cell,
                           byte *AttrName,
                           byte *AttrValue);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoSetCellStrAttrVec(UserTopoUnstrctGeomStruct *Ud,
                              int *CellIdVec,
                              int NumCellId,
                              byte *AttrName,
                              byte **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoAppendUnstrctGeoms(
                                         UserTopoUnstrctGeomStruct *UdA,
                                         UserTopoUnstrctGeomStruct *UdB,
                                        double Eps,
                                        int MergePts,
                                        int **CloneMap,
                                        int **RealIDMap);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UserTopoUnstrctGeomUpdate(UserTopoUnstrctGeomStruct **Ud,
                               double Eps,
                               int PurgeUnusedPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoObjectToId( UserTopoUnstrctGeomStruct *Ud,
                        IPObjectStruct *Cell);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IPObjectStruct *UserTopoIdToObject( UserTopoUnstrctGeomStruct *Ud,
                                   int Id);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetPointIntAttr( UserTopoUnstrctGeomStruct *Ud,
                            int PtId,
                            byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetPointIntAttrVec(UserTopoUnstrctGeomStruct *Ud,
                               int *PtIdVec,
                               int NumPtId,
                               byte *AttrName,
                               int **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserTopoGetPointRealAttr( UserTopoUnstrctGeomStruct *Ud,
                                   int PtId,
                                   byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetPointRealAttrVec(UserTopoUnstrctGeomStruct *Ud,
                                int *PtIdVec,
                                int NumPtId,
                                byte *AttrName,
                                double **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *UserTopoGetPointStrAttr( UserTopoUnstrctGeomStruct *Ud,
                                    int PtId,
                                    byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetPointStrAttrVec(UserTopoUnstrctGeomStruct *Ud,
                               int *PtIdVec,
                               int NumPtId,
                               byte *AttrName,
                                byte ***AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetCellIntAttr( UserTopoUnstrctGeomStruct *Ud,
                           int CellId,
                           byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetCellIntAttrVec(UserTopoUnstrctGeomStruct *Ud,
                              int *CellIdVec,
                              int NumCellId,
                               byte *AttrName,
                              int **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double UserTopoGetCellRealAttr( UserTopoUnstrctGeomStruct *Ud,
                                  int CellId,
                                  byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetCellRealAttrVec(UserTopoUnstrctGeomStruct *Ud,
                               int *CellIdVec,
                               int NumCellId,
                               byte *AttrName,
                               double **AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  byte *UserTopoGetCellStrAttr( UserTopoUnstrctGeomStruct *Ud,
                                   int CellId,
                                   byte *AttrName);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetCellStrAttrVec(UserTopoUnstrctGeomStruct *Ud,
                              int *CellIdVec,
                              int NumCellId,
                              byte *AttrName,
                               byte ***AttrValueVec);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoPtsOfCell( UserTopoUnstrctGeomStruct *Ud,
                        int EntId,
                        int **PtIds);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoAllEntitiesWithPoint( UserTopoUnstrctGeomStruct *Ud,
                                 int PtId,
                                 int **EntIds);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoNumOfEntOfType( UserTopoUnstrctGeomStruct *Ud,
                           IPObjStructType Type);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoCellsAdjacentToCell( UserTopoUnstrctGeomStruct *Ud,
                                int CellID,
                                int **EntIDs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetCellAttrThreshold( UserTopoUnstrctGeomStruct *Ud,
                                   byte *AttrName,
                                   int AttrMinVal,
                                   int AttrMaxVal,
                                   int **EntIDs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int UserTopoGetPointAttrThreshold( UserTopoUnstrctGeomStruct *Ud,
                                  byte *AttrName,
                                  int AttrMinVal,
                                  int AttrMaxVal,
                                  int **PtIDs);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoApplyFilterToGrid(
                                           UserTopoUnstrctGeomStruct *Ud,
                                          int PurgeUnusedPts);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoCrvBndryFilter(
                                           UserTopoUnstrctGeomStruct *Ud);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoSrfBndryFilter(
                                           UserTopoUnstrctGeomStruct *Ud);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomStruct *UserTopoTrivBndryFilter(
                                           UserTopoUnstrctGeomStruct *Ud);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoFilterGridCBFuncType UserTopoSetFilterGridCallBackFunc(
                                         UserTopoFilterGridCBFuncType NewFunc);

        [DllImport("Irit.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UserTopoUnstrctGeomReturnStruct *UserTopoUnstrctGeomMain(
                                      UserTopoUnGridOpType OperationID,
                                      UserTopoUnstrctGeomParamStruct *Params);
    }
}
