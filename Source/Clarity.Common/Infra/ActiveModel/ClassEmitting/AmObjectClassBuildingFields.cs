using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmObjectClassBuildingFields
    {
        private readonly Type amClass;
        private readonly TypeBuilder typeBuilder;
        private readonly Dictionary<IAmBindingDescription, FieldBuilder> bindings;
        private readonly Dictionary<int, FieldBuilder> handlers;
        private int handlerDisambiguator;

        public AmObjectClassBuildingFields(TypeBuilder typeBuilder, AmObjectTypeDescription typeDesc)
        {
            this.typeBuilder = typeBuilder;
            amClass = typeDesc.AmClass;
            bindings = new Dictionary<IAmBindingDescription, FieldBuilder>();
            handlers = new Dictionary<int, FieldBuilder>();
        }

        public FieldBuilder GetOrAddBindingField(IAmBindingDescription propertyDesc) => 
            bindings.GetOrAdd(propertyDesc, CreateBindingField);

        private FieldBuilder CreateBindingField(IAmBindingDescription propertyDesc) => 
            typeBuilder.DefineField("b" + propertyDesc.Property.Name, propertyDesc.MakeBindingType(amClass),
                                    FieldAttributes.Private | FieldAttributes.InitOnly);

        public FieldBuilder GetOrAddHandlerField(int index, Type handlerType) =>
            handlers.GetOrAdd(index, i => CreateHandlerField(handlerType));

        private FieldBuilder CreateHandlerField(Type handlerType)
        {
            var index = Interlocked.Increment(ref handlerDisambiguator);
            return typeBuilder.DefineField("h" + index.ToString(), handlerType,
                                           FieldAttributes.Private | FieldAttributes.InitOnly);
        }
    }
}