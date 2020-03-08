using System;
using Clarity.Common;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Special;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;
using Clarity.Ext.Rendering.Ogl3.Caches;
using Clarity.Ext.Rendering.Ogl3.Pipelining;
using Clarity.Ext.Rendering.Ogl3.Uniforms;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Context.States.DepthStencil;
using ObjectGL.Api.Context.States.Rasterizer;
using GL = OpenTK.Graphics.OpenGL.GL;
using PolygonMode = Clarity.Engine.Visualization.Elements.RenderStates.PolygonMode;

namespace Clarity.Ext.Rendering.Ogl3.Handlers
{
    public class ModelVisualElementHandler : IVisualElementHandler
    {
        private readonly IGraphicsInfra infra;
        private readonly ICommonObjects commonObjects;
        private readonly ISamplerCache samplerCache;

        public ModelVisualElementHandler(IGraphicsInfra infra, ICommonObjects commonObjects, ISamplerCache samplerCache)
        {
            this.infra = infra;
            this.commonObjects = commonObjects;
            this.samplerCache = samplerCache;
        }

        public bool CanHandle(IVisualElement element) => 
            element is IModelVisualElement;

        public bool HasTransparency(RenderQueueItem queueItem) => 
            ((IModelVisualElement)queueItem.VisualElement).Material.HasTransparency;

        public float GetCameraDistSq(RenderQueueItem queueItem, ICamera camera)
        {
            var visual = (IModelVisualElement)queueItem.VisualElement;
            var transform = visual.Transform * queueItem.Node.GlobalTransform;
            return visual.DistanceToCameraSq(transform, camera);
        }

