using Clarity.App.Worlds.Interaction.Tools;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Simulation.Fluids
{
    public class FluidSimulationExtension : IExtension
    {
        public string Name => "Simulation.Fluids";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IToolMenuItem>().To<FluidSimulationToolMenuItem>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}