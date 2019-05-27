using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class DataLogEntryEnumerationMerger
    {
        public static IEnumerable<IDataLogEntry> Merge(IEnumerable<IEnumerable<IDataLogEntry>> sourceEnumerations, IDataLogFilter filter)
        {
            var enumerators = sourceEnumerations.Select(x => x.GetEnumerator()).ToList();
            for (var i = enumerators.Count - 1; i >= 0; i--)
                if (!enumerators[i].MoveNext())
                {
                    enumerators[i].Dispose();
                    enumerators.RemoveAt(i);
                }
            while (enumerators.HasItems())
            {
                var i = Enumerable.Range(0, enumerators.Count).Minimal(x => enumerators[x].Current.Timestamp);
                if (filter.AcceptsEntry(enumerators[i].Current))
                    yield return enumerators[i].Current;
                if (!enumerators[i].MoveNext())
                {
                    enumerators[i].Dispose();
                    enumerators.RemoveAt(i);
                }
            }
        }
    }
}