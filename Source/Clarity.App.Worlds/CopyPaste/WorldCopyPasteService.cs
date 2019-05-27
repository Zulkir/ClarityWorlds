using Clarity.Engine.Gui;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.CopyPaste
{
    public class WorldCopyPasteService : IWorldCopyPasteService
    {
        public const string NodeDataType = "SceneNode";
        public const string NodeText = "__$cw_node$__";

        private readonly IClipboard clipboard;
        private ISceneNode copiedNode;
        private IImage copiedImage;
        private object copiedImageToken;

        public WorldCopyPasteService(IClipboard clipboard)
        {
            this.clipboard = clipboard;
        }

        public string Text { get => clipboard.Text != NodeText ? clipboard.Text : null; set => clipboard.Text = value; }
        public string Html { get => clipboard.Html; set => clipboard.Html = value; }

        public IImage Image
        {
            get
            {
                if (clipboard.ImageToken == copiedImageToken)
                    return copiedImage;
                copiedImageToken = clipboard.ImageToken;
                if (copiedImageToken == null)
                {
                    copiedImage = null;
                    return copiedImage;
                }
                var imageData = clipboard.DecodeImageToken(copiedImageToken);
                copiedImage = new RawImage(ResourceVolatility.Immutable, imageData.First, imageData.Second);
                return copiedImage;
            }
            set
            {
                if (copiedImage == value)
                    return;
                copiedImage = value;
                copiedImageToken = clipboard.EncodeImageToken(value.Size, value.GetRawData());
                clipboard.ImageToken = copiedImageToken;
            }
        }

        public ISceneNode Node
        {
            get
            {
                if (clipboard.Text != NodeText)
                    copiedNode = null;
                return copiedNode;
            }
            set
            {
                clipboard.Text = NodeText;
                copiedNode = value;
            }
        }

        public bool Contains(string dataType)
        {
            return dataType == NodeDataType ? Node != null : clipboard.Contains(dataType);
        }

        public void Clear()
        {
            copiedNode = null;
            copiedImage = null;
            copiedImageToken = null;
            clipboard.Clear();
        }
    }
}