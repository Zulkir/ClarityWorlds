using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3.Handlers
{
    public interface IShaderProgramFactory
    {
        IShaderProgram CreateDefault();
    }
}