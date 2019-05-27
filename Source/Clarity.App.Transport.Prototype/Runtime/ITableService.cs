using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface ITableService
    {
        int GenerateId();
        IDataTable CreateTable(string name, IEnumerable<DataFieldDescription> fieldDescriptions);
        void DropTable(int id);
    }
}