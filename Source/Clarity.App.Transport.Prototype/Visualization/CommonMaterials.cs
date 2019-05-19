using System;
using Clarity.App.Transport.Prototype.TransportLogs;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.App.Transport.Prototype.Visualization
{
    public class CommonMaterials : ICommonMaterials
    {
        private IMaterial siteMaterial;
        private IMaterial createPackageMaterial;
        private IMaterial readPackageMaterial;
        private IMaterial updatePackageMaterial;
        private IMaterial deletePackageMaterial;
        private IMaterial migratePackageMaterial;

        public CommonMaterials()
        {
            siteMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.White));
            createPackageMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Green));
            readPackageMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Blue));
            updatePackageMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Yellow));
            deletePackageMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Red));
            migratePackageMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Magenta));
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
            throw new System.NotImplementedException();
        }
    }
}