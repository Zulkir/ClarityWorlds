using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals;
using Eto.Drawing;
using Eto.Forms;


namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentPanel<T> : IFluentContainerControl<T>
    {
        public Panel Panel { get; }
        private readonly Func<T> getObject;
        private readonly Func<T, bool> isVisible;
        private readonly List<IFluentControl> children;
        private IntSet32 childVisibility;

        public Control EtoControl => Panel;
        public bool IsVisible => isVisible(getObject());
        public IList<IFluentControl> Children => children;

        public int Width
        {
            get => Panel.Width;
            set => Panel.Width = value;
        }

        public FluentPanel(Func<T> getObject, Func<T, bool> isVisible)
        {
            this.getObject = getObject;
            this.isVisible = isVisible;
            children = new List<IFluentControl>();
            Panel = new Panel();
        }

        public T GetObject() => getObject();

        public IFluentGuiBuilder<T> Build() => 
            new FluentGuiBuilder<T>(this);

        public void Update()
        {
            var newChildVisibility = BuildChildVisibility();
            if (newChildVisibility != childVisibility)
                RebuildLayout();
            childVisibility = newChildVisibility;
            foreach (var child in children.Where(x => x.IsVisible))
                child.Update();
        }

        private IntSet32 BuildChildVisibility()
        {
            var visibility = new IntSet32();
            for (var i = 0; i < children.Count; i++)
                if (children[i].IsVisible)
                    visibility = visibility.With(i);
            return visibility;
        }

        private void RebuildLayout()
        {
            var layout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            foreach (var child in children.Where(x => x.IsVisible))
                layout.Rows.Add(new TableRow(child.EtoControl));
            layout.Rows.Add(new TableRow());

            Panel.Content = layout;
        }
    }
}
