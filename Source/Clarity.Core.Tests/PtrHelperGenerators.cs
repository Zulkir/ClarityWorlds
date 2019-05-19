using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Clarity.Common.CodingUtilities.Sugar.Wrappers.Emit;
using NUnit.Framework;

namespace Clarity.Core.Tests
{
    public class PtrHelperGenerators
    {
        [Test]
        public void MakeSectionGreen()
        {
            Assert.That(true, Is.True);
        }

        [Test]
        [Ignore]
        public void GenerateNonSharpAssembly()
        {
            var appDomain = AppDomain.CurrentDomain;
            var assemblyBuilder = appDomain.DefineDynamicAssembly(new AssemblyName("PtrMagic"), AssemblyBuilderAccess.Save, "C:/Workshop/Projects/Clarity/Dependencies/PtrMagic");
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PtrMagic", "PtrMagic.dll");
            var typeBuilder = moduleBuilder.DefineType("PtrMagic.PtrHelper",
                TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Class | TypeAttributes.BeforeFieldInit,
                typeof(object), Type.EmptyTypes);

            DefineSizeOfGenericMethod(typeBuilder);
            DefineCopyBulkMethod(typeBuilder);
            DefineCopyBulkUniversalMethod(typeBuilder);
            DefineWriteSingleMethod(typeBuilder);
            DefineWriteArrayMethod(typeBuilder);
            DefineWriteArrayRangeMethod(typeBuilder);
            DefineReadMethod(typeBuilder);
            DefineReadArrayMethod(typeBuilder);
            DefineReadArrayRangeMethod(typeBuilder);
            typeBuilder.CreateType();

            assemblyBuilder.Save("PtrMagic.dll");
        }

        // int SizeOf<T>()
        private static void DefineSizeOfGenericMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("SizeOf",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(int));
            methodBuilder.SetParameters(Type.EmptyTypes);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            il.Sizeof(genericParam);
            il.Conv_I4();
            il.Ret();
        }

        // int SizeOf(Type type)
        //private static void DefineSizeOfMethod(TypeBuilder typeBuilder)
        //{
        //    var methodBuilder = typeBuilder.DefineMethod("SizeOf",
        //            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
        //    methodBuilder.SetReturnType(typeof(int));
        //    methodBuilder.SetParameters(typeof(Type));
        //    methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
        //    var il = new CilEmitterSugar(methodBuilder.GetILGenerator());
        //
        //    il.Ldarg(0);
        //    var sizeOfMethod = typeof(Marshal).GetMethod("SizeOf", new[] { typeof(Type) });
        //    il.Call(sizeOfMethod);
        //    il.Ret();
        //}

