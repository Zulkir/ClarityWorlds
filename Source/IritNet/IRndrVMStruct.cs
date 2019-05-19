namespace IritNet
{
    public unsafe struct IRndrVMStruct
    {
        public int SizeU, SizeV, TargetSizeU, TargetSizeV, SuperSize,
            DilationItarations, InvScreenMatValid;

        /* Minimum and maximum values of UV coordinates of the objects. */
        public float MaxU, MinU, MaxV, MinV;
        public double CosTanAng, CriticAR;

        /* Whether the current object should be scanned on UV space or just on   */
        /* XYZ space.                                                            */
        public int IsScanOnUV;
        /* Whether to use backface culling in UV scan conversion. */
        public int UVBackfaceCulling;

        /* Matrix accumulate UV information for UV map output. */
        public IRndrUVListStruct** UVMap;
        public IRndrSceneStruct* Scene;
    }
}