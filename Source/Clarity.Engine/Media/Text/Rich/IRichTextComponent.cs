using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextComponent : ISceneNodeComponent
    {
        IRichTextBox TextBox { get; set; }
        Vector2[] BorderCurve { get; }
        IList<Vector2> VisualBorderCurve { get; set; }
        bool BorderComplete { get; set; }
        RtPosition CursorPosition { get; set; }
        RtPosition? SelectionStart { get;set; }
        RtRange? SelectionRange { get; }
        IRtSpanStyle InputTextStyle { get; set; }
    }
}