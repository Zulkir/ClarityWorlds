using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmObjectTypeDescription
    {
        public Type AmClass { get; }
        public string Name { get; }
        public Type ParentType { get; }
        public IReadOnlyList<IAmBindingDescription> Bindings { get; }

        public AmObjectTypeDescription(Type amClass, IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors)
        {
            AmClass = amClass;
            Name = AmClass.Name;
            ParentType = ChooseParentType(AmClass);
            Bindings = AmClass.GetProperties()
                .Where(p => p.GetMethod?.IsAbstract ?? false)
                .Select(p => CreateBindingDescription(p, bindingTypeDescriptors))
                .ToArray();
        }

        private static Type ChooseParentType(Type amClass)
        {
            var interfaces = amClass.GetAllInterfaces();
            var interfaceWithParent =
                interfaces.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAmObject<>));
            return interfaceWithParent != null 
                ? interfaceWithParent.GetGenericArguments().Single() 
                : typeof(IAmObject);
        }
        
        private static IAmBindingDescription CreateBindingDescription(PropertyInfo property, IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors)
        {
            foreach (var descriptor in bindingTypeDescriptors)
            {
                IAmBindingDescription description;
                if (descriptor.TryCreateDescription(property, out description))
                    return description;
            }
            throw new Exception(
                $"Failed to create a binding property description for a property '{property.DeclaringType?.Name}.{property.Name}'.\r\n" +
                "The supported types are:\r\n" +
                string.Join("\r\n", bindingTypeDescriptors.Select(x => x.SupportedTypeFormatString)));
        }
    }
}