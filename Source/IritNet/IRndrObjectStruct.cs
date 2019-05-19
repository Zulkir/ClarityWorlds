namespace IritNet
{
    public unsafe struct IRndrObjectStruct
    {
        public int Power;              /* Power of cosine factor of specular component. */
        public double KSpecular;                       /* Specular coefficient value. */
        public double KDiffuse;                         /* Diffuse coefficient value. */
        public IRndrColorType Color;                            /* Color of the object. */
        public IRndrTextureStruct Txtr;
        public double Transp;                          /* Transparency of the object. */
        public int noShade;                            /* Pure color model (polylines). */
        public IPObjectStruct* OriginalIritObject;
        public IrtHmgnMatType AnimationMatrix;
        public int Transformed;
        public int Animated;
        public int DoVisMapCalcs;         /* If calculations are done over UV triangle. */
    }
}