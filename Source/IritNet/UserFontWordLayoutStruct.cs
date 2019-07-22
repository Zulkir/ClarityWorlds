using System;
namespace IritNet
{
    public unsafe struct UserFontWordLayoutStruct
    {
        public  UserFontWordLayoutStruct* Pnext;
        public IntPtr Word;
        public byte* FontName;
        public UserFontStyleType FontStyle;
        public double RelSize;                         /* Relative scale to the text. */
        public UserFont3DEdgeType Font3DEdge;
        public IrtPtType Font3DOptions;
        public UserFontAlignmentType FontAlignment;
        public  IPObjectStruct* Geom;      /* The geometry representing the text. */
        public GMBBBboxStruct BBox;       /* BBox of Geom, ignoring (X, Y) translation. */
        public double X, Y;                                           /* Word position. */
        public double LeftOverSpace;  /* For last word in line only.  Otherwise zero. */
        public byte NewLine;                              /* A new line after this word. */
    }
}
