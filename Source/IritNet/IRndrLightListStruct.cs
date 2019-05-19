namespace IritNet
{
    public unsafe struct IRndrLightListStruct
    {
        public int n;                                       /* Number of light sources. */
        public IRndrLightStruct *Src;                 /* Array of light source objects. */
    }
}