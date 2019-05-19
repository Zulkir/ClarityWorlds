namespace IritNet
{
    public struct IRndrSceneStruct
    {
        public int SizeX;
        public int SizeY;
        public IRndrMatrixContextStruct Matrices;
        public IRndrLightListStruct Lights;
        public IRndrColorType BackgroundColor;
        public double Ambient;
        public int ShadeModel;			  /* Type of shading model to apply. */
        public int BackFace;	       /* Flag directing to remove back faced flats. */
        public double ZNear;                                      /* Clipping planes. */
        public double ZFar;
        public int ZFarValid;
        public double XMin, XMax, YMin, YMax;
    }
}