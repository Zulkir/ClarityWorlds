using System;
using Clarity.App.Transport.Prototype.TransportLogs;
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

        public IMaterial GetPackageMaterial(LogEntryCode code)
        {
            switch (code)
            {
                case LogEntryCode.Read: return readPackageMaterial;
                case LogEntryCode.Update: return updatePackageMaterial;
                case LogEntryCode.Create: return createPackageMaterial;
                case LogEntryCode.MigrationStart:
                case LogEntryCode.NewCopy:
                case LogEntryCode.RemoveCopy:
                case LogEntryCode.MigrationEnd:  return migratePackageMaterial;
                default: throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}