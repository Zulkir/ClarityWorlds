using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding
{
    public interface ITrwDiffIdentityComparer
    {
        bool AreSameObject(IDictionary<string, object> o1, IDictionary<string, object> o2);
    }
}