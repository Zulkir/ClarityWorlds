using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class EventSender : MonoBehaviour, IEventSender
    {
        public event Action OnGUIEvent;
        public event Action UpdateEvent;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            OnGUIEvent?.Invoke();
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            UpdateEvent?.Invoke();
        }
    }
}