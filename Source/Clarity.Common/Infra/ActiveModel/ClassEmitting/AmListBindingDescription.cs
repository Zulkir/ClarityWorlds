using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmListBindingDescription : AmBindingDescriptionBase
    {
        private Type ChildType { get; }

        public AmListBindingDescription(PropertyInfo property) : base(property)
        {
            Debug.Assert(property.PropertyType.IsGenericType, "property.PropertyType.IsGenericType");
            Debug.Assert(property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>), "property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)");
            ChildType = property.PropertyType.GetGenericArguments().Single();
        }

        public override Type MakeBindingType(Type selfType) => 
            typeof(AmListBinding<,>).MakeGenericType(selfType, ChildType);
        public override MethodInfo MakePropertyGetMethod(Type selfType) =>
            MakeBindingType(selfType).GetMethod("get_" + nameof(IAmListBinding<object>.Items));
        public override MethodInfo MakePropertySetMethod(Type selfType)
        {
            throw new InvalidOperationException("Trying to get a setter property method for a list binding.");
        }

        public override string BuildGetterString(string bindingRef) => $"return {bindingRef}.Items;";
        public override string BuildSetterString(string bindingRef) => null;
    }
}