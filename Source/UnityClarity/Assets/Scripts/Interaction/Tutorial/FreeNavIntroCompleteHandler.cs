using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.Navigation;
using Clarity.Common.Numericals.Geometry;
using UnityEngine;
using Valve.VR;

namespace Assets.Scripts.Interaction.Tutorial
{
    public class FreeNavIntroCompleteHandler : MonoBehaviour
    {
        public IGlobalObjectService GlobalObjectService { get; set; }
        public Func<int> GetNextRoomId { get; set; }
        public INavigationService navigationService;

        private bool wasInside = false;

        public void Update()
        {
            var aaBox = new AaBox(transform.position.ToClarity(), new Size3(2, 2, 2));
            var isInside = aaBox.Contains(GlobalObjectService.MainCamera.transform.position.ToClarity());
            if (!wasInside && isInside)
            {
                Debug.Log("Cake");
                Teleport();
            }
            wasInside = isInside;
        }

        public void Teleport()
        {
            SteamVR_Fade.Start(Color.white, 0);
            navigationService.GoToSpecific(GetNextRoomId());
            SteamVR_Fade.Start(Color.clear, 0.5f);
        }
    }
}
