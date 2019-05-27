using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Queries.Visual;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.App.Transport.Prototype.Temp
{
    public class SiteVisualQuery : IVisualQuery
    {
        private readonly IAppRuntime runtime;
        private readonly Filter filter;
        private readonly List<(double timestamp, Dictionary<int, Vector2> positions)> ownLog;

        private IDataTable sitePositionsTable;
        private IDataField sitePositionsTableXField;
        private IDataField sitePositionsTableYField;
        private string invalidErrorMessage;
        private double timestamp;

        public ISceneNode SceneNode { get; }
        

        public SiteVisualQuery(IAppRuntime runtime)
        {
            this.runtime = runtime;
            filter = new Filter(this);
            ownLog = new List<(double, Dictionary<int, Vector2>)>();
            SceneNode = AmFactory.Create<SceneNode>();
        }

        public void OnAttached()
        {
            RebuildOwnLog(double.MinValue);
        }

        public void OnTableLayoutChanged()
        {
            var oldSitePositionsTable = sitePositionsTable;
            var oldSitePositionsTableXField = sitePositionsTableXField;
            var oldSitePositionsTableYField = sitePositionsTableYField;

            if (!runtime.DataRetrieval.TryGetTable("SitePositions", out sitePositionsTable))
            {
                invalidErrorMessage = "Table 'Sites' not found.";
                return;
            }
            if (!sitePositionsTable.TryGetField("X", out sitePositionsTableXField))
            {
                invalidErrorMessage = "Field 'X' not found in table 'SitePositions'.";
                return;
            }
            if (!sitePositionsTable.TryGetField("Y", out sitePositionsTableYField))
            {
                invalidErrorMessage = "Field 'Y' not found in table 'SitePositions'.";
                return;
            }
            invalidErrorMessage = null;
            if (oldSitePositionsTable != sitePositionsTable || 
                oldSitePositionsTableXField != sitePositionsTableXField || 
                oldSitePositionsTableYField != sitePositionsTableYField)
                RebuildOwnLog(double.MinValue);
        }

        public bool CheckIsValid(out string errorMessage)
        {
            errorMessage = invalidErrorMessage;
            return invalidErrorMessage == null;
        }

        public void OnTimestampChanged(double timestamp)
        {
            this.timestamp = timestamp;
        }

        public void Update(FrameTime frameTime)
        {
            var logIndex = ownLog.Count - 1;
            while (logIndex >= 0 && ownLog[logIndex].timestamp > timestamp)
                logIndex--;

            if (logIndex < 0)
            {
                SceneNode.ChildNodes.Clear();
                return;
            }

            var positions = ownLog[logIndex].positions;
            while (SceneNode.ChildNodes.Count < positions.Count)
            {
                var node = SiteVisualComponent.CreateNode();
                SceneNode.ChildNodes.Add(node);
            }

            var i = 0;
            foreach (var pos in positions.Values)
            {
                SceneNode.ChildNodes[i].Transform = Transform.Translation(new Vector3(pos.X, 0, pos.Y));
                i++;
            }
        }

        private class Filter : IDataLogFilter
        {
            private readonly SiteVisualQuery self;
            public Filter(SiteVisualQuery self) { this.self = self; }
            public bool AcceptsTable(IDataTable table) => table == self.sitePositionsTable;
            public bool AcceptsField(IDataField field) => field == self.sitePositionsTableXField || field == self.sitePositionsTableYField;
            public bool AcceptsEntry(IDataLogEntry entry) => AcceptsTable(entry.Table) && (entry.Field == null || AcceptsField(entry.Field));
        }

        public void OnDataLogUpdated(IDataLogUpdatedEvent evnt)
        {
            if (evnt.TablesAffected.Any(x => filter.AcceptsTable(x)))
                RebuildOwnLog(evnt.StartTimestamp);
        }

        private void RebuildOwnLog(double startTimestamp)
        {
            while (ownLog.HasItems() && ownLog[ownLog.Count - 1].timestamp >= startTimestamp)
                ownLog.RemoveAt(ownLog.Count - 1);
            foreach (var readingState in runtime.DataRetrieval.ReadFiltered(filter, startTimestamp, double.MaxValue))
            {
                var timestamp = readingState.Entry.Timestamp;
                if (!ownLog.HasItems() || ownLog[ownLog.Count - 1].timestamp != timestamp)
                    ownLog.Add((timestamp, new Dictionary<int, Vector2>()));
                var positions = ownLog[ownLog.Count - 1].positions;
                positions.Clear();
                var state = readingState.State.GetTableStateById(sitePositionsTable.Id);
                foreach (var key in state.Keys)
                {
                    var x = state.GetValue<float>(key, sitePositionsTableXField.Index);
                    var y = state.GetValue<float>(key, sitePositionsTableYField.Index);
                    positions[key] = new Vector2(x, y);
                }
            }
        }
    }
}