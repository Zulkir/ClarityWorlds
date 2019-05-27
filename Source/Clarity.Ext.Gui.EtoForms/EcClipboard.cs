using System.Linq;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Gui;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class EcClipboard : IClipboard
    {
        private readonly Clipboard clipboard;

        public EcClipboard()
        {
            clipboard = new Clipboard();
        }

        public string Text { get => clipboard.Text; set => clipboard.Text = value; }
        public string Html { get => clipboard.Html; set => clipboard.Html = value; }
        public object ImageToken { get; set; }

        public bool Contains(string dataType) => clipboard.Types.Any(x => x == dataType);
        public byte[] GetData(string dataType) => clipboard.GetData(dataType);
        public void SetData(string dataType, byte[] data) => clipboard.SetData(data, dataType);
        public void Clear() => clipboard.Clear();

        public Pair<IntSize2, byte[]> DecodeImageToken(object imageToken)
        {
            var image = (Image)imageToken;
            return Tuples.Pair(image.Size.ToClarity(), FromEtoImage.GetRawData(image));
        }

        public object EncodeImageToken(IntSize2 size, byte[] srgbaData)
        {
            return FromEtoImage.EtoBitmapFromRaw(size, srgbaData);
        }
    }
}