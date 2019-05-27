using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types
{
    public struct TableRowEnumeration
    {
        public IDataTableState State;
        public IEnumerable<int> Keys;

        public TableRowEnumeration(IDataTableState state, IEnumerable<int> keys)
        {
            State = state;
            Keys = keys;
        }
    }
}