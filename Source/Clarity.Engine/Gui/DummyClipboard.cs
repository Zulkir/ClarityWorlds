using System;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Gui
{
    public class DummyClipboard : IClipboard
    {
        public string Text { get => null; set { } }
        public string Html { get => null; set { } }
        public object ImageToken { get => null; set { } }
        public bool Contains(string dataType) { return false; }
        public byte[] GetData(string dataType) => throw new InvalidOperationException("Dummy clipboard is always empty.");
        public void SetData(string dataType, byte[] data) { }
        public void Clear() { }
        public Pair<IntSize2, byte[]> DecodeImageToken(object imageToken) => throw new InvalidOperationException("Dummy clipboard is always empty.");
        public object EncodeImageToken(IntSize2 size, byte[] srgbaData) => new object();

    }
}