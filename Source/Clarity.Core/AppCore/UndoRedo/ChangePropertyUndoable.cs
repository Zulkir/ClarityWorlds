using System;
using System.Reflection;

namespace Clarity.Core.AppCore.UndoRedo
{
    public class ChangePropertyUndoable<TStableObj, TObj, TProp> : IUndoable
    {
        private readonly TStableObj stableObject;
        private readonly Func<TStableObj, TObj> getObject;
        private readonly PropertyInfo propertyInfo;
        private readonly TProp newValue;
        private readonly TProp oldValue;
        
        public ChangePropertyUndoable(TStableObj stableObject, Func<TStableObj, TObj> getObject, PropertyInfo propertyInfo, TProp oldValue, TProp newValue)
        {
            this.stableObject = stableObject;
            this.getObject = getObject;
            this.propertyInfo = propertyInfo;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public void Apply() { propertyInfo.SetValue(getObject(stableObject), newValue); }
        public void Undo() { propertyInfo.SetValue(getObject(stableObject), oldValue); }
    }
}