using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class AmObjectTrwHandler<TObj> : ObjectDiffableTrwHandlerBase<TObj, object, IAmBinding>
        where TObj : class, IAmObject
    {
        private readonly IAmObjectInstantiator<TObj> instantiator;

        public AmObjectTrwHandler(IAmDiBasedObjectFactory objectFactory)
        {
            instantiator = objectFactory.GetInstantiator<TObj>();
        }

        public override bool ContentIsProperties => true;
        protected override TObj CreateBuilder() => instantiator.Instantiate();
        protected override IEnumerable<IAmBinding> EnumerateProps(TObj obj) => obj.Bindings;
        protected override string GetPropName(IAmBinding prop) => prop.PropertyName;
        protected override Type GetPropType(IAmBinding prop) => prop.AbstractValueType;
        protected override object GetPropValue(TObj obj, IAmBinding prop) => prop.GetAbstractValue();
        protected override void SetPropValue(TObj obj, object boxedBuilder, IAmBinding prop, object value) => prop.SetAbstractValue(value);

        protected override bool TryGetProp(TObj obj, string name, out IAmBinding prop)
        {
            prop = obj.Bindings.FirstOrDefault(x => x.PropertyName == name);
            return prop != null;
        }
    }
}