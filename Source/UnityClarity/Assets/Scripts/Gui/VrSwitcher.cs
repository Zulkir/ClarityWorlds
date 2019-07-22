using System;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Gui
{
    public class VrSwitcher : MonoBehaviour
    {
        private bool vrActive;
        private Action nextFrame;

        public event Action VrInitialized;

        private void Start()
        {
            if (XRDevice.isPresent)
            {
                vrActive = true;
                VrInitialized?.Invoke();
                nextFrame = () => { XRSettings.enabled = vrActive; };
            }
        }

        private void Update()
        {
            if (nextFrame != null)
            {
                nextFrame();
                nextFrame = null;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                vrActive = true;
                VrInitialized?.Invoke();
                nextFrame = () => { XRSettings.enabled = vrActive; };
            }
        }
    }
}