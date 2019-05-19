using System;
using Clarity.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Special;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;
using Clarity.Ext.Rendering.Ogl3.Text;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Context.States.Rasterizer;
using ObjectGL.Api.Objects.Resources.Images;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class CgModelVisualElementHandler : IVisualElementHandler
    {
        private readonly IRtImageBuilder rtImageBuilder;
        private readonly ICommonObjects commonObjects;
        //private readonly IViewService viewService;

        public CgModelVisualElementHandler(IRtImageBuilder rtImageBuilder, ICommonObjects commonObjects)
        {
            this.rtImageBuilder = rtImageBuilder;
            this.commonObjects = commonObjects;
        }

        public bool CanHandle(IVisualElement element) => 
            element is ICgModelVisualElement;

        public unsafe void OnTraverse(ISceneRenderingContext context, IVisualElement visualElement)
        {
            var visual = (ICgModelVisualElement)visualElement;
            var node = context.CurrentTraverseNode;
            var queueItem = new RenderQueueItem
            {
                VisualElement = visualElement,
                Handler = this,
                Node = node
            };
            if (visual.Material.HasTransparency)
            {
                var camera = context.Camera;
                var transform = visual.Transform * node.GlobalTransform;
                var distSq = visual.DistanceToCameraSq(transform, camera);
                *(float*)&queueItem.ValData = distSq;
                context.StagesByName[StandardRenderStageNames.Transparent].Queue?.Enqueue(queueItem);
            }
            else
            {
                context.StagesByName[StandardRenderStageNames.Opaque].Queue?.Enqueue(queueItem);
            }
        }

        public void OnDequeue(ISceneRenderingContext context, RenderQueueItem queueItem)
        {
            var glContext = context.Infra.GlContext;
            var visual = (ICgModelVisualElement)queueItem.VisualElement;
            var node = queueItem.Node;

            switch (visual.PolygonMode)
            {
                case CgPolygonMode.Fill:
                    glContext.States.Rasterizer.PolygonModeFront.Set(PolygonMode.Fill);
                    break;
                case CgPolygonMode.Line:
                    glContext.States.Rasterizer.PolygonModeFront.Set(PolygonMode.Line);
                    break;
                case CgPolygonMode.Point:
                    glContext.States.Rasterizer.PolygonModeFront.Set(PolygonMode.Point);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (visual.CullFace)
            {
                case CgCullFace.None:
                    glContext.States.Rasterizer.CullFaceEnable.Set(false);
                    break;
                case CgCullFace.Back:
                    glContext.States.Rasterizer.CullFaceEnable.Set(true);
                    glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Back);
                    break;
                case CgCullFace.Front:
                    glContext.States.Rasterizer.CullFaceEnable.Set(true);
                    glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Front);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var material = visual.Material as IStandardMaterial ?? commonObjects.UndefinedMaterial;
            if (material.Hide)
                return;
            var glTexture2D = (ITexture2D)null;
            var diffuseColor = Color4.White;
            var blackIsTransparent = false;
            var whiteIsTransparent = false;
            switch (material.DiffuseTextureSource)
            {
                case ISingleColorPixelSource colorSource:
                    diffuseColor = colorSource.Color;
                    break;
                case IImage image:
                    var imageCache = image.CacheContainer.GetOrAddCache(Tuples.Pair(context.Infra, image), x => new CgImageCache(x.First, x.Second));
                    glTexture2D = imageCache.GetGlTexture2D();
                    break;
                case IMoviePlayback moviePlayback:
                    var movieCache = moviePlayback.CacheContainer.GetOrAddCache(Tuples.Pair(context.Infra, moviePlayback), x => new CgMovieInstanceCache(x.First, x.Second));
                    glTexture2D = movieCache.GetGlTexture2D();
                    break;
                case IRichTextPixelSource textSource:
                    // todo: create ThreeTuple<,,> and use it
                    var textBox = textSource.TextBox;
                    var textCache = textBox.CacheContainer.GetOrAddCache(() => new RichTextBoxCache(context.Infra, rtImageBuilder, textBox));
                    glTexture2D = textCache.GetGlTexture2D();
                    blackIsTransparent = textBox.Text.Style.TransparencyMode == RtTransparencyMode.BlackIsTransparent;
                    whiteIsTransparent = textBox.Text.Style.TransparencyMode == RtTransparencyMode.WhiteIsTransparent;
                    break;
                default:
                    glTexture2D = commonObjects.Texture2DForUndefinedSource;
                    break;
            }

            glContext.Bindings.Textures.Units[0].Set(glTexture2D);
            glContext.Bindings.Samplers[0].Set(commonObjects.DefaultSampler);

            var cWarpScroll = node.SearchComponent<WarpScrollComponent>();

            commonObjects.MaterialUb.SetData(new MaterialUniform
            {
                Color = diffuseColor,
                IsEdited = (Bool32)(visual.HighlightEffect == CgHighlightEffect.BlackWhiteBorder),
                IgnoreLighting = (Bool32)material.IgnoreLighting,
                IsSelected = (Bool32)(visual.HighlightEffect == CgHighlightEffect.RainbowBorder),
                UseTexture = (Bool32)(glTexture2D != null),
                NoSpecular = (Bool32)material.NoSpecular,
                ScrollingEnabled = (Bool32)(cWarpScroll != null),
                ScrollingAmount = cWarpScroll?.VisibleScrollAmount ?? 0.0f,
                BlackIsTransparent = (Bool32)blackIsTransparent,
                WhiteIsTransparent = (Bool32)whiteIsTransparent
            });

            // todo: Use ObjectGL bindings
            GL.PointSize(material.PointSize);
            GL.LineWidth(material.LineWidth);

            // todo: switch (visual.TransformSpace)
            var transform = visual.Transform * node.GlobalTransform;
            Matrix4x4 world;
            if (visual.TransformSpace == CgTransformSpace.ScreenAlighned)
            {
                var globalViewFrame = context.Camera.GetGlobalFrame();
                transform.Rotation =
                    Quaternion.RotationToFrame(globalViewFrame.Right, globalViewFrame.Up);
                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
            }
            else if (visual.TransformSpace == CgTransformSpace.Ortho)
            {
                var props = context.Camera.GetProps();

                var viewMat = context.Camera.GetViewMat();
                var orthoProjMat = Matrix4x4.OrthoFromPerspective((props.Target - props.Frame.Eye).Length(),
                    props.Projection.Fov, context.AspectRatio, -10f, 5f);

                var viewProjInverse = context.Camera.GetViewProjInverse(context.AspectRatio);

                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
                world *= viewMat * orthoProjMat * viewProjInverse;
            }
            else
            {
                world = Matrix4x4.Scaling(visual.NonUniformScale) * transform.ToMatrix4x4();
            }
            
            
            var zOffset = visual.ZOffset + (node.ParentNode?.ChildNodes.IndexOf(node) ?? 0) * GraphicsHelper.MinZOffset;
            commonObjects.TransformUb.SetData(new TransformUniform { World = world, WorldInverseTranspose = world.Invert().Transpose(), ZOffset = zOffset/*-(float)context.ZOffset */});

            var modelPart = visual.Model.Parts[visual.ModelPartIndex];
            var vertexSet = visual.Model.VertexSets[modelPart.VertexSetIndex];
            var vertexSetCache = vertexSet.CacheContainer.GetOrAddCache(Tuples.Pair(context.Infra, vertexSet), x => new VertexSetCache(x.First, x.Second));
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