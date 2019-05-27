using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using Clarity.Ext.Rendering.Ogl3.Uniforms;
using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface ICommonObjects
    {
        IShaderProgram StandardShaderProgram { get; }
        UniformBufferSugar<TransformUniform> TransformUb { get; }
        UniformBufferSugar<Matrix4x4> CameraUb { get; }
        UniformBufferSugar<Vector3> CameraExtraUb { get; }
        UniformBufferSugar<Vector3> LightUb { get; }
        UniformBufferSugar<MaterialUniform> MaterialUb { get; }
        UniformBufferSugar<GlobalUniform> GlobalUb { get; }

        IStandardMaterial UndefinedMaterial { get; }
    }
}