        public void Draw(RenderQueueItem queueItem, ICamera camera, float aspectRatio)
        {
            var glContext = infra.GlContext;
            var visual = (IModelVisualElement)queueItem.VisualElement;
            var node = queueItem.Node;

            if (visual.Hide)
                return;

            var renderStateData = visual.RenderState.GetFallbackData();

            glContext.Bindings.Program.Set(commonObjects.StandardShaderProgram);

            switch (renderStateData.PolygonMode)
            {
                case PolygonMode.Fill:
                    glContext.States.Rasterizer.PolygonModeFront.Set(ObjectGL.Api.Context.States.Rasterizer.PolygonMode.Fill);
                    break;
                case PolygonMode.Line:
                    glContext.States.Rasterizer.PolygonModeFront.Set(ObjectGL.Api.Context.States.Rasterizer.PolygonMode.Line);
                    break;
                case PolygonMode.Point:
                    glContext.States.Rasterizer.PolygonModeFront.Set(ObjectGL.Api.Context.States.Rasterizer.PolygonMode.Point);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (renderStateData.CullFace)
            {
                case CullFace.None:
                    glContext.States.Rasterizer.CullFaceEnable.Set(false);
                    break;
                case CullFace.Back:
                    glContext.States.Rasterizer.CullFaceEnable.Set(true);
                    glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Back);
                    break;
                case CullFace.Front:
                    glContext.States.Rasterizer.CullFaceEnable.Set(true);
                    glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Front);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            glContext.States.DepthStencil.StencilTestEnable.Set(true);
            var stencilFunctionSettings = new StencilFunctionSettings
            {
                Function = StencilFunction.Always,
                Mask = 0xff,
                Reference = 1
            };
            var stencilOperationSettings = new StencilOperationSettings
            {
                DepthPass = queueItem.IsHighlighted ? StencilOp.Replace : StencilOp.Keep,
                DepthFail = queueItem.IsHighlighted ? StencilOp.Replace : StencilOp.Keep,
                StencilFail = StencilOp.Keep
            };
            glContext.States.DepthStencil.Front.StencilWriteMask.Set(0xff);
            glContext.States.DepthStencil.Back.StencilWriteMask.Set(0xff);
            glContext.States.DepthStencil.Front.StencilFunctionSettings.Set(stencilFunctionSettings);
            glContext.States.DepthStencil.Back.StencilFunctionSettings.Set(stencilFunctionSettings);
            glContext.States.DepthStencil.Front.StencilOperationSettings.Set(stencilOperationSettings);
            glContext.States.DepthStencil.Back.StencilOperationSettings.Set(stencilOperationSettings);
            
            var material = visual.Material as IStandardMaterial ?? commonObjects.UndefinedMaterial;
            var diffuseColor = material.DiffuseColor;
            var glDiffuseMap = material.DiffuseMap
                ?.CacheContainer
                .GetOrAddCache(Tuples.Pair(infra, material.DiffuseMap), 
                               x => new CgImageCache(x.First, x.Second))
                .GetGlTexture2D();
            var glNormalMap = material.NormalMap
                ?.CacheContainer
                .GetOrAddCache(Tuples.Pair(infra, material.NormalMap), 
                    x => new CgImageCache(x.First, x.Second))
                .GetGlTexture2D();

            glContext.Bindings.Textures.Units[0].Set(glDiffuseMap);
            glContext.Bindings.Textures.Units[1].Set(glNormalMap);
            var sampler = samplerCache.GetGlSampler(material.Sampler);
            glContext.Bindings.Samplers[0].Set(sampler);
            glContext.Bindings.Samplers[1].Set(sampler);

            var cWarpScroll = node.SearchComponent<IWarpScrollComponent>();

            commonObjects.MaterialUb.SetData(new MaterialUniform
            {
                Color = diffuseColor.SrgbToLinear(),
                IsEdited = (Bool32)(material.HighlightEffect == HighlightEffect.BlackWhiteBorder),
                IgnoreLighting = (Bool32)material.IgnoreLighting,
                IsSelected = (Bool32)(material.HighlightEffect == HighlightEffect.RainbowBorder),
                UseTexture = (Bool32)(glDiffuseMap != null),
                UseNormalMap = (Bool32)(glNormalMap != null),
                NoSpecular = (Bool32)material.NoSpecular,
                ScrollingEnabled = (Bool32)(cWarpScroll != null),
                ScrollingAmount = cWarpScroll?.VisibleScrollAmount ?? 0.0f,
                BlackIsTransparent = (Bool32)(material.RtTransparencyMode == RtTransparencyMode.BlackIsTransparent),
                WhiteIsTransparent = (Bool32)(material.RtTransparencyMode == RtTransparencyMode.WhiteIsTransparent),
                IsPulsating = (Bool32)(material.HighlightEffect == HighlightEffect.Pulsating),
                PulsatingColor = Color4.Orange
            });

            // todo: Use ObjectGL bindings
            GL.PointSize(renderStateData.PointSize);
            GL.LineWidth(renderStateData.LineWidth);

            // todo: switch (visual.TransformSpace)
            var transform = visual.Transform * node.GlobalTransform;
            Matrix4x4 world;
            if (visual.TransformSpace == TransformSpace.ScreenAlighned)
            {
                var globalViewFrame = camera.GetGlobalFrame();
                transform.Rotation =
                    Quaternion.RotationToFrame(globalViewFrame.Right, globalViewFrame.Up);
                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
            }
            else if (visual.TransformSpace == TransformSpace.Ortho)
            {
                var props = camera.GetProps();

                var viewMat = camera.GetViewMat();
                var orthoProjMat = Matrix4x4.OrthoFromPerspective((props.Target - props.Frame.Eye).Length(),
                    props.Projection.Fov, aspectRatio, -10f, 5f);

                var viewProjInverse = camera.GetViewProjInverse(aspectRatio);

                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
                world *= viewMat * orthoProjMat * viewProjInverse;
            }
            else
            {
                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
            }
            
            var zOffset = renderStateData.ZOffset + 
                          GraphicsHelper.MinZOffset * (node.ChildNodes.IsEmptyL() 
                            ? (node.ParentNode?.ChildNodes.IndexOf(node) ?? 0)
                            : 0);
            commonObjects.TransformUb.SetData(new TransformUniform { World = world, WorldInverseTranspose = world.Invert().Transpose(), ZOffset = zOffset});
            
            switch (visual.Model)
            {
                case IFlexibleModel flexibleModel:
                {
                    var modelPart = flexibleModel.Parts[visual.ModelPartIndex];
                    var vertexSet = flexibleModel.VertexSets[modelPart.VertexSetIndex];
                    var vertexSetCache = vertexSet.CacheContainer.GetOrAddCache(Tuples.Pair(infra, vertexSet), x => new VertexSetCache(x.First, x.Second));

                    var glVao = vertexSetCache.GetGlVao();

                    glContext.Bindings.VertexArray.Set(glVao);

                    var beginMode = GetBeginMode(modelPart.PrimitiveTopology);
                    if (vertexSet.IndicesInfo == null)
                    {
                        glContext.Actions.Draw.Arrays(beginMode, modelPart.FirstIndex + modelPart.VertexOffset, modelPart.IndexCount);
                    }
                    else
                    {
                        AnalyzeIndexType(vertexSet.IndicesInfo.Format, out var drawElementsType, out var offsetMultiplier);
                        var indexBufferOffset = modelPart.FirstIndex * offsetMultiplier;
                        if (modelPart.VertexOffset == 0)
                            glContext.Actions.Draw.Elements(beginMode, modelPart.IndexCount, drawElementsType, indexBufferOffset);
                        else
                            glContext.Actions.Draw.ElementsBaseVertex(beginMode, modelPart.IndexCount, drawElementsType, indexBufferOffset, modelPart.VertexOffset);
                    }
                    break;
                }
                case IExplicitModel explicitModel:
                {
                    var modelPart = explicitModel.IndexSubranges[visual.ModelPartIndex];
                    var cache = explicitModel.CacheContainer.GetOrAddCache(
                        Tuples.Pair(infra, explicitModel),
                        x => new ExplicitModelCache(x.First, x.Second));
                    var glObjects = cache.GetGlObjects();
                    glContext.Bindings.VertexArray.Set(glObjects.Vao);
                    var beginMode = GetBeginMode(explicitModel.Topology);
                    if (glObjects.IndexBuffer == null)
                    {
                        glContext.Actions.Draw.Arrays(beginMode, modelPart.FirstIndex, modelPart.IndexCount);
                    }
                    else
                    {
                        var drawElementsType = glObjects.SixteenBitIndices ? DrawElementsType.UnsignedShort : DrawElementsType.UnsignedInt;
                        var indexSize = glObjects.SixteenBitIndices ? sizeof(ushort) : sizeof(int);
                        var indexBufferOffset = modelPart.FirstIndex * indexSize;
                        glContext.Actions.Draw.Elements(beginMode, modelPart.IndexCount, drawElementsType, indexBufferOffset);
                    }
                    break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static BeginMode GetBeginMode(FlexibleModelPrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case FlexibleModelPrimitiveTopology.PointList: return BeginMode.Points;
                case FlexibleModelPrimitiveTopology.LineList: return BeginMode.Lines;
                case FlexibleModelPrimitiveTopology.LineStrip: return BeginMode.LineStrip;
                case FlexibleModelPrimitiveTopology.TriangleList: return BeginMode.Triangles;
                case FlexibleModelPrimitiveTopology.TriangleStrip: return BeginMode.TriangleStrip;
                case FlexibleModelPrimitiveTopology.TriangleFan: return BeginMode.TriangleFan;
                case FlexibleModelPrimitiveTopology.LineListWithAdjacency: return BeginMode.LinesAdjacency;
                case FlexibleModelPrimitiveTopology.LineStripWithAdjacency: return BeginMode.LineStripAdjacency;
                case FlexibleModelPrimitiveTopology.TriangleListWithAdjacency: return BeginMode.TrianglesAdjacency;
                case FlexibleModelPrimitiveTopology.TriangleStripWithAdjacency: return BeginMode.TriangleStripAdjacency;
                case FlexibleModelPrimitiveTopology.PatchList: return BeginMode.Patches;
                default: throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null);
            }
        }

        private static BeginMode GetBeginMode(ExplicitModelPrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case ExplicitModelPrimitiveTopology.PointList: return BeginMode.Points;
                case ExplicitModelPrimitiveTopology.LineList: return BeginMode.Lines;
                case ExplicitModelPrimitiveTopology.LineStrip: return BeginMode.LineStrip;
                case ExplicitModelPrimitiveTopology.TriangleList: return BeginMode.Triangles;
                case ExplicitModelPrimitiveTopology.TriangleStrip: return BeginMode.TriangleStrip;
                default: throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null);
            }
        }

        private static void AnalyzeIndexType(CommonFormat indexFormat, out DrawElementsType drawElementsType, out int offsetMultiplier)
        {
            switch (indexFormat)
            {
                case CommonFormat.R8_SINT:
                case CommonFormat.R8_UINT:
                    drawElementsType = DrawElementsType.UnsignedByte;
                    offsetMultiplier = 1;
                    break;
                case CommonFormat.R16_SINT:
                case CommonFormat.R16_UINT:
                    drawElementsType = DrawElementsType.UnsignedShort;
                    offsetMultiplier = 2;
                    break;
                case CommonFormat.R32_SINT:
                case CommonFormat.R32_UINT:
                    drawElementsType = DrawElementsType.UnsignedInt;
                    offsetMultiplier = 4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(indexFormat), indexFormat, null);
            }
        }
    }
}