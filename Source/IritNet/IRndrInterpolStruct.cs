namespace IritNet
{
    public unsafe struct IRndrInterpolStruct
    {
        public double w, z;                           /* Homogenous and Z coordinate. */
        public double u, v;               /* Bivariative texture mapping coordinates. */
        public IrtNrmlType n;             /* Normal at the current interpolation point. */
        public IrtPtType c;                /* Color at the current interpolation point. */
        public IRndrIntensivityStruct *i;      /* Array of intens. for every light src. */
        public int IntensSize;
        public int HasColor;
    }
}