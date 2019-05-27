using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Clarity.App.Worlds.Assets;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Clarity.Engine.Visualization.Graphics;
using IritNet;

namespace Clarity.Ext.Format.Itd
{
    public unsafe class ItdModelLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = {".itd"};

        public string Name => "CGGC.ITD";
        public string AssetTypeString => $"3D Model files (Irit) (${Name})";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }
        
        private readonly object callbackLock = new object();

        private readonly List<FlexibleModelPart> modelParts;
        private readonly List<VertexPosNormTex> vertices;
        private readonly List<int> indices;

        private const int TRUE = 1;
        private const int FALSE = 0;
        private static readonly void* NULL = (void*)0;

        public ItdModelLoader()
        {
            modelParts = new List<FlexibleModelPart>();
            vertices = new List<VertexPosNormTex>();
            indices = new List<int>();
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
                return AssetLoadResultByLoader.Failure(ex.GetType().Name, ex);
            }
        }

        private IAsset BuildAsset(AssetLoadInfo loadInfo)
        {
            // todo: load from memory

            /* Get the data files: */
            Irit.IPSetFlattenObjects(0);
            
            var pFileName = Marshal.StringToHGlobalAnsi(loadInfo.LoadPath);
            var pObjects = Irit.IPGetDataFiles((byte**)&pFileName, 1, TRUE, FALSE);
            Marshal.FreeHGlobal(pFileName);
            if (pObjects == NULL)
                throw new Exception($"Failed to load '{loadInfo.LoadPath}' as an ITD model.");

            pObjects = Irit.IPResolveInstances(pObjects);
            
            var viewMat = new IrtHmgnMatType
            {
                [0, 0] = 1,
                [1, 1] = 1,
                [2, 2] = 1,
                [3, 3] = 1
            };

            lock (callbackLock)
            {
                modelParts.Clear();
                vertices.Clear();
                indices.Clear();

                Irit.IPTraverseObjListHierarchy(pObjects, &viewMat, ExtractTraversed);

                var pack = new ResourcePack(ResourceVolatility.Immutable);

                RawDataResource vertexDataRes;
                var vertexArray = vertices
                    .Select(x => new VertexPosNormTex(x.Position.Y, x.Position.Z, x.Position.X, x.Normal.Y, x.Normal.Z, x.Normal.X, x.TexCoord.X, x.TexCoord.Y))
                    .ToArray();

                var boundingSphere = Sphere.BoundingSphere(vertexArray.Select(x => x.Position).ToList());
                // todo: remove (no longer necessary to centralize the model)
                //for (int i = 0; i < vertices.Count; i++)
                //    vertexArray[i].Position -= boundingSphere.Center;

                fixed (VertexPosNormTex* pVertices = vertexArray)
                    vertexDataRes = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pVertices, vertexArray.Length * sizeof(VertexPosNormTex));
                pack.AddSubresource("VertexArray", vertexDataRes);

                RawDataResource indexDataRes;
                var indiexArray = indices.ToArray();
                fixed (int* pIndices = indiexArray)
                    indexDataRes = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pIndices, indiexArray.Length * sizeof(int));
                pack.AddSubresource("IndexArray", indexDataRes);

                var arraySubranges = new[]
                {
                    vertexDataRes.GetSubrange(0),
                    indexDataRes.GetSubrange(0)
                };

                var elementInfos = VertexPosNormTex.GetElementsInfos(0);
                var indicesInfo = new VertexIndicesInfo(1, CommonFormat.R32_UINT);

                var vertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, arraySubranges, elementInfos, indicesInfo);
                pack.AddSubresource("VertexSet", vertexSet);

                var model = new FlexibleModel(ResourceVolatility.Immutable, new []{vertexSet}, modelParts, boundingSphere);
                pack.AddSubresource("Model", model);

                AssetHashMd5 hash;
                using (var stream = loadInfo.FileSystem.OpenRead(loadInfo.LoadPath))
                    hash = AssetHashMd5.FromSingleFile(stream);
                var fileName = Path.GetFileName(loadInfo.LoadPath);
                return new Asset(loadInfo.AssetName, pack, AssetStorageType.ReferenceOriginal, hash, loadInfo.ReferencePath, fileName);
            }
        }

        private void ExtractTraversed(IPObjectStruct* pObj, IrtHmgnMatType* mat)
        {
            IPObjectStruct* pObjs;

            if (Irit.IP_IS_FFGEOM_OBJ(pObj))
            {
                var ffcv = new IPFreeformConvStateStruct
                {
                    DrawFFGeom = 1,
                    CrvApproxTolSamples = 64,
                    FineNess = 20,
                    ComputeNrml = 1,
                    LinearOnePolyFlag = 1,

                    ComputeUV = 1,
                    FourPerFlat = 1,
                };
                ffcv.NumOfIsolines[0] = 10;
                ffcv.NumOfIsolines[1] = 10;
                ffcv.NumOfIsolines[2] = 10;
                pObjs = Irit.IPConvertFreeForm(pObj, &ffcv); /* Convert in place. */
            }
            else
            {
                pObjs = pObj;
            }

            for (pObj = pObjs; pObj != NULL; pObj = pObj->Pnext)
                ExtractMesh(pObj);
        }

        private void ExtractMesh(IPObjectStruct* pObj)
        {
            if (pObj->ObjType != IPObjStructType.IP_OBJ_POLY)
                return;

            var pVeridix = Irit.IPCnvPolyToPolyVrtxIdxStruct(pObj, FALSE, 7);

            var isPolygon = Irit.IP_IS_POLYGON_OBJ(pObj);

            var vertexOffset = vertices.Count;

            for (var ppVertex = pVeridix->Vertices; *ppVertex != NULL; ppVertex++)
            {
                var clVertex = IritToClarity.Convert(*ppVertex);
                vertices.Add(clVertex);
            }

            var firstIndex = indices.Count;
            var polyIndices = new List<int>();
            
            for (var ppPoly = pVeridix->Polygons; *ppPoly != NULL; ppPoly++)
            {
                var pPoly = *ppPoly;
                polyIndices.Clear();
                
                do
                {
                    var iritIndex = *pPoly++;
                    polyIndices.Add(iritIndex + vertexOffset);
                }
                while (*pPoly >= 0);

                if (isPolygon)
                {
                    if (polyIndices.Count == 3)
                    {
                        indices.AddRange(polyIndices);
                    }
                    else if (polyIndices.Count < 3)
                    {
                        indices.AddRange(polyIndices);
                        for (int i = 0; i < 3 - polyIndices.Count; i++)
                            indices.Add(polyIndices.FirstOrDefault());
                    }
                    else
                    {
                        for (int i = 0; i < polyIndices.Count - 2; i++)
                        {
                            indices.Add(polyIndices[0]);
                            indices.Add(polyIndices[i + 1]);
                            indices.Add(polyIndices[i + 2]);
                        }
                    }
                }
                else
                {
                    indices.AddRange(polyIndices);
                }
            }

            int r, g, b;
            var colorResult = Irit.AttrGetObjectRGBColor(pObj, &r, &g, &b);
            if (Irit.IP_ATTR_IS_BAD_COLOR(colorResult))
                r = g = b = 255;
            var a = Irit.AttrGetObjectRealAttrib(pObj, IritStrings.transp);
            if (Irit.IP_ATTR_IS_BAD_REAL(a))
                a = 1.0;
            var color = new Color4(r, g, b, (int)(a * 255.99999999));

            //var pipeline = Irit.IP_IS_POLYGON_OBJ(pObj)
            //    ? pipelineSurf
            //    : pipelineLine;

            // todo: use material info

            var primitiveTopology = Irit.IP_IS_POLYGON_OBJ(pObj)
                ? FlexibleModelPrimitiveTopology.TriangleList
                : FlexibleModelPrimitiveTopology.LineStrip;

            var modelPart = new FlexibleModelPart
            {
                VertexSetIndex = 0,
                ModelMaterialName = "DefaultMaterial",
                PrimitiveTopology = primitiveTopology,
                FirstIndex = firstIndex,
                IndexCount = indices.Count - firstIndex
            };

            // todo: use attributes
            var attributes = new Dictionary<string, object>();

            var pAttr = pObj->Attr;
            while (pAttr != NULL)
            {
                var attr = *pAttr;
                string name;
                try
                {
                    var pName = Irit.AttrGetAttribName(pAttr);
                    name = Marshal.PtrToStringAnsi((IntPtr)pName);
                    // todo: free
                }
                catch
                {
                    continue;
                }
                switch (pAttr->Type)
                {
                    case IPAttributeType.IP_ATTR_NONE:
                        attributes.Add(name, "");
                        break;
                    case IPAttributeType.IP_ATTR_INT:
                        attributes.Add(name, attr.U.I.ToString(CultureInfo.InvariantCulture));
                        break;
                    case IPAttributeType.IP_ATTR_REAL:
                        attributes.Add(name, attr.U.R.ToString(CultureInfo.InvariantCulture));
                        break;
                    case IPAttributeType.IP_ATTR_REALPTR:
                    {
                        var reals = new double[attr.U.Vec.Len];
                        for (int i = 0; i < reals.Length; i++)
                            reals[i] = attr.U.Vec.Coord[i];
                        attributes.Add(name, string.Join(", ", reals.Select(x => x.ToString(CultureInfo.InvariantCulture))));
                        break;
                    }    
                    case IPAttributeType.IP_ATTR_UV:
                    {
                        var ptr = (float*)&attr.U;
                        var uv = new Vector2(ptr[0], ptr[1]);
                        attributes.Add(name, uv.ToString());
                        break;
                    }
                    case IPAttributeType.IP_ATTR_STR:
                    {
                        var str = Marshal.PtrToStringAnsi((IntPtr)attr.U.Str);
                        attributes.Add(name, str);
                        break;
                    }
                    case IPAttributeType.IP_ATTR_OBJ:
                    {
                        var str = Marshal.PtrToStringAnsi((IntPtr)attr.U.PObj->ObjName);
                        attributes.Add(name, "!@#POBJ " + str);
                        break;
                    }
                    case IPAttributeType.IP_ATTR_PTR:
                        attributes.Add(name, "!@#PTR " + (IntPtr)attr.U.Ptr);
                        break;
                    case IPAttributeType.IP_ATTR_REFPTR:
                        attributes.Add(name, "!@#PTR " + (IntPtr)attr.U.RefPtr);
                        break;
                    default:
                        attributes.Add(name, "!@#UNKNOWN (" + attr.Type + ")");
                        break;
                }
                pAttr = pAttr->Pnext;
            }

            Irit.IPPolyVrtxIdxFree(pVeridix);

            modelParts.Add(modelPart);
        }
    }
}