﻿using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Gui;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Views
{
    public interface IViewServiceModel
    {
        IRenderGuiControl RenderControl { get; set; }
        ISceneNode SelectedNode { get; set; }
        event Action<IAmEventMessage> Updated;
    }
}