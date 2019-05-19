using System;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AmObjectTrwHandler<T> : ITrwSerializationHandler<T>
        where T : class, IAmObject
    {
        private readonly IAmObjectInstantiator<T> instantiator;

        public AmObjectTrwHandler(IAmDiBasedObjectFactory objectFactory)
        {
            instantiator = objectFactory.GetInstantiator<T>();
        }

        public Type Type => typeof(T);

        void ITrwSerializationHandler.SaveContent(ITrwSerializationWriteContext context, object value) => SaveContent(context, (T)value);
        object ITrwSerializationHandler.LoadContent(ITrwSerializationReadContext context) => LoadContent(context);

        public bool ContentIsProperties => true;

        public void SaveContent(ITrwSerializationWriteContext context, T value)
        {
            foreach (var binding in value.Bindings)
            {
                var val = binding.GetAbstractValue();
                var bindingValueType = binding.AbstractValueType;
                context.WriteProperty(binding.PropertyName, bindingValueType, val);
            }
        }
        
        public T LoadContent(ITrwSerializationReadContext context)
        {
            var obj = instantiator.Instantiate();
            while (context.Reader.TokenType != TrwTokenType.EndObject)
            {
                var name = context.Reader.ValueAsString;
                var binding = obj.Bindings.FirstOrDefault(x => x.PropertyName == name);
                if (binding == null)
                {
                    context.Reader.Skip();
                }
                else
                {
                    context.Reader.MoveNext();
                    var bindingValueType = binding.AbstractValueType;
                    var val = context.Read(bindingValueType);
                    binding.SetAbstractValue(val);
                }
            }
            return obj;
        }
    }
}