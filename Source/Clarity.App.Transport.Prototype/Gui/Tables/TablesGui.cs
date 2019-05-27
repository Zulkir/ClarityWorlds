using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Engine.Platforms;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui.Tables
{
    public class TablesGui
    {
        public GroupBox Control { get; }

        private readonly IAppRuntime appRuntime;
        private readonly List<TableViewForm> tableViewForms;
        private IDataTable[] tables;

        public TablesGui(IAppRuntime appRuntime)
        {
            this.appRuntime = appRuntime;
            Control = new GroupBox
            {
                Text = "Tables"
            };
            tables = new IDataTable[0];
            tableViewForms = new List<TableViewForm>();
        }

        public void OnTableFormClosing(TableViewForm tableViewForm)
        {
            tableViewForms.Remove(tableViewForm);
        }

        public void Update(FrameTime frameTime)
        {
            var tableCount = 0;
            var foundDifference = false;
            foreach (var table in appRuntime.DataRetrieval.AllTables())
            {
                if (tables.Length <= tableCount || tables[tableCount] != table)
                {
                    foundDifference = true;
                    break;
                }
                tableCount++;
            }

            if (foundDifference || tableCount != tables.Length)
            {
                Rebuild();
                return;
            }

            foreach (var tableViewForm in tableViewForms)
                tableViewForm.Update(frameTime);
        }

        private void Rebuild()
        {
            var tableViewFormsCopy = tableViewForms.ToArray();
            foreach (var tableViewForm in tableViewFormsCopy)
                tableViewForm.Close();
            tables = appRuntime.DataRetrieval.AllTables().ToArray();
            Control.Content = new TableLayout(tables.Select(table =>
            {
                var button = new Button
                {
                    Text = table.Name,
                };
                button.Click += (s, a) =>
                {
                    var form = new TableViewForm(this, appRuntime, table);
                    tableViewForms.Add(form);
                    form.Show();
                };
                return new TableRow(button);
            }));
        }
    }
}