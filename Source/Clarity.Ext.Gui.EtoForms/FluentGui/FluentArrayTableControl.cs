using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    // todo: enable by refactoring controls to templates
    public class FluentArrayTableControl<T> : IFluentControl<IEnumerable<T>>
    {
        private TableLayout etoTableLayout;
        private readonly Action onLayoutChanged;
        private readonly Func<IEnumerable<T>> getObject;
        private readonly List<IFluentTableRowControl> rows;
        private readonly List<T> itemObjects;

        public Control EtoControl => etoTableLayout;
        public bool IsVisible => rows.Any(x => x.IsVisible);
        public IEnumerable<T> GetObject() => getObject();

        public FluentArrayTableControl(Action onLayoutChanged, Func<IEnumerable<T>> getObject)
        {
            this.onLayoutChanged = onLayoutChanged;
            this.getObject = getObject;
            rows = new List<IFluentTableRowControl>();
            itemObjects = new List<T>();
            etoTableLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
        }

        public IFluentTableBuilder<T> Build()
        {
            rows.Clear();
            etoTableLayout.Rows.Clear();
            //return new FluentGuiBuilder<T>(() => throw new NotSupportedException(), AddRow, OnChildLayoutChanged);
            throw new NotImplementedException();
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
            var oldCount = itemObjects.Count;
            itemObjects.Clear();
            itemObjects.AddRange(getObject());

            if (rows.Count < itemObjects.Count)
                for (var i = rows.Count; i < itemObjects.Count; i++)
                {
                    var iLoc = i;
                    var row = new FluentTableRowControl<T>(OnChildLayoutChanged, () => itemObjects[iLoc]);
                    // TODO fill the row with controls
                    rows.Add(row);
                }

            foreach (var row in rows.Take(itemObjects.Count))
                row.Update();

            if (itemObjects.Count != oldCount)
                RebuildEto();
        }

        private void RebuildEto()
        {
            etoTableLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            foreach (var row in rows.Take(itemObjects.Count))
                etoTableLayout.Rows.Add(row.EtoTableRow);
            onLayoutChanged();
        }
    }
}