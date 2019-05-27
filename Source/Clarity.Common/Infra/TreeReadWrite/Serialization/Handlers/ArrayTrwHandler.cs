using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class ArrayTrwHandler<TItem> : ArrayTrwHandlerBase<TItem[], List<TItem>, TItem>
    {
        protected override TrwValueType TrwValueType => TrwValueType.Undefined;

        protected override IEnumerable<TItem> EnumerateItems(TItem[] array) => array;
        protected override List<TItem> CreateBuilder() => new List<TItem>();
        protected override void AddItem(List<TItem> builder, TItem value) => builder.Add(value);
        protected override TItem[] Finalize(List<TItem> builder) => builder.ToArray();
    }
}