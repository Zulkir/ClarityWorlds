using System;
using System.Collections.ObjectModel;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Collections
{
    public static class ObservableCollectionExtensions
    {
        public static void Subscribe<T>(this ObservableCollection<T> collection, Action<T> onAdd, Action<T> onRemove)
        {
            collection.CollectionChanged += (sender, args) =>
            {
                if (args.OldItems != null)
                    foreach (var item in args.OldItems)
                        onRemove((T)item);
                if (args.NewItems != null)
                    foreach (var item in args.NewItems)
                        onAdd((T)item);
            };
        }
    }
}