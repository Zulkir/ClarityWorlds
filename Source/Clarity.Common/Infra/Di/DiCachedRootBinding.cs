using System;

namespace Clarity.Common.Infra.Di
{
    public class DiCachedRootBinding : IDiCachedRootBinding
    {
        private readonly DiRootBindingType rootBindingType;
        private volatile object result;
        private IDiRootBinding binding;
        private readonly object buildResultLock = new object();

        public DiCachedRootBinding(IDiRootBinding binding, DiRootBindingType rootBindingType) 
        {
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            this.binding = binding;
            this.rootBindingType = rootBindingType;
        }

        public object GetResult(IDiContainer di)
        {
            if (result == null)
                lock (buildResultLock)
                    if (result == null)
                    {
                        result = BuildResult(di, binding, DiBuildInstanceType.Singleton);
                        binding = null;
                    }
            return result;
        }

        public object Instantiate(IDiContainer di)
        {
            var bindingLoc = binding;
            if (bindingLoc == null)
                throw new InvalidOperationException("Trying instantiate a class, that was acquired as a singleton.");
            return BuildResult(di, bindingLoc, DiBuildInstanceType.NewEachTime);
        }

        private object BuildResult(IDiContainer di, IDiRootBinding bindingLoc, DiBuildInstanceType buildInstanceType)
        {
            switch (rootBindingType)
            {
                case DiRootBindingType.Single:
                    return bindingLoc.BuildSingle(di, buildInstanceType);
                case DiRootBindingType.Multi:
                    return bindingLoc.BuildMulti(di, buildInstanceType);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDiRootBinding GetRootBinding(DiRootBindingType requestedRootBindingType)
        {
            if (requestedRootBindingType != rootBindingType)
                throw new InvalidOperationException($"Trying to use a '{rootBindingType}' binding as a '{requestedRootBindingType}' binding");
            var bindingLoc = binding;
            if (bindingLoc == null)
                throw new InvalidOperationException("Trying to bind after an instance had already been created.");
            return bindingLoc;
        }
    }
}