using System;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources.Buffers;
using PtrMagic;

namespace Clarity.Ext.Rendering.Ogl3.Sugar
{
    public class UniformBufferSugar<T> : IDisposable where T : struct
    {
        private readonly IBuffer buffer;

        public UniformBufferSugar(IContext glContext)
        {
            buffer = glContext.Create.Buffer(BufferTarget.Uniform, PtrHelper.SizeOf<T>(), BufferUsageHint.DynamicDraw);
        }

        public void Bind(IContext glContext, int index)
        {
            glContext.Bindings.Buffers.UniformIndexed[index].Set(buffer);
        }

        public unsafe void SetData(T data)
        {
            var stackBuffer = stackalloc byte[buffer.SizeInBytes];
            PtrHelper.Write(stackBuffer, data);
            buffer.SetData((IntPtr)stackBuffer);
        }

        public void Dispose()
        {
            buffer.Dispose();
        }
    }
}