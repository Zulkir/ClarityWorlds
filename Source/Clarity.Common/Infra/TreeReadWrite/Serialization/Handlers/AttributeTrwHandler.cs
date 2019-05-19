using System;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class AttributeTrwHandler<T> : TrwSerializationHandlerBase<T>
    {
        private readonly Func<T> createObj;

        private readonly FieldInfo[] fields;
        private readonly PropertyInfo[] properties;

        public AttributeTrwHandler(ITrwAttributeObjectCreator objectCreator)
        {
            createObj = objectCreator.GetConstructor<T>();

            fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<TrwSerializeAttribute>() != null)
                .ToArray();

            properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<TrwSerializeAttribute>() != null && x.GetMethod != null && x.SetMethod != null)
                .ToArray();

            if (!fields.Any() && !properties.Any())
            {
                fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .ToArray();

                properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetMethod != null && x.SetMethod != null)
                    .ToArray();
            }
        }

        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, T value)
        {
            foreach (var field in fields)
                context.WriteProperty(field.Name, field.FieldType, field.GetValue(value));
            foreach (var property in properties.Reverse())
                context.WriteProperty(property.Name, property.PropertyType, property.GetValue(value));
        }

        public override T LoadContent(ITrwSerializationReadContext context)
        {
            var obj = createObj();
            var boxed = (object)obj;
            while (context.Reader.TokenType != TrwTokenType.EndObject)
            {
                var name = context.Reader.ValueAsString;
                var field = fields.FirstOrDefault(x => x.Name == name);
                var property = field == null ? properties.FirstOrDefault(x => x.Name == name) : null;
                context.Reader.MoveNext();
                if (field != null)
                {
                    var value = context.Read(field.FieldType);
                    field.SetValue(boxed, value);
                }
                else if (property != null)
                {
                    var value = context.Read(property.PropertyType);
                    property.SetValue(boxed, value);
                }
            }
            return (T)boxed;
        }
    }
}