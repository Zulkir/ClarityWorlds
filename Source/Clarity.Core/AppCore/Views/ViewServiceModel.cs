using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Gui;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Views
{
    public abstract class ViewServiceModel : AmObjectBase<ViewServiceModel>, IViewServiceModel
    {
        [AmReference]
        public abstract IRenderGuiControl RenderControl { get; set; }
        [AmReference]
        public abstract ISceneNode SelectedNode { get; set; }
        public event Action<IAmEventMessage> Updated;

        public override void AmOnChildEvent(IAmEventMessage message) => 
            Updated?.Invoke(message);
    }
}