namespace IritNet
{
    public unsafe struct TrivIGASeedingStateStruct
    {
        public fixed double AlphaVal[3];
        public fixed int NumOfIntervals[3];
        public fixed double DmnMin[3], DmnMax[3];
    }
}