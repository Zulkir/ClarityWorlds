using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Gui
{
    public interface IClipboard
    {
        string Text { get; set; }
        string Html { get; set; }
        object ImageToken { get; set; }

        bool Contains(string dataType);
        byte[] GetData(string dataType);
        void SetData(string dataType, byte[] data);

        void Clear();

        Pair<IntSize2, byte[]> DecodeImageToken(object imageToken);
        object EncodeImageToken(IntSize2 size, byte[] srgbaData);
    }
}