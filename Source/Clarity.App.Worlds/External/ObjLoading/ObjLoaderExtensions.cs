using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals.Algebra;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;

namespace Clarity.App.Worlds.External.ObjLoading
{
    public static class ObjLoaderExtensions
    {
        public static Vector3 ToClarity(this Vertex v) =>
            new Vector3(v.X, v.Y, v.Z);

        public static Vector3 ToClarity(this Normal n) =>
            new Vector3(n.X, n.Y, n.Z);

        public static Vector2 ToClarity(this Texture t) =>
            new Vector2(t.X, t.Y);

        public static IEnumerable<FaceVertex> EnumerateVertices(this Face face) => 
            Enumerable.Range(0, face.Count).Select(x => face[x]);
    }
}