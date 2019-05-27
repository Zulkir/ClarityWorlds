using Clarity.App.Transport.Prototype.SimLogs;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Transport.Prototype.Visualization
{
    public interface ICommonMaterials
    {
        IMaterial GetSiteMaterial();
        IMaterial GetPackageMaterial(SimLogEntryCode code);
    }
}