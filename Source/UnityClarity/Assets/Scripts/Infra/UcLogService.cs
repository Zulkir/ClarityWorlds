using Clarity.App.Worlds.Logging;
using UnityEngine;

namespace Assets.Scripts.Infra
{
    public class UcLogService : ILogService
    {
        public void Write(LogMessageType messageType, string message)
        {
            switch (messageType)
            {
                case LogMessageType.Info:
                    Debug.Log(message);
                    break;
                case LogMessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogMessageType.HandledError:
                case LogMessageType.FatalError:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
