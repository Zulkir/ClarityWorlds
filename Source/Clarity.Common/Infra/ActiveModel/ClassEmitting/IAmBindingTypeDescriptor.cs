using System.Reflection;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public interface IAmBindingTypeDescriptor
    {
        string SupportedTypeFormatString { get; }
        bool TryCreateDescription(PropertyInfo property, out IAmBindingDescription description);
    }
}