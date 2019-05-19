using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Clarity.Core.AppCore.UndoRedo
{
    public interface ICommonUndoRedo
    {
        void Add<TItem>(ICollection<TItem> collection, TItem item);
        void Add<TStableObj, TItem>(TStableObj stableObj, Func<TStableObj, ICollection<TItem>> getCollection, TItem item);
        void Remove<TItem>(ICollection<TItem> collection, TItem item);
        void Remove<TStableObj, TItem>(TStableObj stableObj, Func<TStableObj, ICollection<TItem>> getCollection, TItem item);
        void ChangeProperty<TObj, TProp>(TObj obj, Expression<Func<TObj, TProp>> path, TProp newValue);
        void ChangeProperty<TObj, TProp>(TObj obj, Expression<Func<TObj, TProp>> path, TProp oldValue, TProp newValue);
        void ChangeProperty<TStableObj, TObj, TProp>(TStableObj stableObj, Func<TStableObj, TObj> getObj, Expression<Func<TObj, TProp>> property, TProp newValue);
        void ChangeProperty<TStableObj, TObj, TProp>(TStableObj stableObj, Func<TStableObj, TObj> getObj, Expression<Func<TObj, TProp>> property, TProp oldValue, TProp newValue);
    }
}