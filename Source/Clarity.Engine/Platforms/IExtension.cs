using Clarity.Common.Infra.DependencyInjection;

namespace Clarity.Engine.Platforms
{
    public interface IExtension
    {
        string Name { get; }
        void Bind(IDiContainer di);
        void OnStartup(IDiContainer di);
    }
}