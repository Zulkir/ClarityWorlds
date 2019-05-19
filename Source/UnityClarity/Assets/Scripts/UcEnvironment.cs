using System.Collections.Generic;
using Clarity.Engine.Platforms;

namespace Assets.Scripts
{
    public class UcEnvironment : IEnvironment
    {
        public IReadOnlyList<IExtension> Extensions { get; }

        public UcEnvironment(params IExtension[] extensions)
        {
            Extensions = extensions;
        }
    }
}