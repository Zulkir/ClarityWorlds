using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.Common.Numericals.Geometry;
using System;
using UnityEngine;

namespace Assets.Scripts.Interaction.Tutorial
{
    public class TutorialRoomStartTrigger : MonoBehaviour
    {
        public IGlobalObjectService GlobalObjectService { get; set; }
        public IVrInputDispatcher VrInputDispatcher { get; set; }
        public VrInputDispatcherCapabilities Capabilities { get; set; }
        public Type VrNavigationModeType { get; set; }

        private bool wasInside = false;

        public void Update()
        {
            var aaBox = new AaBox(transform.position.ToClarity(), new Size3(2, 2, 2));
            var isInside = aaBox.Contains(GlobalObjectService.MainCamera.transform.position.ToClarity());
            if (!wasInside && isInside)
            {
                Debug.Log("Cake");
                Invoke(nameof(ChangeMode), 2);
            }
            wasInside = isInside;
        }

        private void ChangeMode()
        {
            VrInputDispatcher.SetCapabilities(Capabilities);
            VrInputDispatcher.SetVrNavigationMode(VrNavigationModeType);
        }
    }
}
