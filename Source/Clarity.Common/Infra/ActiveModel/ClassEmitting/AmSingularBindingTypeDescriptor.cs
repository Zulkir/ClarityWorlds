using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmSingularBindingTypeDescriptor : IAmBindingTypeDescriptor
    {
        public string SupportedTypeFormatString => "T { get; set; } // SingularBinding";

        public bool TryCreateDescription(PropertyInfo property, out IAmBindingDescription description)
        {
            if (property.GetMethod != null && property.SetMethod != null)
            {
                description = new AmSingularBindingDescription(property);
                return true;
            }
            description = null;
            return false;
        }
    }
}