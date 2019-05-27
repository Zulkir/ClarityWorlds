using System;
using System.Globalization;
using Clarity.App.Transport.Prototype.Queries.Data;
using Clarity.Engine.Platforms;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui.Queries
{
    public class QueryEditForm : Form
    {
        private readonly IDataQuery query;
        private readonly Action<QueryEditForm> onClosing;
        private readonly TextBox cQueryText;
        private readonly TextBox cParamText;
        private double remainingCooldown;
        private bool isActualizing;

        public QueryEditForm(IDataQuery query, Action<QueryEditForm> onClosing)
        {
            this.query = query;
            this.onClosing = onClosing;
            cQueryText = new TextBox { Enabled = false };
            cParamText = new TextBox();

            Content = new TableLayout(
                new TableRow(new Label{Text = "Query Text:"}, cQueryText),
                new TableRow(new Label{Text = "Param 1"}, cParamText));

            cParamText.TextChanged += (s, a) =>
            {
                if (isActualizing)
                    return;
                if (!double.TryParse(cParamText.Text, out var param)) 
                    return;
                query.Param0 = param;
                Actualize();
            };
            Actualize();

            Size = new Size(500, 100);
        }

        public void Update(FrameTime frameTime)
        {
            remainingCooldown -= frameTime.DeltaSeconds;
            if (remainingCooldown > 0)
                return;
            Actualize();
        }

        private void Actualize()
        {
            isActualizing = true;
            cQueryText.Text = query.Text;
            if (!cParamText.HasFocus)
                cParamText.Text = query.Param0.ToString(CultureInfo.InvariantCulture);
            remainingCooldown = 0.5;
            isActualizing = false;
        }

        public override void Close()
        {
            onClosing(this);
            base.Close();
        }
    }
}