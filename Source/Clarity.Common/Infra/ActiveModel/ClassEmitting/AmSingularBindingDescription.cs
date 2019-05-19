using System;
using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmSingularBindingDescription : AmBindingDescriptionBase
    {
        private Type ChildType { get; }

        public AmSingularBindingDescription(PropertyInfo property) : base(property)
        {
            ChildType = property.PropertyType;
        }

        public override Type MakeBindingType(Type selfType) => 
            typeof(AmSingularBinding<,>).MakeGenericType(selfType, ChildType);
        public override MethodInfo MakePropertyGetMethod(Type selfType) =>
            MakeBindingType(selfType).GetMethod("get_" + nameof(IAmSingularBinding<object>.Value));
        public override MethodInfo MakePropertySetMethod(Type selfType) =>
            MakeBindingType(selfType).GetMethod("set_" + nameof(IAmSingularBinding<object>.Value));

        public override string BuildGetterString(string bindingRef) => $"return {bindingRef}.Value;";
        public override string BuildSetterString(string bindingRef) => $"{bindingRef}.Value = value;";
    }
}