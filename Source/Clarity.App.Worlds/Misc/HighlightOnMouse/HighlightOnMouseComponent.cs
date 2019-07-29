using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;

namespace Clarity.App.Worlds.Misc.HighlightOnMouse
{
    public abstract class HighlightOnMouseComponent : SceneNodeComponentBase<HighlightOnMouseComponent>,
        IVisualComponent
    {
        private readonly IHighlightOnMouseService highlightOnMouseService;

        public abstract string GroupName { get; set; }

        private bool wasMouseOver;

        private static readonly IVisualEffect[] HighlightedEffects = {new HighlightVisualEffect()};
        private static readonly IVisualEffect[] NonHighlightedEffects = {};

        protected HighlightOnMouseComponent(IHighlightOnMouseService highlightOnMouseService)
        {
            this.highlightOnMouseService = highlightOnMouseService;
            GroupName = "default";
        }

        // todo: implement mouse-in / mouse-out interaction events instead
        public override void OnRoutedEvent(IRoutedEvent evnt)
        {
            if (!(evnt is IMouseEvent mevent))
                return;
            var layer = mevent.Viewport.View.Layers.FirstOrDefault(x => x.VisibleScene == Node.Scene);
            if (layer == null)
                return;
            var rayHitInfo = new RayCastInfo(mevent.Viewport, layer, mevent.State.Position);
            var isMouseOver = Node.SearchComponents<IRayHittableComponent>()
                .Any(x => x.HitWithClick(rayHitInfo).Successful);
            if (!wasMouseOver && isMouseOver)
                highlightOnMouseService.OnObjectIn(this, GroupName);
            else if (wasMouseOver && !isMouseOver)
                highlightOnMouseService.OnObjectOut(this, GroupName);
            wasMouseOver = isMouseOver;
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            return EmptyArrays<IVisualElement>.Array;
        }

        public IEnumerable<IVisualEffect> GetVisualEffects()
        {
            return highlightOnMouseService.GroupIsHighlighted(GroupName)
                ? HighlightedEffects
                : NonHighlightedEffects;
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            if (!wasMouseOver || !message.Obj(this).ValueChanged(x => x.GroupName, out var valMessage))
                return;
            highlightOnMouseService.OnObjectOut(this, valMessage.OldValue);
            wasMouseOver = false;
        }
    }
}