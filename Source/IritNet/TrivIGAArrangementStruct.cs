namespace IritNet
{
    public unsafe struct TrivIGAArrangementStruct
    {
        public TrivIGAFieldStruct *Fields;  /* List of IGA fields (a few trivariates). */
        public TrivIGAErrorType LastError;
        public int UniqueGlblCtlPtIDMax;
        public TrivIGAMaterialStructPtrArray16 Materials;
        public int NumMaterials;
        public TrivIGABoundaryNodeStruct *BoundaryNodes;
        public TrivIGASeedingStateStruct SeedingState;
    }
}