using System;

namespace Clarity.Engine.EventRouting
{
    public interface IServiceEventDependencyGraph
    {
        void AddDirectDependency(Type dependantServiceType, Type masterServiceType);
        bool DependencyExists(Type dependantServiceType, Type masterServiceType);
    }
}