using System;

namespace Clarity.Core.AppCore.AppModes
{
    public interface IAppModeService
    {
        AppMode Mode { get; }
        void SetMode(AppMode mode);
        event Action ModeChanged;
    }
}