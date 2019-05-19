using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Clarity.Core.AppCore.UndoRedo
{
    public class CommonUndoRedo : ICommonUndoRedo
    {
        private readonly IUndoRedoService undoRedoService;

        public CommonUndoRedo(IUndoRedoService undoRedoService)
        {
            this.undoRedoService = undoRedoService;
        }

        private void Apply(IUndoable undoable)
        {
            undoRedoService.Apply(undoable);
        }
        
        public void Add<TItem>(ICollection<TItem> collection, TItem item) =>
            Add(collection, x => x, item);

        public void Add<TStableObj, TItem>(TStableObj stableObj, Func<TStableObj, ICollection<TItem>> getCollection, TItem item) => 
            Apply(new AddToCollectionUndoable<TStableObj, TItem>(stableObj, getCollection, item));

        public void Remove<TItem>(ICollection<TItem> collection, TItem item) =>
            Remove(collection, x => x, item);

        public void Remove<TStableObj, TItem>(TStableObj stableObj, Func<TStableObj, ICollection<TItem>> getCollection, TItem item) => 
            Apply(new AddToCollectionUndoable<TStableObj, TItem>(stableObj, getCollection, item).Reverse());

        public void ChangeProperty<TObj, TProp>(TObj obj, Expression<Func<TObj, TProp>> property, TProp newValue) =>
            ChangeProperty(obj, x => x, property, (TProp)GetPropertyInfo(property).GetValue(obj), newValue);

        public void ChangeProperty<TObj, TProp>(TObj obj, Expression<Func<TObj, TProp>> property, TProp oldValue, TProp newValue) =>
            ChangeProperty(obj, x => x, property, oldValue, newValue);

        public void ChangeProperty<TStableObj, TObj, TProp>(TStableObj stableObj, Func<TStableObj, TObj> getObj, Expression<Func<TObj, TProp>> property, TProp newValue) =>
            ChangeProperty(stableObj, getObj, property, (TProp)GetPropertyInfo(property).GetValue(getObj(stableObj)), newValue);
        
        public void ChangeProperty<TStableObj, TObj, TProp>(TStableObj stableObj, Func<TStableObj, TObj> getObj, 
                                                            Expression<Func<TObj, TProp>> property, TProp oldValue, TProp newValue) => 
            Apply(new ChangePropertyUndoable<TStableObj, TObj, TProp>(stableObj, getObj, GetPropertyInfo(property), oldValue, newValue));

        private static PropertyInfo GetPropertyInfo(LambdaExpression propertyExpression) => 
            (PropertyInfo)((MemberExpression)propertyExpression.Body).Member;
    }
}