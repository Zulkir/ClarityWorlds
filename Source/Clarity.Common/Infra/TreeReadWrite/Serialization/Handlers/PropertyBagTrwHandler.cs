using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class PropertyBagTrwHandler : ObjectDiffableTrwHandlerBase<IPropertyBag, object, string>
    {
        protected override IEnumerable<string> EnumerateProps(IPropertyBag obj) => obj.Keys;
        protected override string GetPropName(string prop) => prop;
        protected override Type GetPropType(string prop) => typeof(object);
        protected override object GetPropValue(IPropertyBag obj, string prop) => obj.SearchValue<object>(prop);
        protected override IPropertyBag CreateBuilder() => new PropertyBag();
        protected override void SetPropValue(IPropertyBag builder, object boxedBuilder, string prop, object value) => builder.SetValue(prop, value);

        protected override bool TryGetProp(IPropertyBag obj, string name, out string prop)
        {
            prop = name;
            return true;
        }
    }
}