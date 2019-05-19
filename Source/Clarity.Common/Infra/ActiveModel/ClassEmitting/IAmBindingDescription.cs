using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public interface IAmBindingDescription
    {
        PropertyInfo Property { get; }
        AmBindingFlags Flags { get; }
        Type MakeBindingType(Type selfType);
        MethodInfo MakePropertyGetMethod(Type selfType);
        MethodInfo MakePropertySetMethod(Type selfType);
        [CanBeNull ]string BuildGetterString(string bindingRef);
        [CanBeNull ]string BuildSetterString(string bindingRef);
    }
}