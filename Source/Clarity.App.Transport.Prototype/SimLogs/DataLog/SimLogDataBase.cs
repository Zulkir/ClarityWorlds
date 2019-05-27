using System;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.SimLogs.DataLog
{
    public class SimLogDataBase : DataBase
    {
        public SitesTable SitesTable { get; }
        public MessagesTable MessagesTable { get; }

        public SimLogDataBase(Func<int> generateTableId) : base(new IDataTable[]
        {
            new SitesTable(generateTableId()),
            new MessagesTable(generateTableId()), 
        })
        {
            SitesTable = (SitesTable)Tables[0];
            MessagesTable = (MessagesTable)Tables[1];
        }
    }
}