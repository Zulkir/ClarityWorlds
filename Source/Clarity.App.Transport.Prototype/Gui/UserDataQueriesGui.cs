using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Gui.Queries;
using Clarity.App.Transport.Prototype.Queries.Data;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Engine.Platforms;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui
{
    public class UserDataQueriesGui
    {
        public GroupBox Control { get; }
        
        private readonly IDataQueriesService dataQueriesService;
        private readonly List<QueryEditForm> editForms;
        private IDataQuery[] queries;

        public UserDataQueriesGui(IDataQueriesService dataQueriesService)
        {
            this.dataQueriesService = dataQueriesService;

            Control = new GroupBox
            {
                Text = "Data Queries"
            };

            queries = new IDataQuery[0];
            editForms = new List<QueryEditForm>();
        }

        public void Update(FrameTime frameTime)
        {
            if (dataQueriesService.Queries.Count != queries.Length || 
                Enumerable.Range(0, queries.Length).Any(x => dataQueriesService.Queries[x] != queries[x]))
            {
                queries = dataQueriesService.Queries.ToArray();
                foreach (var editForm in editForms.ToArray())
                    editForm.Close();
                Control.Content = new TableLayout(queries.Select(x =>
                {
                    var editButton = new Button {Text = "Edit"};
                    editButton.Click += (s, a) =>
                    {
                        var form = new QueryEditForm(x, f => editForms.Remove(f));
                        editForms.Add(form);
                        form.Show();
                    };
                    return new TableRow(new Label {Text = x.ResultTable.Name}, editButton);
                }));
            }
            foreach (var editForm in editForms)
                editForm.Update(frameTime);
        }
    }
}