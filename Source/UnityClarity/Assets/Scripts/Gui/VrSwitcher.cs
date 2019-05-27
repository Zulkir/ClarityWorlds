using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Gui
{
    public class VrSwitcher : MonoBehaviour
    {
        private bool vrActive;
        private Action nextFrame;

        private void Update()
        {
            if (nextFrame != null)
            {
                nextFrame();
                nextFrame = null;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                var deviceName = !vrActive 
                    ? XRSettings.supportedDevices.First(x => x.ToLower().Contains("open")) 
                    : "None";
                XRSettings.LoadDeviceByName(deviceName);
                vrActive = !vrActive;
                nextFrame = () => XRSettings.enabled = vrActive;
            }
        }
    }
}