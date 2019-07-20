using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentTableControl<T> : IFluentTableControl<T>
    {
        private TableLayout etoTableLayout;
        private readonly Action onLayoutChanged;
        private readonly Func<T> getObject;
        private readonly List<IFluentTableRowControl> rows;
        private IntSet32 rowVisibility;

        public Control EtoControl => etoTableLayout;
        public bool IsVisible => rows.Any(x => x.IsVisible);
        public T GetObject() => getObject();

        public FluentTableControl(Action onLayoutChanged, Func<T> getObject)
        {
            this.onLayoutChanged = onLayoutChanged;
            this.getObject = getObject;
            rows = new List<IFluentTableRowControl>();
            etoTableLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
        }

        public IFluentGuiTableBuilder<T> Build()
        {
            rows.Clear();
            etoTableLayout.Rows.Clear();
            return new FluentGuiTableBuilder<T>(this);
        }

        public void OnChildLayoutChanged()
        {
            onLayoutChanged();
        }

        public void AddRow(IFluentTableRowControl row)
        {
            rows.Add(row);
        }

        public void Update()
        {
            foreach (var row in rows.Where(x => x.IsVisible))
                row.Update();
            var newRowVisibility = BuildChildVisibility();
            if (newRowVisibility != rowVisibility)
                RebuildEto();
            rowVisibility = newRowVisibility;
        }

        private IntSet32 BuildChildVisibility()
        {
            var visibility = new IntSet32();
            for (var i = 0; i < rows.Count; i++)
                if (rows[i].IsVisible)
                    visibility = visibility.With(i);
            return visibility;
        }

        private void RebuildEto()
        {
            etoTableLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            foreach (var row in rows.Where(x => x.IsVisible))
                etoTableLayout.Rows.Add(row.EtoTableRow);
            onLayoutChanged();
        }
    }
}