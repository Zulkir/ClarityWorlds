using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Rendering.Ogl3.Uniforms
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TransformUniform
    {
        public Matrix4x4 World;
        public Matrix4x4 WorldInverseTranspose;
        public float ZOffset;
    }
}