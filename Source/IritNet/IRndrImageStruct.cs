namespace IritNet
{
    public unsafe struct IRndrImageStruct
    {
        public struct Union
        {
            IrtImgPixelStruct* RGB;
            private IrtImgRGBAPxlStruct* RGBA { get { return (IrtImgRGBAPxlStruct*)RGB; } set { RGB = (IrtImgPixelStruct*)value; } }
        }

        public int xSize, ySize, Alpha;
        public Union U;
    }
}