using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Ext.Rendering.Ogl3.Handlers;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using Clarity.Ext.Rendering.Ogl3.Uniforms;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class CommonObjects : ICommonObjects
    {
        public IShaderProgram StandardShaderProgram { get; }
        public UniformBufferSugar<TransformUniform> TransformUb { get; }
        public UniformBufferSugar<Matrix4x4> CameraUb { get; }
        public UniformBufferSugar<Vector3> CameraExtraUb { get; }
        public UniformBufferSugar<Vector3> LightUb { get; }
        public UniformBufferSugar<MaterialUniform> MaterialUb { get; }
        public UniformBufferSugar<GlobalUniform> GlobalUb { get; }
        public IStandardMaterial UndefinedMaterial { get; }

        public CommonObjects(IContext glContext, IShaderProgramFactory shaderProgramFactory)
        {
            StandardShaderProgram = shaderProgramFactory.CreateDefault();
            TransformUb = new UniformBufferSugar<TransformUniform>(glContext);
            CameraUb = new UniformBufferSugar<Matrix4x4>(glContext);
            CameraExtraUb = new UniformBufferSugar<Vector3>(glContext);
            LightUb = new UniformBufferSugar<Vector3>(glContext);
            MaterialUb = new UniformBufferSugar<MaterialUniform>(glContext);
            GlobalUb = new UniformBufferSugar<GlobalUniform>(glContext);

            UndefinedMaterial = CreateUndefinedMaterial();
        }

        private static IStandardMaterial CreateUndefinedMaterial()
        {
            return StandardMaterial.New()
                .SetDiffuseColor(Color4.Red)
                .SetNoSpecular(true)
                .FromGlobalCache();
        }
    }
}