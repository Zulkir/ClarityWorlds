using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class EventSender : MonoBehaviour, IEventSender
    {
        public event Action OnGUIEvent;
        public event Action UpdateEvent;
        public event Action LateUpdateEvent;
        public event Action FixedUpdateEvent;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            OnGUIEvent?.Invoke();
        }

        private void Update()
        {
            UpdateEvent?.Invoke();
        }

        // ReSharper disable once UnusedMember.Local
        private void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }
    }
}