using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Engine.EventRouting
{
    public class ServiceEventDependencyGraph : IServiceEventDependencyGraph
    {
        private readonly Dictionary<Type, List<Type>> directDependencies;

        public ServiceEventDependencyGraph()
        {
            directDependencies = new Dictionary<Type, List<Type>>();
        }

        public void AddDirectDependency(Type dependantServiceType, Type masterServiceType)
        {
            directDependencies.GetOrAdd(dependantServiceType, x => new List<Type>()).Add(masterServiceType);
            ValidateDependencies(dependantServiceType);
        }

        private void ValidateDependencies(Type startingType)
        {
            ValidateDependenciesRecursive(startingType, new Stack<Type>());
        }

        private void ValidateDependenciesRecursive(Type startingType, Stack<Type> visitedTypes)
        {
            const string separator = " -> ";
            if (visitedTypes.Contains(startingType))
                throw new InvalidOperationException($"Recursive dependency found: {string.Join(separator, visitedTypes.SkipWhile(x => x != startingType).ConcatSingle(startingType))}");

            visitedTypes.Push(startingType);
            foreach (var dependency in directDependencies.GetOrAdd(startingType, x => new List<Type>()))
                ValidateDependenciesRecursive(dependency, visitedTypes);
            visitedTypes.Pop();
        }

        public bool DependencyExists(Type dependantServiceType, Type masterServiceType)
        {
            return directDependencies.TryGetValue(dependantServiceType, out var list) &&
                   (list.Contains(masterServiceType) || list.Any(x => DependencyExists(x, masterServiceType)));
        }
    }
}