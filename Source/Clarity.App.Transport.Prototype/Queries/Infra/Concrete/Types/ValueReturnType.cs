using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types
{
    public class ValueReturnType : IInfraQueryReturnType
    {
        public DataFieldType ValueType { get; }

        public ValueReturnType(DataFieldType valueType)
        {
            ValueType = valueType;
        }
    }
}