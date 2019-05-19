namespace IritNet
{
    public unsafe struct IRndrZPointStruct
    {
        public IRndrZPointStruct *Next; /* Link to next z-point at same location.*/
        public IRndrPixelType Color;
        public float z;
        public float Transp;                         /* Transparancy factor. */
        public IPPolygonStruct* Triangle;     /* The triangle which created this point. */
    }
}