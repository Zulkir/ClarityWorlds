using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Clarity.Common.CodingUtilities.Sugar.Wrappers.Emit;

namespace Clarity.Common.Infra.ActiveModel.ClassEmitting
{
    public class AmObjectClassFactory : IAmObjectClassFactory
    {
        private readonly IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors;
        private readonly ModuleBuilder moduleBuilder;
        private static int classNameDisambiguator;
        
        public AmObjectClassFactory(IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors)
        {
            this.bindingTypeDescriptors = bindingTypeDescriptors;
            var appDomain = AppDomain.CurrentDomain;
            var assemblyBuilder = appDomain.DefineDynamicAssembly(new AssemblyName("AmGeneratedObjects"), AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("AmGeneratedObjectsModule");
        }

        public IAmObjectInstantiator CreateObjectClass(Type objectInterface, Func<Type, object> getDefaultDependency)
        {
            var typeBuilder = DeclareType(objectInterface);
            var context = new AmObjectClassBuildingContext(objectInterface, typeBuilder, bindingTypeDescriptors);
            CreateConstructor(context);
            foreach (var bindingDesc in context.Desc.Bindings)
                CreateBindingMethods(context, bindingDesc);
            CreateInitBindingsMethod(context);
            var objectClass = typeBuilder.CreateType();
            var instantiatorType = typeof(AmObjectInstantiator<>).MakeGenericType(objectClass);
            return (IAmObjectInstantiator)instantiatorType.GetConstructors().Single().Invoke(new object[] { getDefaultDependency });
        }

        private TypeBuilder DeclareType(Type amClass)
        {
            var disambiguator = Interlocked.Increment(ref classNameDisambiguator);
            return moduleBuilder.DefineType($"_am_{amClass.Name}_{disambiguator}",
                                            TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                            amClass);
        }

        private static void CreateBindingMethods(AmObjectClassBuildingContext context, IAmBindingDescription bindingDesc)
        {
            CreateMethod(context, bindingDesc, bindingDesc.Property.GetMethod);
            if (bindingDesc.Property.SetMethod != null)
                CreateMethod(context, bindingDesc, bindingDesc.Property.SetMethod);
        }

        private static void CreateMethod(AmObjectClassBuildingContext context, IAmBindingDescription bindingDesc, MethodInfo method)
        {
            var methodBuilder = context.TypeBuilder.DefineMethod(method.Name,
                    MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig |
                    MethodAttributes.NewSlot | MethodAttributes.Virtual);
            methodBuilder.SetParameters(method.GetParameters().Select(x => x.ParameterType).ToArray());
            methodBuilder.SetReturnType(method.ReturnType);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());
            var field = context.Fields.GetOrAddBindingField(bindingDesc);
            if (method.Name.StartsWith("get_"))
            {
                il.Ldarg(0);
                il.Ldfld(field);
                il.Call(bindingDesc.MakePropertyGetMethod(context.AmClass));
                il.Ret();
            }
            else if (method.Name.StartsWith("set_"))
            {
                il.Ldarg(0);
                il.Ldfld(field);
                il.Ldarg(1);
                il.Call(bindingDesc.MakePropertySetMethod(context.AmClass));
                il.Ret();
            }
            else
            {
                throw new Exception($"Either getter or setter expected, but '{method.Name}' found.");
            }
            context.TypeBuilder.DefineMethodOverride(methodBuilder, method);
        }

        private void CreateConstructor(AmObjectClassBuildingContext context)
        {
            var baseConstructor = context.AmClass.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance).Single();
            var parameterTypes = baseConstructor.GetParameters().Select(x => x.ParameterType).ToArray();
            var constructorBuilder = context.TypeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard, parameterTypes);
            
            var il = new CilEmitterSugar(constructorBuilder.GetILGenerator());
            
            il.Ldarg(0);                // [this]
            for (int i = 0; i < parameterTypes.Length; i++)
                il.Ldarg(i + 1);
            il.Call(baseConstructor);   // []

            //il.Ldarg(0);
            //il.Callvirt(typeof(IAmObject).GetMethod(nameof(IAmObject.AmOnFinalized)));

            il.Ret();
        }

        private void CreateInitBindingsMethod(AmObjectClassBuildingContext context)
        {
            var methodBuilder = context.TypeBuilder.DefineMethod("AmInitBindings", MethodAttributes.Public | MethodAttributes.Virtual);
            methodBuilder.SetParameters(Type.EmptyTypes);
            methodBuilder.SetReturnType(typeof(List<IAmBinding>));
            methodBuilder.SetImplementationFlags(MethodImplAttributes.Managed);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());
            var varList = il.DeclareLocal(typeof(List<IAmBinding>));
            il.Newobj(typeof(List<IAmBinding>).GetConstructor(Type.EmptyTypes));
            il.Stloc(varList);
            foreach (var bindingDesc in context.Desc.Bindings)
            {
                var field = context.Fields.GetOrAddBindingField(bindingDesc);
                il.Ldarg(0);                            // [this]
                il.Dup();                               // [this, this]
                il.Ldstr(bindingDesc.Property.Name);    // [this, this, propName]
                il.Ldc_I4((int)bindingDesc.Flags);      // [this, this, propName, flags]
                var bindingCtor = field.FieldType.GetConstructors().Single();
                il.Newobj(bindingCtor);                 // [this, binding]
                il.Stfld(field);                        // []
                il.Ldloc(varList);                      // [list]
                il.Ldarg(0);                            // [list, this]
                il.Ldfld(field);                        // [list, binding]
                var addMethod = typeof(List<IAmBinding>).GetMethod(nameof(List<IAmBinding>.Add));
                il.Callvirt(addMethod); // []
            }
            il.Ldloc(varList);
            il.Ret();
        }
    }
}