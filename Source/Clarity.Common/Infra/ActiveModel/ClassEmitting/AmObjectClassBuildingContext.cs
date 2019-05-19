using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmObjectClassBuildingContext
    {
        public Type AmClass { get; }
        public TypeBuilder TypeBuilder { get; }
        public AmObjectTypeDescription Desc { get; }
        public AmObjectClassBuildingFields Fields { get; }

        public AmObjectClassBuildingContext(Type amClass, TypeBuilder typeBuilder, IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors)
        {
            AmClass = amClass;
            TypeBuilder = typeBuilder;
            Desc = new AmObjectTypeDescription(amClass, bindingTypeDescriptors);
            Fields = new AmObjectClassBuildingFields(typeBuilder, Desc);
        }
    }
}