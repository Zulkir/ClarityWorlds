using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Text.Rich;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class UcRichTextMeasurer : IRichTextMeasurer
    {
        private readonly TextGenerator textGenerator;

        public UcRichTextMeasurer()
        {
            textGenerator = new TextGenerator();
        }

        public Size2 MeasureString(string str, IRtSpanStyle style)
        {
            throw new System.NotImplementedException();
        }

        public Size2 GetCharSize(char ch, IRtSpanStyle style)
        {
            throw new System.NotImplementedException();
        }
    }
}