using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class ListTrwHandler<TItem> : ArrayDiffableTrwHandlerBase<IList<TItem>, TItem>
    {
        protected override TrwValueType TrwValueType => TrwValueType.Undefined;
        protected override IEnumerable<TItem> EnumerateItems(IList<TItem> array) => array;
        protected override IList<TItem> CreateBuilder() => new List<TItem>();
        protected override void AddItem(IList<TItem> builder, TItem value) => builder.Add(value);
        protected override int GetCount(IList<TItem> array) => array.Count;
        protected override TItem GetItem(IList<TItem> array, int index) => array[index];
        protected override void RemoveAt(IList<TItem> array, int index) => array.RemoveAt(index);
        protected override void Insert(IList<TItem> array, int index, TItem value) => array.Insert(index, value);
    }
}