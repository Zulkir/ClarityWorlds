namespace IritNet
{
    public unsafe struct UserGCPreDefinedObsPtSuggestionsStruct
    {
        public struct PolyhedronStruct
        {
            public IPObjectStruct* Poly;
            public int Level;             /* Number of times to divid each polygon. */
        }

        public struct PerimeterStruct
        {
            /* See UserGCSetSuggestionPointsFromPerimeter */
            public byte* CrfFileName;
            public int GuardsNumber;          /* Number of guards on the perimeter. */
            public double GuardsHeight;  /* The height of the guard above the     */
            /* surface.                              */
        }

        public struct SurfaceGridStruct
        {
            /* See UserGCSetSuggestionPointsFromSurfaceGrid */
            public byte* SrfFileName;
            public fixed int GuardsNumber[2];          /* Number of guards on U and on V. */
            public double GuardsHeight;  /* The height of the guard above the     */
            /* surface.                              */
        }

        public UserGCPredefinedSuggestionsType PredefinedSuggestionType;
        public int AddAntipodal;

        public PolyhedronStruct Polyhedron { get { var loc = SurfaceGrid; return *(PolyhedronStruct*)&loc; } set { var loc = this; *(PolyhedronStruct*)&loc = value; this = loc; } }
        public PerimeterStruct Perimeter { get { var loc = SurfaceGrid; return *(PerimeterStruct*)&loc; } set { var loc = this; *(PerimeterStruct*)&loc = value; this = loc; } }
        public SurfaceGridStruct SurfaceGrid;
    }
}