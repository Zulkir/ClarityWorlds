using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IShaderProgramFactory
    {
        IShaderProgram CreateDefault();
    }
}