using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public abstract class ObjectTrwHandlerBase<TObj, TBuilder, TValue, TProp> : TrwSerializationHandlerBase<TObj>
    {
        public override bool ContentIsProperties => true;

        protected abstract IEnumerable<TProp> EnumerateProps(TObj obj);
        protected abstract string GetPropName(TProp prop);
        protected abstract Type GetPropType(TProp prop);
        protected abstract TValue GetPropValue(TObj obj, TProp prop);
        
        protected abstract TBuilder CreateBuilder();
        protected abstract void SetPropValue(TBuilder builder, object boxedBuilder, TProp prop, TValue value);
        protected abstract TObj Finalize(TBuilder builder);

        protected abstract bool TryGetProp(TBuilder obj, string name, out TProp prop);

        public override void SaveContent(ITrwSerializationWriteContext context, TObj value)
        {
            foreach (var prop in EnumerateProps(value))
            {
                var name = GetPropName(prop);
                var type = GetPropType(prop);
                var val = GetPropValue(value, prop);
                context.WriteProperty(name, type, val);
            }
        }

        public override TObj LoadContent(ITrwSerializationReadContext context)
        {
            var obj = CreateBuilder();
            var boxed = (object)obj;
            while (context.Reader.TokenType != TrwTokenType.EndObject)
            {
                var name = context.Reader.ValueAsString;
                if (TryGetProp(obj, name, out var prop))
                {
                    context.Reader.MoveNext();
                    var type = GetPropType(prop);
                    var val = (TValue)context.Read(type);
                    SetPropValue(obj, boxed, prop, val);
                }
                else
                {
                    context.Reader.Skip();
                }
            }
            return Finalize((TBuilder)boxed);
        }
    }
}