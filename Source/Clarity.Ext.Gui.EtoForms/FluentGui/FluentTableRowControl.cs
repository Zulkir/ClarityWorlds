using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentTableRowControl<T> : IFluentTableRowControl
    {
        private TableRow etoTableRow;
        private readonly Action onLayoutChanged;
        private readonly Func<T> getObject;
        private readonly List<IFluentControl> cells;
        private IntSet32 rowVisibility;

        public TableRow EtoTableRow => etoTableRow;
        public Control EtoControl => null;
        public bool IsVisible => cells.Any(x => x.IsVisible);
        public T GetObject() => getObject();

        public FluentTableRowControl(Action onLayoutChanged, Func<T> getObject)
        {
            this.getObject = getObject;
            this.onLayoutChanged = onLayoutChanged;
            cells = new List<IFluentControl>();
            etoTableRow = new TableRow();
        }

        public IFluentGuiBuilder<T> Build()
        {
            cells.Clear();
            etoTableRow.Cells.Clear();
            return new FluentGuiBuilder<T>(GetObject, AddChild, OnChildLayoutChanged);
        }

        public void AddChild(IFluentControl control)
        {
            cells.Add(control);
        }

        public void OnChildLayoutChanged()
        {
            onLayoutChanged();
        }

        public void Update()
        {
            foreach (var row in cells.Where(x => x.IsVisible))
                row.Update();
            var newRowVisibility = BuildChildVisibility();
            if (newRowVisibility != rowVisibility)
                RebuildEto();
            rowVisibility = newRowVisibility;
        }

        private IntSet32 BuildChildVisibility()
        {
            var visibility = new IntSet32();
            for (var i = 0; i < cells.Count; i++)
                if (cells[i].IsVisible)
                    visibility = visibility.With(i);
            return visibility;
        }

        private void RebuildEto()
        {
            etoTableRow = new TableRow();
            foreach (var row in cells.Where(x => x.IsVisible))
                etoTableRow.Cells.Add(row.EtoControl);
            onLayoutChanged();
        }
    }
}