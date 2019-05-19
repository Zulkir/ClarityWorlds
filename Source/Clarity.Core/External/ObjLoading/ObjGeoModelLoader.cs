using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;
using ObjLoader.Loader.Loaders;

namespace Clarity.Core.External.ObjLoading
{
    // todo: to multifile loader (materials)
    public class ObjGeoModelLoader : IAssetLoader
    {
        private struct VertexMediator
        {
            public int PositionIndex;
            public int NormalIndex;
            public int TexCoordIndex;
            public int? NextMediator;
        }

        private static readonly string[] SupportedExtensions = {".obj"};

        public string Name => "CGGC.OBJ";
        public string AssetTypeString => " 3D Model files (OBJ) (ObjLoader)";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            try
            {
                var asset = BuildAsset(loadInfo);
                return AssetLoadResultByLoader.Success(asset);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure("EXCEPTION", ex);
            }
        }

        private unsafe IAsset BuildAsset(AssetLoadInfo loadInfo)
        {
            var fileData = loadInfo.FileSystem.ReadAllBytes(loadInfo.LoadPath);

            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create(new MyMaterialStreamProvider(Path.GetDirectoryName(loadInfo.LoadPath), loadInfo.FileSystem));
            LoadResult objLoadResult;
            using (var stream = new MemoryStream(fileData))
                objLoadResult = objLoader.Load(stream);

            var anyTextures = objLoadResult.Textures.Any();
            var initialMediatorIndices = new int?[objLoadResult.Vertices.Count];
            var mediators = new List<VertexMediator>();
            var faceMediatorIndices = new List<int>();
            var indexList = new List<int>();
            var materials = new List<StandardMaterial>();
            var modelParts = new List<FlexibleModelPart>();
            
            foreach (var grp in objLoadResult.Groups)
            {
                var startIndexLocation = indexList.Count;
                foreach (var face in grp.Faces)
                {
                    faceMediatorIndices.Clear();
                    foreach (var fv in face.EnumerateVertices())
                    {
                        var newMediator = new VertexMediator
                        {
                            PositionIndex = fv.VertexIndex - 1,
                            NormalIndex = fv.NormalIndex - 1,
                            TexCoordIndex = anyTextures ? fv.TextureIndex - 1 : 0
                        };
                        var initialIndex = initialMediatorIndices[newMediator.PositionIndex];
                        if (!initialIndex.HasValue)
                        {
                            var mediatorIndex = mediators.Count;
                            mediators.Add(newMediator);
                            faceMediatorIndices.Add(mediatorIndex);
                            initialMediatorIndices[newMediator.PositionIndex] = mediatorIndex;
                        }
                        else
                        {
                            var mediatorIndex = initialIndex.Value;
                            var oldMediator = mediators[mediatorIndex];
                            while (true)
                            {
                                if (oldMediator.PositionIndex == newMediator.PositionIndex &&
                                    oldMediator.NormalIndex == newMediator.NormalIndex &&
                                    oldMediator.TexCoordIndex == newMediator.TexCoordIndex)
                                {
                                    break;
                                }
                                if (!oldMediator.NextMediator.HasValue)
                                {
                                    mediatorIndex = mediators.Count;
                                    mediators.Add(newMediator);
                                    oldMediator.NextMediator = mediatorIndex;
                                    break;
                                }
                                mediatorIndex = oldMediator.NextMediator.Value;
                                oldMediator = mediators[mediatorIndex];
                            }
                            faceMediatorIndices.Add(mediatorIndex);
                        }
                    }
                    if (faceMediatorIndices.Count <= 3)
                    {
                        for (int i = 0; i < faceMediatorIndices.Count - 3; i++)
                            indexList.Add(faceMediatorIndices[0]);
                        foreach (var mi in faceMediatorIndices)
                            indexList.Add(mi);
                    }
                    else
                    {
                        for (int i = 0; i < faceMediatorIndices.Count - 2; i++)
                        {
                            indexList.Add(faceMediatorIndices[0]);
                            indexList.Add(faceMediatorIndices[i + 1]);
                            indexList.Add(faceMediatorIndices[i + 2]);
                        }
                    }
                }
                var indexCount = indexList.Count - startIndexLocation;
                var color = grp.Material != null
                    ? new Color4(
                        grp.Material.DiffuseColor.X,
                        grp.Material.DiffuseColor.Y,
                        grp.Material.DiffuseColor.Z)
                    : (Color4?)null;
                
                // todo: use material

                modelParts.Add(new FlexibleModelPart
                {
                    VertexSetIndex = 0,
                    ModelMaterialName = grp.Material?.Name ?? "DefaultMaterial",
                    PrimitiveTopology = FlexibleModelPrimitiveTopology.TriangleList,
                    IndexCount = indexCount,
                    FirstIndex = startIndexLocation
                });
            }

            CgVertexPosNormTex[] finalVertices;
            int[] finalIndices;

            if (objLoadResult.Normals.Count > 0)
            {
                var vertices = mediators.Select(m => new CgVertexPosNormTex()).ToArray();
                for (int i = 0; i < mediators.Count; i++)
                {
                    var mediator = mediators[i];
                    vertices[i].Position = objLoadResult.Vertices[mediator.PositionIndex].ToClarity();
                    vertices[i].Normal = objLoadResult.Normals[mediator.NormalIndex].ToClarity();
                    if (anyTextures)
                        vertices[i].TexCoord = objLoadResult.Textures[mediator.TexCoordIndex].ToClarity();
                }

                finalVertices = vertices;
                finalIndices = indexList.ToArray();
            }
            else
            {
                var newIndices = Enumerable.Range(0, indexList.Count).ToArray();
                var newVertices = new CgVertexPosNormTex[newIndices.Length];
                for (int i = 0; i + 2 < indexList.Count; i += 3)
                {
                    var mediator0 = mediators[indexList[i]];
                    var mediator1 = mediators[indexList[i + 1]];
                    var mediator2 = mediators[indexList[i + 2]];
                    var pos0 = objLoadResult.Vertices[mediator0.PositionIndex].ToClarity();
                    var pos1 = objLoadResult.Vertices[mediator1.PositionIndex].ToClarity();
                    var pos2 = objLoadResult.Vertices[mediator2.PositionIndex].ToClarity();
                    newVertices[i] = new CgVertexPosNormTex
                    {
                        Position = pos0,
                        Normal = -Vector3.Cross(pos2 - pos0, pos1 - pos0).Normalize(),
                        TexCoord = new Vector2(0.3f, 0.3f)
                    };
                    newVertices[i + 1] = new CgVertexPosNormTex
                    {
                        Position = pos1,
                        Normal = -Vector3.Cross(pos0 - pos1, pos2 - pos1).Normalize(),
                        TexCoord = new Vector2(0.7f, 0.3f)
                    };
                    newVertices[i + 2] = new CgVertexPosNormTex
                    {
                        Position = pos2,
                        Normal = -Vector3.Cross(pos1 - pos2, pos0 - pos2).Normalize(),
                        TexCoord = new Vector2(0.5f, 0.7f)
                    };
                }

                finalVertices = newVertices;
                finalIndices = newIndices;
            }

            var pack = new ResourcePack(ResourceVolatility.Immutable);

            IRawDataResource vertexRawData;
            fixed (CgVertexPosNormTex* pVertices = finalVertices)
                vertexRawData = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pVertices, sizeof(CgVertexPosNormTex) * finalVertices.Length);
            pack.AddSubresource("VertexRawData", vertexRawData);

            IRawDataResource indexRawData;
            fixed (int* pIndices = finalIndices)
                indexRawData = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pIndices, sizeof(int) * finalIndices.Length);
            pack.AddSubresource("IndexRawData", indexRawData);

            var arraySubranges = new []
            {
                vertexRawData.GetSubrange(0),
                indexRawData.GetSubrange(0)
            };

            var elementInfos = CgVertexPosNormTex.GetElementsInfos(0);
            var indicesInfo = new VertexIndicesInfo(1, CommonFormat.R32_UINT);
            var vertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, arraySubranges, elementInfos, indicesInfo);
            pack.AddSubresource("VertexSet", vertexSet);

            var radiusSq = finalVertices.Max(x => x.Position.LengthSquared());
            var radius = MathHelper.Sqrt(radiusSq);
            var model = new FlexibleModel(ResourceVolatility.Immutable, new [] {vertexSet}, modelParts, radius);
            pack.AddSubresource("Model", model);

            var hash = AssetHashMd5.FromSingleFile(fileData);
            var fileName = Path.GetFileName(loadInfo.LoadPath);
            return new Asset(loadInfo.AssetName, pack, AssetStorageType.CopyLocal, hash, loadInfo.ReferencePath, fileName);
        }
    }
}