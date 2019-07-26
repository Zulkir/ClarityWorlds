using Clarity.App.Worlds.Interaction.Tools;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class SpherePackingExtension : IExtension
    {
        public string Name => "Simulation.SpherePacking";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IToolMenuItem>().To<CirclePackingToolMenuItem>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}
