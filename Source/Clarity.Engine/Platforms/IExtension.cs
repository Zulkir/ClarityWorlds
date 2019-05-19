using Clarity.Common.Infra.Di;

namespace Clarity.Engine.Platforms
{
    public interface IExtension
    {
        string Name { get; }
        void Bind(IDiContainer di);
        void OnStartup(IDiContainer di);
    }
}