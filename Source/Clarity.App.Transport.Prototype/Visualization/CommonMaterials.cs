using System;
using Clarity.App.Transport.Prototype.SimLogs;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Transport.Prototype.Visualization
{
    public class CommonMaterials : ICommonMaterials
    {
        private readonly IMaterial siteMaterial;
        private readonly IMaterial createPackageMaterial;
        private readonly IMaterial readPackageMaterial;
        private readonly IMaterial updatePackageMaterial;
        private readonly IMaterial deletePackageMaterial;
        private readonly IMaterial migratePackageMaterial;

        public CommonMaterials()
        {
            siteMaterial = StandardMaterial.New().SetDiffuseColor(Color4.White).FromGlobalCache();
            createPackageMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Green).FromGlobalCache();
            readPackageMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Blue).FromGlobalCache();
            updatePackageMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Yellow).FromGlobalCache();
            deletePackageMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Red).FromGlobalCache();
            migratePackageMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Magenta).FromGlobalCache();
        }

        public IMaterial GetSiteMaterial()
        {
            return siteMaterial;
        }

        public IMaterial GetPackageMaterial(SimLogEntryCode code)
        {
            switch (code)
            {
                case SimLogEntryCode.Read: return readPackageMaterial;
                case SimLogEntryCode.Update: return updatePackageMaterial;
                case SimLogEntryCode.Create: return createPackageMaterial;
                case SimLogEntryCode.MigrationStart:
                case SimLogEntryCode.NewCopy:
                case SimLogEntryCode.RemoveCopy:
                case SimLogEntryCode.MigrationEnd:  return migratePackageMaterial;
                default: throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}