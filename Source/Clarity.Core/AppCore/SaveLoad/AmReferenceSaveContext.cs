using System;
using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AmReferenceSaveContext : IDisposable
    {
        private readonly Dictionary<IAmObject, int> refIds = new Dictionary<IAmObject, int>();

        public bool SavingRef { get; set; }

        public int GetRefIdFor(IAmObject obj)
        {
            if (refIds.TryGetValue(obj, out var id))
                return id;
            id = refIds.Count;
            refIds.Add(obj, id);
            return id;
        }

        public void Dispose() { }
    }
}