using Clarity.App.Worlds.Interaction.Tools;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Platforms;
using Clarity.Ext.Simulation.SpherePacking.CirclePacking;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class SpherePackingExtension : IExtension
    {
        public string Name => "Simulation.SpherePacking";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IToolMenuItem>().To<CirclePackingToolMenuItem>();
            di.BindMulti<IToolMenuItem>().To<CirclePackingAutoToolMenuItem>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}
