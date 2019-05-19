using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Clarity.Common.CodingUtilities.Sugar.Wrappers.Emit
{
    public class CilEmitterSugar
    {
        private readonly ILGenerator il;

        public CilEmitterSugar(ILGenerator il)
        {
            this.il = il;
        }

        #region Emit

        public void Add() => il.Emit(OpCodes.Add);
        public void And() => il.Emit(OpCodes.And);
        public void Bge(Label label) => il.Emit(OpCodes.Bge, label);
        public void Blt(Label label) => il.Emit(OpCodes.Blt, label);
        public void Bne_Un(Label label) => il.Emit(OpCodes.Bne_Un, label);
        public void Br(Label label) => il.Emit(OpCodes.Br, label);
        public void Brfalse(Label label) => il.Emit(OpCodes.Brfalse, label);
        public void Brtrue(Label label) => il.Emit(OpCodes.Brtrue, label);
        public void Call(ConstructorInfo constructor) => il.Emit(OpCodes.Call, constructor);
        public void Call(MethodInfo method) => il.Emit(OpCodes.Call, method);
        public void Callvirt(MethodInfo method) => il.Emit(OpCodes.Callvirt, method);
        public void Castclass(Type type) => il.Emit(OpCodes.Castclass, type);
        public void Ceq() => il.Emit(OpCodes.Ceq);
        public void Conv_I() => il.Emit(OpCodes.Conv_I);
        public void Conv_I4() => il.Emit(OpCodes.Conv_I4);
        public void Conv_I8() => il.Emit(OpCodes.Conv_I8);
        public void Conv_U() => il.Emit(OpCodes.Conv_U);
        public void Cpblk() => il.Emit(OpCodes.Cpblk);
        public void Dup() => il.Emit(OpCodes.Dup);
        public void Initobj(Type type) => il.Emit(OpCodes.Initobj, type);
        public void Isinst(Type type) => il.Emit(OpCodes.Isinst, type);

        public void Ldarg(int argIndex)
        {
            switch (argIndex)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); return;
                case 1: il.Emit(OpCodes.Ldarg_1); return;
                case 2: il.Emit(OpCodes.Ldarg_2); return;
                case 3: il.Emit(OpCodes.Ldarg_3); return;
            }
            il.Emit(IsSbyte(argIndex) ? OpCodes.Ldarg_S : OpCodes.Ldarg, argIndex);
        }

        public void Ldc_I4(int constant)
        {
            switch (constant)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); return;
                case 0: il.Emit(OpCodes.Ldc_I4_0); return;
                case 1: il.Emit(OpCodes.Ldc_I4_1); return;
                case 2: il.Emit(OpCodes.Ldc_I4_2); return;
                case 3: il.Emit(OpCodes.Ldc_I4_3); return;
                case 4: il.Emit(OpCodes.Ldc_I4_4); return;
                case 5: il.Emit(OpCodes.Ldc_I4_5); return;
                case 6: il.Emit(OpCodes.Ldc_I4_6); return;
                case 7: il.Emit(OpCodes.Ldc_I4_7); return;
                case 8: il.Emit(OpCodes.Ldc_I4_8); return;
            }

            il.Emit(IsSbyte(constant) ? OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, constant);
        }

        public void Ldc_I8(long constant) => il.Emit(OpCodes.Ldc_I8, constant);
        public void Ldelem(Type elemType) => il.Emit(OpCodes.Ldelem, elemType);
        public void Ldelem_I() => il.Emit(OpCodes.Ldelem_I);
        public void Ldelem_I1() => il.Emit(OpCodes.Ldelem_I1);
        public void Ldelem_I2() => il.Emit(OpCodes.Ldelem_I2);
        public void Ldelem_I4() => il.Emit(OpCodes.Ldelem_I4);
        public void Ldelem_I8() => il.Emit(OpCodes.Ldelem_I8);
        public void Ldelem_R4() => il.Emit(OpCodes.Ldelem_R4);
        public void Ldelem_R8() => il.Emit(OpCodes.Ldelem_R8);
        public void Ldelem_U1() => il.Emit(OpCodes.Ldelem_U1);
        public void Ldelem_U2() => il.Emit(OpCodes.Ldelem_U2);
        public void Ldelem_U4() => il.Emit(OpCodes.Ldelem_U4);
        public void Ldelem_Ref() => il.Emit(OpCodes.Ldelem_Ref);
        public void Ldelema(Type type) => il.Emit(OpCodes.Ldelema, type);
        public void Ldfld(FieldInfo field) => il.Emit(OpCodes.Ldfld, field);
        public void Ldftn(MethodInfo method) => il.Emit(OpCodes.Ldftn, method);
        public void Ldind_I() => il.Emit(OpCodes.Ldind_I);
        public void Ldind_I4() => il.Emit(OpCodes.Ldind_I4);
        public void Ldlen() => il.Emit(OpCodes.Ldlen);

        public void Ldloc(LocalBuilder local)
        {
            switch (local.LocalIndex)
            {
                case 0: il.Emit(OpCodes.Ldloc_0, local); return;
                case 1: il.Emit(OpCodes.Ldloc_1, local); return;
                case 2: il.Emit(OpCodes.Ldloc_2, local); return;
                case 3: il.Emit(OpCodes.Ldloc_3, local); return;
            }
            il.Emit(IsSbyte(local.LocalIndex) ? OpCodes.Ldloc_S : OpCodes.Ldloc, local);
        }

        public void Ldloca(LocalBuilder local) => il.Emit(IsSbyte(local.LocalIndex) ? OpCodes.Ldloca_S : OpCodes.Ldloca, local);
        public void Ldnull() => il.Emit(OpCodes.Ldnull);
        public void Ldobj(Type type) => il.Emit(OpCodes.Ldobj, type);
        public void Ldstr(string str) => il.Emit(OpCodes.Ldstr, str);
        public void Ldtoken(Type type) => il.Emit(OpCodes.Ldtoken, type);
        public void Mul() => il.Emit(OpCodes.Mul);
        public void Newarr(Type type) => il.Emit(OpCodes.Newarr, type);
        public void Newobj(ConstructorInfo constructor) => il.Emit(OpCodes.Newobj, constructor);
        public void Or() => il.Emit(OpCodes.Or);
        public void Pop() => il.Emit(OpCodes.Pop);
        public void Ret() => il.Emit(OpCodes.Ret);
        public void Shl() => il.Emit(OpCodes.Shl);
        public void Shr() => il.Emit(OpCodes.Shr);
        public void Sizeof(Type type) => il.Emit(OpCodes.Sizeof, type);
        public void Stelem_Ref() => il.Emit(OpCodes.Stelem_Ref);
        public void Stfld(FieldInfo field) => il.Emit(OpCodes.Stfld, field);
        public void Stind_I() => il.Emit(OpCodes.Stind_I);
        public void Stind_I4() => il.Emit(OpCodes.Stind_I4);
        public void Stind_Ref() => il.Emit(OpCodes.Stind_Ref);

        public void Stloc(LocalBuilder localVar)
        {
            switch (localVar.LocalIndex)
            {
                case 0: il.Emit(OpCodes.Stloc_0, localVar); return;
                case 1: il.Emit(OpCodes.Stloc_1, localVar); return;
                case 2: il.Emit(OpCodes.Stloc_2, localVar); return;
                case 3: il.Emit(OpCodes.Stloc_3, localVar); return;
            }
            il.Emit(IsSbyte(localVar.LocalIndex) ? OpCodes.Stloc_S : OpCodes.Stloc, localVar);
        }

        public void Stobj(Type type) => il.Emit(OpCodes.Stobj, type);
        public void Sub() => il.Emit(OpCodes.Sub);
        public void Switch(Label[] labelGroup) => il.Emit(OpCodes.Switch, labelGroup);
        public void Throw() => il.Emit(OpCodes.Throw);
        public void Unaligned(int alignment) => il.Emit(OpCodes.Unaligned, alignment);
        public void Unbox_Any(Type type) => il.Emit(OpCodes.Unbox_Any, type);

        #endregion

        #region Complex Emit
        private static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle", new[] { typeof(RuntimeTypeHandle) });

        public void LoadTypeObject(Type type)
        {
            Ldtoken(type);
            Call(GetTypeFromHandleMethod);
        }

        public LocalBuilder PinArray(Type elementType, Action<CilEmitterSugar> load)
        {
            var pointerVar = DeclareLocal(elementType.MakeByRefType(), true);

            load(this);
            Ldc_I4(0);
            Ldelema(elementType);
            Stloc(pointerVar);

            return pointerVar;
        }

        public LocalBuilder PinArray(Type elementType, LocalBuilder localVar)
        {
            return PinArray(elementType, lil => lil.Ldloc(localVar));
        }

        public LocalBuilder PinArray(Type elementType, int argIndex)
        {
            return PinArray(elementType, lil => lil.Ldarg(argIndex));
        }

        public void UnpinArray(LocalBuilder pointerVar)
        {
            Ldc_I4(0);
            Conv_U();
            Stloc(pointerVar);
        }
        #endregion

        #region Meta Actions

        public LocalBuilder DeclareLocal(Type localType, bool pinned = false)
        {
            return il.DeclareLocal(localType, pinned);
        }

        public Label DefineLabel()
        {
            return il.DefineLabel();
        }

        public void MarkLabel(Label label)
        {
            il.MarkLabel(label);
        }

        #endregion

        private static bool IsSbyte(int constant)
        {
            return sbyte.MinValue <= constant && constant <= sbyte.MaxValue;
        }
    }
}