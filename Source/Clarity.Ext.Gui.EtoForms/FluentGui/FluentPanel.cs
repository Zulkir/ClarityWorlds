using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentPanel<T> : IFluentContainerControl<T>
    {
        private readonly Panel etoPanel;
        private readonly Func<T> getObject;
        private readonly Func<T, bool> isVisible;
        private bool contentVisibility;
        private bool layoutDirty;

        public IFluentControl Content { get; set; }

        public Control EtoControl => etoPanel;
        public bool IsVisible => isVisible(getObject());
        public T GetObject() => getObject();

        public int Width
        {
            get => etoPanel.Width;
            set => etoPanel.Width = value;
        }

        public FluentPanel(Func<T> getObject, Func<T, bool> isVisible)
        {
            this.getObject = getObject;
            this.isVisible = isVisible;
            etoPanel = new Panel();
        }

        public IFluentGuiBuilder<T> Build()
        {
            Content = null;
            return new FluentGuiBuilder<T>(this);
        }

        public void AddChild(IFluentControl control)
        {
            if (Content != null)
                throw new InvalidOperationException("FluentPanel can have only one child control.");
            Content = control;
        }

        public void OnChildLayoutChanged()
        {
            layoutDirty = true;
        }

        public void Update()
        {
            var newContentVisibility = Content?.IsVisible ?? false;
            if (Content?.IsVisible ?? false)
                Content.Update();
            if (layoutDirty || newContentVisibility != contentVisibility)
            {
                etoPanel.Content = Content?.EtoControl;
                layoutDirty = false;
                contentVisibility = newContentVisibility;
            }
        }
    }
}
