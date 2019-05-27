using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.DataSources;
using Clarity.App.Transport.Prototype.Temp;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Resources;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class OriginDataLogService : IOriginDataLogService, IDataSourceReceiver
    {
        private readonly Lazy<IAppRuntime> appRuntimeLazy;
        private readonly IEventRoutingService eventRoutingService;
        private readonly ITableService tableService;
        private readonly MutableDataLog dataLog;
        //private readonly DataLogNewEntryEvent newEntryEvent;
        private readonly DataLogTablesEvent tablesEvent;
        private readonly List<IDataTable> packTables;
        private bool isReceivingPack;
        private double packMinTimestamp;

        private readonly IEmbeddedResources embeddedResources;

        private IAppRuntime AppRuntime => appRuntimeLazy.Value;

        public IDataLog DataLog => dataLog;

        public OriginDataLogService(Lazy<IAppRuntime> appRuntimeLazy, IEventRoutingService eventRoutingService, ITableService tableService, IEmbeddedResources embeddedResources)
        {
            this.appRuntimeLazy = appRuntimeLazy;
            this.eventRoutingService = eventRoutingService;
            this.tableService = tableService;
            this.embeddedResources = embeddedResources;
            dataLog = new MutableDataLog();
            //newEntryEvent = new DataLogNewEntryEvent();
            tablesEvent = new DataLogTablesEvent();
            packTables = new List<IDataTable>();
            eventRoutingService.Subscribe<IDataSourceChangedEvent>(typeof(IOriginDataLogService), nameof(OnDataSourceChanged), OnDataSourceChanged);
        }

        private void OnDataSourceChanged(IDataSourceChangedEvent evnt)
        {
            foreach (var table in dataLog.Tables)
                tableService.DropTable(table.Id);
            evnt.NewDataSource.AttachReceiver(this);
            evnt.NewDataSource.Open();
        }

        #region IDataSourceReceiver implementation
        public void Reset(IReadOnlyList<IDataTable> tables)
        {
            var oldTables = dataLog.Tables;
            dataLog.Reset(tables);
            tablesEvent.Reset(tables, EmptyArrays<IDataTable>.Array, oldTables);
            eventRoutingService.FireEvent<IDataLogTablesEvent>(tablesEvent);
            // todo: remove
            AppRuntime.DataQueries.AddQuery(new SitePositionsDataQuery(AppRuntime, tables.Single(x => x.Name == "Sites").Id));
            AppRuntime.DataQueries.AddQuery(new SiteTrafficDataQuery(AppRuntime, tableService));
            AppRuntime.VisualQueries.AddQuery(new SiteVisualQuery(AppRuntime));
            AppRuntime.VisualQueries.AddQuery(new SiteTrafficVisualQuery(AppRuntime.DataRetrieval, AppRuntime.PlaybackService, embeddedResources));
        }

        public void NewEntry(IDataLogEntry entry)
        {
            dataLog.AddEntry(entry);
            if (!isReceivingPack)
            {
                //newEntryEvent.Reset(entry);
                //eventRoutingService.FireEvent<IDataLogNewEntryEvent>(newEntryEvent);
                // todo: optimize allocation
                eventRoutingService.FireEvent<IDataLogUpdatedEvent>(new DataLogUpdatedEvent(entry.Timestamp,
                    new[] {entry.Table}));
            }
            else
            {
                packTables.AddUnique(entry.Table);
            }
        }

        public void BeginPack()
        {
            isReceivingPack = true;
            packMinTimestamp = double.MaxValue;
            packTables.Clear();
        }

        public void EndPack()
        {
            isReceivingPack = false;
            // todo: optimize allocation
            eventRoutingService.FireEvent<IDataLogUpdatedEvent>(new DataLogUpdatedEvent(packMinTimestamp, packTables));
        }
        #endregion
    }
}