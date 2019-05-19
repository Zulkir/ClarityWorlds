using System.Collections.Generic;
using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmListBindingTypeDescriptor : IAmBindingTypeDescriptor
    {
        public string SupportedTypeFormatString => "IList<T> { get; } // ListBinding";

        public bool TryCreateDescription(PropertyInfo property, out IAmBindingDescription description)
        {
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) &&
                property.GetMethod != null && property.SetMethod == null)
            {
                description = new AmListBindingDescription(property);
                return true;
            }
            description = null;
            return false;
        }
    }
}