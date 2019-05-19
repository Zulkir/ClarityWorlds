using System;
using Clarity.Engine.Platforms;
using Clarity.Ext.Rendering.Ogl3;

namespace Clarity.App.Transport.Prototype
{
    static class Program
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
                new OglRenderingExtension(),
            };
            var appLifecycle = new AppLifecycle();
            var environment = new DesktopEnvironment(extensions);
            appLifecycle.StartAndRun(environment);
        }
    }
}
