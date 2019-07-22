namespace IritNet
{
    public unsafe struct TrivIGAAdjacencyInfoStruct
    {
        public  TrivTVStruct* AdjTV;
        public int AdjBndryIdx;   /* 0 to 5 for Umin, Umax, Vmin, Vmax, Wmin, Wmax. */
        public int ReverseU, ReverseV, ReverseUwithV;
        public int SameSpace;
    }
}
