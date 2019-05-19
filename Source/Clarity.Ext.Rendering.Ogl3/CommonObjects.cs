using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Visualization.Graphics.Materials;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Samplers;
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
        public ISampler DefaultSampler { get; }
        public IStandardMaterial UndefinedMaterial { get; }
        public ITexture2D Texture2DForUndefinedSource { get; }

        public CommonObjects(IContext glContext, IShaderProgramFactory shaderProgramFactory)
        {
            StandardShaderProgram = shaderProgramFactory.CreateDefault();
            TransformUb = new UniformBufferSugar<TransformUniform>(glContext);
            CameraUb = new UniformBufferSugar<Matrix4x4>(glContext);
            CameraExtraUb = new UniformBufferSugar<Vector3>(glContext);
            LightUb = new UniformBufferSugar<Vector3>(glContext);
            MaterialUb = new UniformBufferSugar<MaterialUniform>(glContext);
            GlobalUb = new UniformBufferSugar<GlobalUniform>(glContext);
            DefaultSampler = glContext.Create.Sampler();
            DefaultSampler.SetMagFilter(TextureMagFilter.Linear);
            DefaultSampler.SetMinFilter(TextureMinFilter.LinearMipmapLinear);
            DefaultSampler.SetWrapR(TextureWrapMode.MirroredRepeat);
            DefaultSampler.SetWrapS(TextureWrapMode.MirroredRepeat);
            DefaultSampler.SetWrapT(TextureWrapMode.MirroredRepeat);
            DefaultSampler.SetMaxAnisotropy(16f);

            UndefinedMaterial = CreateUndefinedMaterial();
            Texture2DForUndefinedSource = CreateTextureForUndefinedSource();
        }

        private static IStandardMaterial CreateUndefinedMaterial()
        {
            return new StandardMaterial(new SingleColorPixelSource(Color4.Red))
            {
                NoSpecular = true
            };
        }

        private static ITexture2D CreateTextureForUndefinedSource()
        {
            return null;
        }
    }
}