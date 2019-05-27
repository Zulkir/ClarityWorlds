using System;
using Clarity.App.Worlds.Infra;
using Clarity.Engine.Platforms;
using Clarity.Ext.Format.Dicom;
using Clarity.Ext.Audio.Oal;
using Clarity.Ext.Gui.EtoForms;
using Clarity.Ext.Rendering.Ogl3;
using Clarity.Ext.Simulation.Fluids;
using Clarity.Ext.Format.Itd;
using Clarity.Ext.Import.Pptx;
using Clarity.Ext.StoryLayout.Building;
using Clarity.Ext.TextImlets.Latex;
using Clarity.Ext.Video.FFmpeg;

namespace Clarity
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += PrimitiveExceptionHandler.Handle;
            Run(args);
        }

        private static void Run(string[] args)
        {
            //GenOutput.FillDomain(GenDomain.Static);
            var extensions = new IExtension[]
            {
                new EtoGuiExtension(),
                new OglRenderingExtension(),
                new ItdFormatExtension(),
                new FFmpegVideoExtension(),
                new FluidSimulationExtension(),
                new DicomFormatExtension(),
                new BuildingStoryLayoutExtension(),
                new OalExtension(),
                new PptxImportExtension(),
                new LatexImletExtension(),
            };
            var environment = new DesktopEnvironment(extensions);
            var appLifecycle = new AppLifecycle();
            appLifecycle.StartAndRun(environment);
        }
    }
}