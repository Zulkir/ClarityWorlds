using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Collections
{
    public delegate void OnRelaxedObservableListItemEvent<in T>(T item, int index);

    public interface IRelaxedObservableList<T> : IList<T>
    {
        event OnRelaxedObservableListItemEvent<T> ItemAdded;
        event OnRelaxedObservableListItemEvent<T> ItemRemoved;
    }
}