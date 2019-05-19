namespace IritNet
{
    public struct IRndrLightStruct
    {
        public int Type;                                          /* Light source type. */
        public IrtPtType Where;                     /* Light source position or vector. */
        public IRndrColorType Color;                      /* Color of the light source. */
    }
}