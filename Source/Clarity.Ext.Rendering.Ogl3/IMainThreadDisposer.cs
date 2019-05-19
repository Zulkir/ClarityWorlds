using System;
using JetBrains.Annotations;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IMainThreadDisposer
    {
        bool FinishedWorking { get; set; }
        void Add([CanBeNull] IDisposable obj);
        void DisposeOfAll();
    }
}