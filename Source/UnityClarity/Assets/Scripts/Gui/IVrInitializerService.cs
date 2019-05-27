using System;

namespace Assets.Scripts.Gui
{
    public interface IVrInitializerService
    {
        bool IsInitialized { get; }
        void Init();

        event Action Initialized;
    }
}