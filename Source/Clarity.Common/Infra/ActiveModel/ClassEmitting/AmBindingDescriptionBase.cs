using System;
using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public abstract class AmBindingDescriptionBase : IAmBindingDescription
    {
        public PropertyInfo Property { get; }
        public AmBindingFlags Flags { get; }
        
        protected AmBindingDescriptionBase(PropertyInfo property)
        {
            Property = property;
            Flags = DecodeFlags(property);
        }

        public abstract Type MakeBindingType(Type selfType);
        public abstract MethodInfo MakePropertyGetMethod(Type selfType);
        public abstract MethodInfo MakePropertySetMethod(Type selfType);
        public abstract string BuildGetterString(string bindingRef);
        public abstract string BuildSetterString(string bindingRef);

        private static AmBindingFlags DecodeFlags(PropertyInfo property)
        {
            var flags = AmBindingFlags.None;
            foreach (var attribute in property.CustomAttributes)
                if (attribute.AttributeType == typeof(AmReferenceAttribute))
                    flags |= AmBindingFlags.Reference;
                else if (attribute.AttributeType == typeof(AmDerivedAttribute))
                    flags |= AmBindingFlags.Derived;
            return flags;
        }
    }
}