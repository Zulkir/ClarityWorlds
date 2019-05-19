using System;

namespace Clarity.Engine.Resources
{
    public interface IFactoryResourceSource : IResourceSource, IEquatable<IFactoryResourceSource>
    {
        IResourceFactory Factory { get; }
    }
}