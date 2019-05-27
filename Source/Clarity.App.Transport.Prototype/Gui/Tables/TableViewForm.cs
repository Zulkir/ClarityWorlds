using System;
using System.Collections.Generic;
using System.ComponentModel;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Platforms;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui.Tables
{
    public class TableViewForm : Form
    {
        private readonly TablesGui tablesGui;
        private readonly IAppRuntime appRuntime;
        private readonly IDataTable table;
        private readonly List<object[]> dataStore;
        private readonly GridView grid;
        private double timestamp;
        private float lastUpdateTime;

        private const float Cooldown = 0.1f;

        public TableViewForm(TablesGui tablesGui, IAppRuntime appRuntime, IDataTable table)
        {
            this.tablesGui = tablesGui;
            this.appRuntime = appRuntime;
            this.table = table;
            dataStore = new List<object[]>();

            grid = new GridView { DataStore = dataStore };
            foreach (var field in table.Fields)
            {
                grid.Columns.Add(new GridColumn
                {
                    HeaderText = field.Name,
                    DataCell = new TextBoxCell
                    {
                        Binding = Binding.Delegate(CreateBindingDelegate(field))
                    }
                });
            }

            Content = grid;
            Size = new Size(500, 500);
        }

        private Func<object[], string> CreateBindingDelegate(IDataField field)
        {
            switch (field.Type)
            {
                case DataFieldType.Int32: return x => ((int)x[field.Index]).ToString();
                case DataFieldType.Float: return x => ((float)x[field.Index]).ToString("N4");
                case DataFieldType.Pc64: return x => ((DPolyCubic)x[field.Index]).ValueAt(timestamp).ToString("N4");
                case DataFieldType.String: return x => (string)x[field.Index];
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            tablesGui.OnTableFormClosing(this);
            base.OnClosing(e);
        }

        public void Update(FrameTime frameTime)
        {
            if (frameTime.TotalSeconds - lastUpdateTime < Cooldown)
                return;
            timestamp = appRuntime.PlaybackService.AbsoluteTime;
            var data = appRuntime.DataRetrieval.GetLogForTable(table).GetStateAt(timestamp).GetTableState(table);
            if (dataStore.Count > data.RowCount)
                dataStore.RemoveRange(data.RowCount, dataStore.Count - data.RowCount);
            while(dataStore.Count < data.RowCount)
                dataStore.Add(new object[table.Fields.Count]);
            for (var i = 0; i < data.RowCount; i++)
            {
                var storeItem = dataStore[i];
                for (var j = 0; j < table.Fields.Count; j++)
                    storeItem[j] = data.GetValueAbstractByIndex(i, j);
            }
            grid.DataStore = dataStore;
            lastUpdateTime = frameTime.TotalSeconds;
        }
    }
}