using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui 
{
    public class UniversalFluentControlTemplate<TParent> : IFluentControlTemplate<TParent>
    {
        private readonly Func<Func<TParent>, IFluentControl> instantiate;

        public UniversalFluentControlTemplate(Func<Func<TParent>, IFluentControl> instantiate)
        {
            this.instantiate = instantiate;
        }

        public IFluentControl Instantiate(Func<TParent> getParentObj)
        {
            return instantiate(getParentObj);
        }
    }
}