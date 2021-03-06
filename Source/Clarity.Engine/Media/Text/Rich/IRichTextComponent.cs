﻿using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextComponent : ISceneNodeComponent
    {
        IRichTextBox TextBox { get; set; }
        Vector2[] BorderCurve { get; set; }
        IList<Vector2> VisualBorderCurve { get; set; }
        bool BorderComplete { get; set; }
        IRichTextHeadlessEditor HeadlessEditor { get; }
    }
}