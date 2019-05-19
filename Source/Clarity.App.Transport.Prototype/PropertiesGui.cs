using System.Globalization;
using Clarity.Engine.Platforms;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype
{
    public class PropertiesGui
    {
        private readonly IPlayback playback;

        private readonly Label cTimestamp;
        private readonly Label cLastEntryInex;
        private readonly Label cSpeed;

        private readonly Label cEntryIndex;
        private readonly Label cEntryCodeName;
        private readonly TextBox cEntryBody;

        private readonly Label cSiteName;

        public TableLayout Layout { get; }

        private const float UpdateCooldown = 0.05f;
        private float remainingUpdateCooldown;

        public PropertiesGui(IPlayback playback)
        {
            this.playback = playback;
            cTimestamp = new Label();
            cLastEntryInex = new Label();
            cSpeed = new Label();

            cEntryIndex = new Label();
            cEntryCodeName = new Label();
            cEntryBody = new TextBox
            {
                ReadOnly = true,
                Height = 200
            };

            cSiteName = new Label();

            Layout = new TableLayout(
                new GroupBox
                {
                    Text = "Playback",
                    Content = new TableLayout(
                        new TableRow(new Label { Text = "Timestamp" }, cTimestamp),
                        new TableRow(new Label { Text = "Entry Index" }, cLastEntryInex),
                        new TableRow(new Label { Text = "Speed" }, cSpeed)
                    ),
                    Padding = new Padding(5)
                },
                new GroupBox
                {
                    Text = "Selected Package",
                    Content = new TableLayout(
                        new TableLayout(
                            new TableRow(new Label { Text = "Entry Index" }, cEntryIndex),
                            new TableRow(new Label { Text = "Entry Code" }, cEntryCodeName),
                            new TableRow(new Label { Text = "Entry Body" })
                        ),
                        cEntryBody)
                },
                new GroupBox
                {
                    Text = "Selected Site",
                    Content = new TableLayout(
                        new TableRow(new Label { Text = "Site" }, cSiteName)
                    )
                }
            ) { Width = 200, Padding = new Padding(5)};
        }

        public void Update(FrameTime frameTime)
        {
            remainingUpdateCooldown -= frameTime.DeltaSeconds;
            if (remainingUpdateCooldown > 0)
                return;
            Actualize();
            remainingUpdateCooldown = UpdateCooldown;
        }

        private void Actualize()
        {
            cTimestamp.Text = playback.AbsoluteTime.ToString("N0", CultureInfo.InvariantCulture);
            cLastEntryInex.Text = playback.LastEntryIndex.ToString();
            cSpeed.Text = playback.Speed.ToString("N0", CultureInfo.InvariantCulture);

            cEntryIndex.Text = playback.SelectedPackage?.Entry.Header.Sequence.ToString() ?? "";
            cEntryCodeName.Text = playback.SelectedPackage?.Entry.Header.Code.ToString() ?? "";
            cEntryBody.Text = playback.SelectedPackage?.Entry.BodyStr ?? "";

            cSiteName.Text = playback.SelectedSite ?? "";
        }
    }
}