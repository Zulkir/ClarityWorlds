using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.SaveLoad.Import;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.Files;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;

namespace Clarity.Ext.Import.Pptx
{
    public class PptxPresentationImporter : IPresentationImporter
    {
        private static readonly IReadOnlyList<string> Extensions = new[] { ".pptx" };
        private static readonly char[] Separators = { '\r' };

        public string Name => "PowerPoint (PPTX)";
        public IReadOnlyList<string> FileExtensions => Extensions;

        private readonly ICommonNodeFactory commonNodeFactory;

        public PptxPresentationImporter(ICommonNodeFactory commonNodeFactory)
        {
            this.commonNodeFactory = commonNodeFactory;
        }

        public void Load(IFileLoadInfo loadInfo)
        {
            var ppApp = new Application();
            var ppPresentation = ppApp.Presentations.Open(loadInfo.FilePath, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
            var context = InitConversionContext(loadInfo, ppPresentation);
            foreach (Slide slide in ppPresentation.Slides)
                LoadSlideNode(context, slide);
            var sceneRoot = context.World.Scenes.First().Root;
            var cStoryRoot = sceneRoot.GetComponent<StoryServiceRootComponent>();
            foreach (var nodePair in context.SlideNodes.SequentialPairs())
                cStoryRoot.Edges.Add(Tuples.SameTypePair(nodePair.First.Id, nodePair.Second.Id));
            var cStory = sceneRoot.GetComponent<IStoryComponent>();
            cStory.ShowAux1 = false;
            cStory.ShowAux2 = false;
            loadInfo.OnLoadedWorld(context.World);
            ppPresentation.Close();
        }

        private ConversionContext InitConversionContext(IFileLoadInfo loadInfo, Presentation ppPresentation)
        {
            var nextId = 1;
            var world = AmFactory.Create<World>();
            var scene = AmFactory.Create<Scene>();
            scene.Name = "Scene";
            scene.BackgroundColor = Color4.White;
            world.Scenes.Add(scene);
            var rootNode = commonNodeFactory.WorldRoot(true);
            scene.Root = rootNode;
            rootNode.Id = nextId++;
            var floorNode = commonNodeFactory.StoryNode();
            floorNode.Id = nextId++;
            floorNode.Name = "Slides";
            rootNode.ChildNodes.Add(floorNode);

            return new ConversionContext
            {
                NextId = nextId,
                LoadInfo = loadInfo,
                PpPresentation = ppPresentation,
                World = world,
                SlideNodes = floorNode.ChildNodes
            };
        }

        private void LoadSlideNode(ConversionContext context, Slide slide)
        {
            var slideNode = commonNodeFactory.StoryNode();
            slideNode.Id = context.GetNewId();
            slideNode.Name = slide.Name;
            context.CurrentSlide = slide;
            context.CurrentSlideNode = slideNode;
            foreach (Shape shape in slide.Shapes)
                if (shape.HasTextFrame == MsoTriState.msoTrue && shape.TextFrame.HasText == MsoTriState.msoTrue)
                    LoadTextShape(context, shape);
                else if (shape.Type == MsoShapeType.msoPicture)
                    LoadPictureShape(context, shape);
            context.SlideNodes.Add(slideNode);
        }

        private void LoadTextShape(ConversionContext context, Shape shape)
        {
            var richText = AmFactory.Create<RichText>();
            var style = AmFactory.Create<RtOverallStyle>();
            style.TransparencyMode = RtTransparencyMode.Opaque;
            style.BackgroundColor = context.CurrentSlide.Background.Fill.ForeColor.ToClarity();
            richText.Style = style;

            foreach (TextRange ppPara in shape.TextFrame.TextRange.Paragraphs())
            {
                var cPara = AmFactory.Create<RtParagraph>();
                cPara.Style.Alignment = ppPara.ParagraphFormat.Alignment.ToClarity() ?? RtParagraphAlignment.Left;
                cPara.Style.Direction = ppPara.ParagraphFormat.TextDirection.ToClarity() ?? RtParagraphDirection.LeftToRight;
                cPara.Style.ListType = ppPara.ParagraphFormat.Bullet.Type.ToClarity() ?? RtListType.None;
                // todo: bullet conversion
                // todo: correct margin conversion
                cPara.Style.MarginUp = ppPara.ParagraphFormat.SpaceBefore * 512 / context.PpPresentation.PageSetup.SlideHeight;
                cPara.Style.TabCount = cPara.Style.ListType == RtListType.None 
                    ? ppPara.IndentLevel - 1
                    : ppPara.IndentLevel;

                foreach (TextRange ppRun in ppPara.Runs())
                {
                    var cSpan = AmFactory.Create<RtPureSpan>();

                    cSpan.Style.FontDecoration = FontDecoration.None;
                    if (ppRun.Font.Bold == MsoTriState.msoTrue)
                        cSpan.Style.FontDecoration |= FontDecoration.Bold;
                    if (ppRun.Font.Italic == MsoTriState.msoTrue)
                        cSpan.Style.FontDecoration |= FontDecoration.Italic;
                    // todo: Strikethrough
                    if (ppRun.Font.Underline == MsoTriState.msoTrue)
                        cSpan.Style.FontDecoration |= FontDecoration.Underline;

                    cSpan.Style.FontFamily = ppRun.Font.Name;
                    // todo: set actual size and compensate some other way
                    cSpan.Style.Size = (int)(ppRun.Font.Size * 512f / 360f);
                    cSpan.Style.TextColor = ppRun.Font.Color.ToClarity();

                    cSpan.Text = ppRun.Text; // remove \r ?

                    cPara.Spans.Add(cSpan);
                }

                richText.Paragraphs.Add(cPara);
            }
            
            var textNode = commonNodeFactory.RichTextRectangle(richText);
            textNode.Id = context.GetNewId();
            textNode.Name = shape.Name;
            var cRect = textNode.GetComponent<IRectangleComponent>();
            cRect.Rectangle = GetShapeRectangle(context, shape);

            context.CurrentSlideNode.ChildNodes.Add(textNode);
        }

        private void LoadPictureShape(ConversionContext context, Shape shape)
        {
            var filePath = Path.GetTempFileName().Replace(".tmp", ".png");
            shape.Export(filePath, PpShapeFormat.ppShapeFormatPNG);
            var nodeId = context.GetNewId();
            var asset = context.LoadInfo.OnFoundAsset(new AssetLoadInfo
            {
                AssetName = $"Picture {nodeId}",
                LoadPath = filePath,
                FileSystem = ActualFileSystem.Singleton,
                ReferencePath = filePath,
                StorageType = AssetStorageType.CopyLocal
            });

            var node = commonNodeFactory.ImageRectangleNode((IImage)asset.Resource);
            node.Id = nodeId;
            node.Name = shape.Name;
            var cRect = node.GetComponent<IRectangleComponent>();
            cRect.Rectangle = GetShapeRectangle(context, shape);

            context.CurrentSlideNode.ChildNodes.Add(node);
        }

        private static AaRectangle2 GetShapeRectangle(ConversionContext context, Shape shape)
        {
            var ppScreenWidth = context.PpPresentation.PageSetup.SlideWidth;
            var ppScreenHeight = context.PpPresentation.PageSetup.SlideHeight;
            var aspectRatio = GraphicsHelper.AspectRatio((int)ppScreenWidth, (int)ppScreenHeight);

            var ppRectLeft = shape.Left;
            var xlin = 2f / ppScreenWidth * aspectRatio;
            var xconst = -aspectRatio;
            var clarityLeft = ppRectLeft * xlin + xconst;

            var ppRectBottom = shape.Top + shape.Height;
            var ylin = -2f / ppScreenHeight;
            var yconst = 1f;
            var clarityBottom = ppRectBottom * ylin + yconst;

            var wlin = 2f * aspectRatio / ppScreenWidth;
            var hlin = 2f / ppScreenHeight;
            
            return new AaRectangle2(
                new Vector2(clarityLeft, clarityBottom), 
                new Size2(shape.Width * wlin, shape.Height * hlin));
        }
    }
}
