using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGroupBox<T> : IFluentContainerControl
    {
        private readonly GroupBox etoGroupBox;
        private readonly Func<T> getObject;
        private readonly Func<T, bool> isVisible;
        private bool contentVisibility;
        private bool layoutDirty;

        public IFluentControl Content { get; set; }

        public Control EtoControl => etoGroupBox;
        public bool IsVisible => isVisible(getObject());
        public T GetObject() => getObject();

        public int Width
        {
            get => etoGroupBox.Width;
            set => etoGroupBox.Width = value;
        }

        public FluentGroupBox(string text, Func<T> getObject, Func<T, bool> isVisible)
        {
            this.getObject = getObject;
            this.isVisible = isVisible;
            etoGroupBox = new GroupBox{Text = text};
        }

        public IFluentGuiBuilder<T> Build()
        {
            Content = null;
            return new FluentGuiBuilder<T>(GetObject, AddChild, OnChildLayoutChanged);
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
                etoGroupBox.Content = Content?.EtoControl;
                layoutDirty = false;
                contentVisibility = newContentVisibility;
            }
        }
    }
}