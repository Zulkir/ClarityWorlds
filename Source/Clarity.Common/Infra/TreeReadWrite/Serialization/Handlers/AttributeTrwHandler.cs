using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class AttributeTrwHandler<T> : ObjectDiffableTrwHandlerBase<T, object, MemberInfo>
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

        protected override T CreateBuilder() => createObj();
        protected override IEnumerable<MemberInfo> EnumerateProps(T obj) => fields.Cast<MemberInfo>().Concat(properties);
        protected override string GetPropName(MemberInfo prop) => prop.Name;

        protected override Type GetPropType(MemberInfo prop)
        {
            switch (prop)
            {
                case FieldInfo fieldInfo: return fieldInfo.FieldType;
                case PropertyInfo propertyInfo: return propertyInfo.PropertyType;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override object GetPropValue(T obj, MemberInfo prop)
        {
            switch (prop)
            {
                case FieldInfo fieldInfo: return fieldInfo.GetValue(obj);
                case PropertyInfo propertyInfo: return propertyInfo.GetValue(obj);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void SetPropValue(T obj, object boxedBuilder, MemberInfo prop, object value)
        {
            switch (prop)
            {
                case FieldInfo fieldInfo: fieldInfo.SetValue(boxedBuilder, value); break;
                case PropertyInfo propertyInfo: propertyInfo.SetValue(boxedBuilder, value); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override bool TryGetProp(T obj, string name, out MemberInfo prop)
        {
            prop = EnumerateProps(obj).FirstOrDefault(x => x.Name == name);
            return prop != null;
        }
    }
}