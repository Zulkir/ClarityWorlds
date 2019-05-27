using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Databases.LogEntries;
using Clarity.App.Transport.Prototype.DataSources;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.SimLogs.DataLog
{
    public class SimLogFileDataSource : IDataSource
    {
        private readonly List<IDataLogEntry> dataEntries;
        private readonly SimLogDataBase dataBase;
        private IDataSourceReceiver receiver;

        public SimLogFileDataSource(Func<Stream> openFile, bool compressed, Func<int> generateTableId)
        {
            dataEntries = new List<IDataLogEntry>();
            var delayedEntries = new List<IDataLogEntry>();
            var siteKeysByName = new Dictionary<string, int>();

            dataBase = new SimLogDataBase(generateTableId);
            var sitesTable = dataBase.SitesTable;
            var messagesTable = dataBase.MessagesTable;

            using (var stream = openFile())
            using (var reader = new SimLogReader(stream, compressed))
            {
                foreach (var simEntry in reader.ReadEntries())
                {
                    var timestamp = (double)simEntry.Header.Systime;
                    foreach (var delayedEntry in delayedEntries.Where(x => x.Timestamp <= timestamp).OrderBy(x => x.Timestamp))
                        dataEntries.Add(delayedEntry);
                    for (var i = delayedEntries.Count - 1; i >= 0; i--)
                        if (delayedEntries[i].Timestamp <= timestamp)
                            delayedEntries.RemoveAt(i);

                    (string from, string to, int size, float time, int cost) bodyData;
                    switch (simEntry.Header.Code)
                    {
                        case SimLogEntryCode.Read:
                        case SimLogEntryCode.Update:
                        case SimLogEntryCode.Create:
                        {
                            var body = CrudSimLogEntryBody.Parse(simEntry.BodyStr);
                            bodyData = (body.FromSite, body.ToSite, body.TupleSize, body.CommunicationTime, body.CommunicationCost);
                            break;
                        }
                        case SimLogEntryCode.MigrationStart:
                        case SimLogEntryCode.NewCopy:
                        case SimLogEntryCode.RemoveCopy:
                        case SimLogEntryCode.MigrationEnd:
                        {
                            var body = CrudSimLogEntryBody.Parse(simEntry.BodyStr);
                            bodyData = (body.FromSite, body.ToSite, body.TupleSize, body.CommunicationTime, body.CommunicationCost);
                            break;
                        }
                        default:
                            continue;
                    }

                    void ProcessSite(string name, out int id)
                    {
                        if (siteKeysByName.TryGetValue(name, out id))
                            return;
                        id = siteKeysByName.Count + 1;
                        siteKeysByName.Add(name, id);
                        dataEntries.Add(new InsertRowLogEntry(timestamp, sitesTable, id));
                        dataEntries.Add(new SetValueLogEntry<string>(timestamp, sitesTable, id, sitesTable.NameField, name));
                    }

                    ProcessSite(bodyData.from, out var fromSiteId);
                    ProcessSite(bodyData.to, out var toSiteId);

                    var rowKey = simEntry.Header.Sequence;
                    dataEntries.Add(new InsertRowLogEntry(timestamp, messagesTable, rowKey));
                    //dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.SequenceField, simEntry.Header.Sequence));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.CodeField, (int)simEntry.Header.Code));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.AppIdField, simEntry.Header.AppId));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.FromField, fromSiteId));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.ToField, toSiteId));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.SizeField, bodyData.size));
                    dataEntries.Add(new SetValueLogEntry<float>(timestamp, messagesTable, rowKey, messagesTable.TimeField, bodyData.time));
                    dataEntries.Add(new SetValueLogEntry<int>(timestamp, messagesTable, rowKey, messagesTable.CostField, bodyData.cost));

                    delayedEntries.Add(new DeleteRowLogEntry(timestamp + bodyData.time, messagesTable, rowKey));
                }
            }
        }

        public void Dispose()
        {
        }

        public void AttachReceiver(IDataSourceReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Open()
        {
            receiver.Reset(dataBase.Tables);
            receiver.BeginPack();
            foreach (var entry in dataEntries)
                receiver.NewEntry(entry);
            receiver.EndPack();
        }

        public void OnNewFrame(FrameTime frameTime)
        {
        }
    }
}