        // void CopyBulk(byte* dst, byte* src, int numBytes)
        private static void DefineCopyBulkMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("CopyBulk",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(byte*), typeof(byte*), typeof(int));
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.DefineParameter(3, 0, "numBytes");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());
            il.Ldarg(0);
            il.Ldarg(1);
            il.Ldarg(2);
            il.Cpblk();
            il.Ret();
        }

        // void CopyBulkUniversal(byte* dst, byte* src, int numBytes)
        private static void DefineCopyBulkUniversalMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("CopyBulkUniversal",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(byte*), typeof(byte*), typeof(int));
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.DefineParameter(3, 0, "numBytes");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());
            var toInt64Method = typeof(IntPtr).GetMethod("ToInt64");
            var getSizeMethod = typeof(IntPtr).GetMethod("get_Size");
            il.Ldarg(0);
            il.Call(toInt64Method);
            il.Ldarg(1);
            il.Call(toInt64Method);
            il.Or();                    // [(dst.ToInt64() | src.ToInt64())]

            il.Call(getSizeMethod);
            il.Ldc_I4(1);
            il.Sub();
            il.Conv_I8();
            il.And();                   // [((dst.ToInt64() | src.ToInt64()) & (IntPtr.Size - 1))]
            
            var cpblkLabel = il.DefineLabel();
            il.Brfalse(cpblkLabel);
            il.Unaligned(1);
            il.MarkLabel(cpblkLabel);
            il.Ldarg(0);
            il.Ldarg(1);
            il.Ldarg(2);
            il.Cpblk();
            il.Ret();
        }

        // void Write<T>(byte* dst, T val)
        private static void DefineWriteSingleMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Write",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(byte*), genericParam);
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "val");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            il.Ldarg(0);
            il.Ldarg(1);
            il.Stobj(genericParam);
            il.Ret();
        }

        // void Write<T>(byte* dst, T[] array)
        private static void DefineWriteArrayMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Write",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(byte*), genericParam.MakeArrayType());
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            var okLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();

            il.Ldarg(1);
            il.Brtrue(okLabel);

            il.Ldstr("src");
            il.Newobj(typeof(ArgumentNullException).GetConstructor(new[] { typeof(string) }));
            il.Throw();

            il.MarkLabel(okLabel);

            il.Ldarg(1);
            il.Ldlen();
            il.Conv_I4();
            il.Brfalse(endLabel);

            var pinnedVar = il.PinArray(genericParam, 1);
            il.Ldarg(0);
            il.Ldloc(pinnedVar);
            il.Ldarg(1);
            il.Ldlen();
            il.Conv_I4();
            il.Sizeof(genericParam);
            il.Mul();
            il.Cpblk();
            il.UnpinArray(pinnedVar);

            il.MarkLabel(endLabel);
            il.Ret();
        }

        // void Write<T>(byte* dst, T[] array, int startElem, int numElems)
        private static void DefineWriteArrayRangeMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Write",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(byte*), genericParam.MakeArrayType(), typeof(int), typeof(int));
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.DefineParameter(3, 0, "startElem");
            methodBuilder.DefineParameter(4, 0, "numElems");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            var throwLabel = il.DefineLabel();
            var okLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();

            il.Ldarg(1);
            il.Brfalse(throwLabel);

            il.Ldarg(2);
            il.Ldc_I4(0);
            il.Blt(throwLabel);

            il.Ldarg(1);
            il.Ldlen();
            il.Conv_I4();
            il.Ldarg(2);
            il.Ldarg(3);
            il.Add();
            il.Bge(okLabel);

            il.MarkLabel(throwLabel);
            il.Ldstr("Array is either null or the range is outside its bounds.");
            il.Newobj(typeof(ArgumentException).GetConstructor(new[] { typeof(string) }));
            il.Throw();

            il.MarkLabel(okLabel);

            il.Ldarg(3);
            il.Ldc_I4(1);
            il.Blt(endLabel);

            var pinnedVar = il.PinArray(genericParam, 1);
            il.Ldarg(0);
            il.Ldloc(pinnedVar);
            il.Ldarg(2);
            il.Sizeof(genericParam);
            il.Mul();
            il.Add();
            il.Ldarg(3);
            il.Sizeof(genericParam);
            il.Mul();
            il.Cpblk();
            il.UnpinArray(pinnedVar);

            il.MarkLabel(endLabel);
            il.Ret();
        }

        // T Read<T>(byte* src)
        private static void DefineReadMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Read",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(genericParam);
            methodBuilder.SetParameters(typeof(byte*));
            methodBuilder.DefineParameter(1, 0, "src");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            il.Ldarg(0);
            il.Ldobj(genericParam);
            il.Ret();
        }

        // void Read<T>(T[] dst, byte* src)
        private static void DefineReadArrayMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Read",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(genericParam.MakeArrayType(), typeof(byte*));
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            var okLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();

            il.Ldarg(0);
            il.Brtrue(okLabel);

            il.Ldstr("dst");
            il.Newobj(typeof(ArgumentNullException).GetConstructor(new[] { typeof(string) }));
            il.Throw();

            il.MarkLabel(okLabel);

            il.Ldarg(0);
            il.Ldlen();
            il.Brfalse(endLabel);

            var pinnedVar = il.PinArray(genericParam, 0);
            il.Ldloc(pinnedVar);
            il.Ldarg(1);
            il.Ldarg(0);
            il.Ldlen();
            il.Conv_I4();
            il.Sizeof(genericParam);
            il.Mul();
            il.Cpblk();
            il.UnpinArray(pinnedVar);

            il.MarkLabel(endLabel);
            il.Ret();
        }

        // void Read<T>(T[] dst, byte* src, int startElem, int numElems)
        private static void DefineReadArrayRangeMethod(TypeBuilder typeBuilder)
        {
            var methodBuilder = typeBuilder.DefineMethod("Read",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static);
            var genericTypeParams = methodBuilder.DefineGenericParameters("T");
            var genericParam = genericTypeParams.Single();
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(genericParam.MakeArrayType(), typeof(byte*), typeof(int), typeof(int));
            methodBuilder.DefineParameter(1, 0, "dst");
            methodBuilder.DefineParameter(2, 0, "src");
            methodBuilder.DefineParameter(3, 0, "startElem");
            methodBuilder.DefineParameter(4, 0, "numElems");
            methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);
            var il = new CilEmitterSugar(methodBuilder.GetILGenerator());

            var throwLabel = il.DefineLabel();
            var okLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();

            il.Ldarg(0);
            il.Brfalse(throwLabel);

            il.Ldarg(2);
            il.Ldc_I4(0);
            il.Blt(throwLabel);

            il.Ldarg(0);
            il.Ldlen();
            il.Ldarg(2);
            il.Ldarg(3);
            il.Add();
            il.Bge(okLabel);

            il.MarkLabel(throwLabel);
            il.Ldstr("Array is either null or the range is outside its bounds.");
            il.Newobj(typeof(ArgumentException).GetConstructor(new[] { typeof(string) }));
            il.Throw();

            il.MarkLabel(okLabel);

            il.Ldarg(3);
            il.Ldc_I4(1);
            il.Blt(endLabel);

            var pinnedVar = il.PinArray(genericParam, 0);
            il.Ldloc(pinnedVar);
            il.Ldarg(2);
            il.Sizeof(genericParam);
            il.Mul();
            il.Add();
            il.Ldarg(1);
            il.Ldarg(3);
            il.Sizeof(genericParam);
            il.Mul();
            il.Cpblk();
            il.UnpinArray(pinnedVar);

            il.MarkLabel(endLabel);
            il.Ret();
        }
    }
}