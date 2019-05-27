﻿using Clarity.App.Transport.Prototype.TransportLogs;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Transport.Prototype.Visualization
{
    public interface ICommonMaterials
    {
        IMaterial GetSiteMaterial();
        IMaterial GetPackageMaterial(LogEntryCode code);
    }
}