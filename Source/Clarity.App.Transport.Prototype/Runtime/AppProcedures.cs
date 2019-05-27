using System;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class AppProcedures : IAppProcedures
    {
        private readonly Lazy<IAppRuntime> appRuntimeLazy;

        private IAppRuntime AppRuntime => appRuntimeLazy.Value;

        public AppProcedures(Lazy<IAppRuntime> appRuntimeLazy)
        {
            this.appRuntimeLazy = appRuntimeLazy;
        }
    }
}