using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Algebra;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Ext.Format.Dicom.Native;
using Clarity.Ext.Format.Itd;
using IritNet;

namespace Clarity.Ext.Format.Dicom
{
    // todo: to multifile loader
    public unsafe class DicomModelLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = {".dcm", ".dcmf"};

        public string Name => "Technion.DICOM";
        public string AssetTypeString => "Volumeric Data (DICOM) (DicomProject)";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            // todo: use file system
            try
            {
                if (Path.GetExtension(loadInfo.LoadPath) == ".dcm")
                    return LoadSingle(loadInfo);
                if (Path.GetExtension(loadInfo.LoadPath) == ".dcmf")
                    return LoadFolder(loadInfo);
                return AssetLoadResultByLoader.Failure("Neither .dcm nor .dcmf", null);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure(ex.GetType().Name, ex);
            }
        }

        private AssetLoadResultByLoader LoadSingle(AssetLoadInfo loadInfo)
        {
            void* dicomManager = null;
            var pFileName = Marshal.StringToHGlobalAnsi(loadInfo.LoadPath);
            CheckSuccess(DicomProject.AnalyzerSingleDicomFile((byte*)pFileName, &dicomManager));
            Marshal.FreeHGlobal(pFileName);
            return Load(loadInfo, dicomManager);
        }

        private AssetLoadResultByLoader LoadFolder(AssetLoadInfo loadInfo)
        {
            void* dicomManager = null;
            var folderRelPath = Path.GetFileNameWithoutExtension(loadInfo.LoadPath);
            var folderPath = Path.Combine(Path.GetDirectoryName(loadInfo.LoadPath), folderRelPath);
            var pFolderPath = Marshal.StringToHGlobalAnsi(folderPath);
            CheckSuccess(DicomProject.AnalyzerMultiDicomFiles((byte*)pFolderPath, &dicomManager));
            Marshal.FreeHGlobal(pFolderPath);
            return Load(loadInfo, dicomManager);
        }

        private static AssetLoadResultByLoader Load(AssetLoadInfo loadInfo, void * dicomManager)
        {
            int width, height, depth;
            CheckSuccess(DicomProject.AnalyzerGetDimensions(dicomManager, 1, &width, &height, &depth));

            var lowerBuffer = new byte[width * height];
            var higherBuffer = new byte[width * height];
            var vertexList = new List<CgVertexPosNormTex>();

            fixed (byte* pBuffer = higherBuffer)
            {
                int bufferSize = higherBuffer.Length;
                int bytesWritten;
                CheckSuccess(DicomProject.AnalyzerGetMonochromePixelDataBufferOfSlice(dicomManager, 1, pBuffer, &bufferSize, &bytesWritten));
            }

            for (int z = 1; z < depth; z++)
            {
                CodingHelper.Swap(ref lowerBuffer, ref higherBuffer);

                fixed (byte* pBuffer = higherBuffer)
                {
                    int bufferSize = higherBuffer.Length;
                    int bytesWritten;
                    CheckSuccess(DicomProject.AnalyzerGetMonochromePixelDataBufferOfSlice(dicomManager, z + 1, pBuffer, &bufferSize, &bytesWritten));
                }

                for (int y = 0; y < height - 1; y++)
                for (int x = 0; x < width - 1; x++)
                {
                    var vx = (100f / width) * x - 50f;
                    var vy = (100f / height) * y - 50f;
                    var vz = (100f / depth) * z - 50f;
                        
                    var yStride = width;
                        
                    var mcCube = new MCCubeCornerScalarStruct();
                    mcCube.CubeDim = new IrtPtType(100.0 / width, 100.0 / height, 100.0 / depth);
                    //mcCube.CubeDim = new IrtPtType(scale, scale, scale);
                    mcCube.Vrtx0Lctn = new IrtPtType(vx, vy, vz);
                    mcCube.Corners[0] = UNormToDouble(lowerBuffer[yStride * y + x]);
                    mcCube.Corners[1] = UNormToDouble(lowerBuffer[yStride * y + (x + 1)]);
                    mcCube.Corners[2] = UNormToDouble(lowerBuffer[yStride * (y + 1) + (x + 1)]);
                    mcCube.Corners[3] = UNormToDouble(lowerBuffer[yStride * (y + 1) + x]);
                    mcCube.Corners[4] = UNormToDouble(higherBuffer[yStride * y + x]);
                    mcCube.Corners[5] = UNormToDouble(higherBuffer[yStride * y + (x + 1)]);
                    mcCube.Corners[6] = UNormToDouble(higherBuffer[yStride * (y + 1) + (x + 1)]);
                    mcCube.Corners[7] = UNormToDouble(higherBuffer[yStride * (y + 1) + x]);
                    // todo: use ref instead
                    EstimateGradient(&mcCube);
                    var MCPolys = Irit.MCThresholdCube(&mcCube, 0.5);

                    while (MCPolys != null)
                    {
                        var texCoord = new Vector2((float)x / width, (float)z / depth);
                        
                        var vertex0 = MCPolys->GetClarityVertex(0, texCoord);
                        var vertexCurr = MCPolys->GetClarityVertex(1, texCoord);
                            
                        for (var i = 2; i < MCPolys->NumOfVertices; i++)
                        {
                            var vertexPrev = vertexCurr;

                            vertexCurr = MCPolys->GetClarityVertex(i, texCoord);
                            vertexList.Add(vertex0);
                            vertexList.Add(vertexPrev);
                            vertexList.Add(vertexCurr);
                        }

                        //Irit.IritFree(MCPolys);

                        MCPolys = MCPolys->Pnext;
                    }
                }
            }

            var pack = new ResourcePack(ResourceVolatility.Immutable);
            
            var rawVerticesData = new RawDataResource(ResourceVolatility.Immutable, vertexList.Count * sizeof(CgVertexPosNormTex));
            pack.AddSubresource("VertexData", rawVerticesData);
            var pRawData = (CgVertexPosNormTex*)rawVerticesData.Map();
            for (int i = 0; i < vertexList.Count; i++)
                pRawData[i] = vertexList[i];
            rawVerticesData.Unmap(true);

            var vertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, new[] { new RawDataResSubrange(rawVerticesData, 0) }, CgVertexPosNormTex.GetElementsInfos(0), null);
            pack.AddSubresource("ModelVertexSet", vertexSet);
            var modelPart = new FlexibleModelPart
            {
                IndexCount = vertexList.Count,
                PrimitiveTopology = FlexibleModelPrimitiveTopology.TriangleList,
                VertexSetIndex = 0
            };
            var model = new FlexibleModel(ResourceVolatility.Immutable, new[] { vertexSet }, new[] { modelPart }, 50f);
            pack.AddSubresource("Model", model);

            CheckSuccess(DicomProject.AnalyzerKillDicomAnalyzer(&dicomManager));

            // todo: calculate hash honestly
            var hash = AssetHashMd5.Random(new Random());
            var fileName = Path.GetFileName(loadInfo.LoadPath);
            var asset = new Asset(loadInfo.AssetName, pack, AssetStorageType.ReferenceOriginal, hash, loadInfo.ReferencePath, fileName);
            return AssetLoadResultByLoader.Success(asset);
        }

        private static void CheckSuccess(DicomResult result)
        {
            if (result != DicomResult.SUCCESS)
                throw new Exception(result.ToString());
        }

        private static long GetLong(void* dm, int slice, TagInfoStruct tagInfo, int index)
        {
            var val = 0l;
            var res = DicomProject.AnalyzerGetTagValueLong(dm, 1, tagInfo.GroupId, tagInfo.Id, &val, 0);
            CheckSuccess(res);
            return val;
        }

        private static double UNormToDouble(byte unorm) => unorm / 255.0;

        private static void EstimateGradient(MCCubeCornerScalarStruct* CCS)
        {
            CCS->GradientX[0] =
                CCS->GradientX[1] =
                    CCS->Corners[1] - CCS->Corners[0];
            CCS->GradientX[2] =
                CCS->GradientX[3] =
                    CCS->Corners[2] - CCS->Corners[3];
            CCS->GradientX[4] =
                CCS->GradientX[5] =
                    CCS->Corners[5] - CCS->Corners[4];
            CCS->GradientX[6] =
                CCS->GradientX[7] =
                    CCS->Corners[6] - CCS->Corners[7];

            CCS->GradientY[0] =
                CCS->GradientY[3] =
                    CCS->Corners[3] - CCS->Corners[0];
            CCS->GradientY[1] =
                CCS->GradientY[2] =
                    CCS->Corners[2] - CCS->Corners[1];
            CCS->GradientY[4] =
                CCS->GradientY[7] =
                    CCS->Corners[7] - CCS->Corners[4];
            CCS->GradientY[5] =
                CCS->GradientY[6] =
                    CCS->Corners[6] - CCS->Corners[5];

            CCS->GradientZ[0] =
                CCS->GradientZ[4] =
                    CCS->Corners[4] - CCS->Corners[0];
            CCS->GradientZ[1] =
                CCS->GradientZ[5] =
                    CCS->Corners[5] - CCS->Corners[1];
            CCS->GradientZ[2] =
                CCS->GradientZ[6] =
                    CCS->Corners[6] - CCS->Corners[2];
            CCS->GradientZ[3] =
                CCS->GradientZ[7] =
                    CCS->Corners[7] - CCS->Corners[3];

            CCS->HasGradient = Irit.TRUE;
        }
    }
}