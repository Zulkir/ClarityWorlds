using System.Collections.Generic;

namespace Clarity.Engine.Platforms
{
    public interface IEnvironment
    {
        IReadOnlyList<IExtension> Extensions { get; }
    }
}