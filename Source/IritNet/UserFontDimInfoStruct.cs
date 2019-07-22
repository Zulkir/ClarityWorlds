namespace IritNet
{
    public unsafe struct UserFontDimInfoStruct
    {
        public double DescentLineHeight;     /* The four height lines of this font. */
        public double BaseLineHeight;
        public double MeanLineHeight;
        public double AscentLineHeight;
        public double SpaceWidth;              /* The estimated space width to use. */
        public GMBBBboxStruct BBox;   /* Of a generic byte 's' in this font/size etc. */
    }
}
