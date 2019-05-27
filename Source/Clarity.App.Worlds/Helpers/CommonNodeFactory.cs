using System;
using Clarity.App.Worlds.Interaction.RectangleManipulation;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryLayouts.Sphere;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.Helpers
{
    public class CommonNodeFactory : ICommonNodeFactory
    {
        private readonly IAmDiBasedObjectFactory objectFactory;
        private int counter;

        private const float FrustumDistance = 2.414213562373095f;

        public CommonNodeFactory(IAmDiBasedObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }
        
        public ISceneNode WorldRoot(bool withStoryComponent)
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "World";
            
            if (withStoryComponent)
            {
                node.Components.Add(PresentationRootComponent.Create(Guid.NewGuid()));
                node.Components.Add(AmFactory.Create<StoryServiceRootComponent>());
                var storyComponent = objectFactory.Create<StoryComponent>();
                storyComponent.StartLayoutType = typeof(SphereStoryLayout);
                node.Components.Add(storyComponent);
            }
            else
            {
                node.Components.Add(PresentationComponent.Create());
            }
            return node;
        }

        public ISceneNode ColorRectangleNode(Color4 color)
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "Color Rectangle";
            node.Components.Add(PresentationComponent.Create());
            var rectComponent = objectFactory.Create<RectangleComponent>();
            var colorRectComponent = objectFactory.Create<ColorRectangleComponent>();
            colorRectComponent.Color = color;
            node.Components.Add(rectComponent);
            node.Components.Add(colorRectComponent);
            return node;
        }

        public ISceneNode ImageRectangleNode(IImage image)
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "Image";
            node.Components.Add(PresentationComponent.Create());
            var rectComponent = objectFactory.Create<RectangleComponent>();
            var imageRectComponent = objectFactory.Create<ImageRectangleComponent>();
            imageRectComponent.Image = image;
            node.Components.Add(rectComponent);
            node.Components.Add(imageRectComponent);
            return node;
        }

        public ISceneNode MovieRectangleNode(IMovie movie)
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "Movie";
            node.Components.Add(PresentationComponent.Create());
            var rectComponent = objectFactory.Create<RectangleComponent>();
            var movieRectComponent = objectFactory.Create<MovieRectangleComponent>();
            movieRectComponent.Movie = movie;
            node.Components.Add(rectComponent);
            node.Components.Add(movieRectComponent);
            return node;
        }

        public ISceneNode RichTextRectangle(IRichText text)
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "Text Entity";
            node.Components.Add(PresentationComponent.Create());
            var rectComponent = objectFactory.Create<RectangleComponent>();
            rectComponent.DragByBorders = true;
            var richTextComponent = objectFactory.Create<RichTextComponent>();
            richTextComponent.TextBox.Text = text;
            node.Components.Add(rectComponent);
            node.Components.Add(richTextComponent);
            return node;
        }
        
        public ISceneNode StoryNode()
        {
            var node = objectFactory.Create<SceneNode>();
            node.Name = "StoryNode" + counter++;
            node.Components.Add(PresentationComponent.Create());
            node.Components.Add(objectFactory.Create<StoryComponent>());
            return node;
        }

        public ISceneNode RectangleEditGizmo(ISceneNode rectangleNode)
        {
            var gizmoRoot = AmFactory.Create<SceneNode>();
            gizmoRoot.Name = "RectangleEditGizmo";
            gizmoRoot.Components.Add(EditRectangleGizmoComponent.Create(rectangleNode));
            var dragGizmo = AmFactory.Create<SceneNode>();
            var dragComponent = AmFactory.Create<DragRectangleGizmoComponent>();
            dragGizmo.Components.Add(dragComponent);
            gizmoRoot.ChildNodes.Add(dragGizmo);

            ISceneNode CreateResizeGizmo(ResizeRectangleGizmoPlace place)
            {
                var resizeGizmo = AmFactory.Create<SceneNode>();
                var resizeComponent = AmFactory.Create<ResizeRectangleGizmoComponent>();
                resizeComponent.Place = place;
                resizeGizmo.Components.Add(resizeComponent);
                return resizeGizmo;
            }

            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.Left));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.Right));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.Bottom));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.Top));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.BottomLeft));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.BottomRight));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.TopLeft));
            gizmoRoot.ChildNodes.Add(CreateResizeGizmo(ResizeRectangleGizmoPlace.TopRight));

            return gizmoRoot;
        }
    }
}