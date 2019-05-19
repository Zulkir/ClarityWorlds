using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.Tools;
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