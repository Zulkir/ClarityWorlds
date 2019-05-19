namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public static class TrwSerializationReadContextExtensions
    {
        public static T ReadProperty<T>(this ITrwSerializationReadContext context, string propertyName)
        {
            context.Reader.CheckPropertyAndMoveNext(propertyName);
            return context.Read<T>();
        }
    }
}