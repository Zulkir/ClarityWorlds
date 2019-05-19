using System;
using System.Collections.Generic;

namespace Clarity.Core.AppCore.UndoRedo
{
    public class AddToCollectionUndoable<TStableObj, TItem> : IUndoable
    {
        private readonly TStableObj stableObj;
        private readonly Func<TStableObj, ICollection<TItem>> getCollection;
        private readonly TItem item;

        public AddToCollectionUndoable(TStableObj stableObj, Func<TStableObj, ICollection<TItem>> getCollection, TItem item)
        {
            this.stableObj = stableObj;
            this.getCollection = getCollection;
            this.item = item;
        }

        public void Apply() => getCollection(stableObj).Add(item);
        public void Undo() => getCollection(stableObj).Remove(item);
    }
}