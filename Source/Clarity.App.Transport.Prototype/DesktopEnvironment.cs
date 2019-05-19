using System.Collections.Generic;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype
{
    public class DesktopEnvironment : IEnvironment
    {
        public IReadOnlyList<IExtension> Extensions { get; private set; }

        public DesktopEnvironment(params IExtension[] extensions)
        {
            Extensions = extensions;
        }
    }
}