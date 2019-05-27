using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Infra
{
    public class GlobalObjectService : IGlobalObjectService
    {
        public GameObject EventObject { get; }
        public GameObject VisualObjects { get; }
        public Text MessagePopupText { get; }
        public Camera RttCamera { get; }
        public Camera MainCamera { get; set; }
        public GameObject VrPlayerCarrier { get; set; }
        public GameObject VrPlayer { get; set; }
        public GameObject VrRightHand { get; set; }
        public GameObject VrLeftHand { get; set; }
        public GameObject VrGuiCanvas { get; set; }

        public GameObject UICarrier { get; }

        public GlobalObjectService()
        {
            // todo: Maybe create at runtime
            EventObject = GameObject.Find("EventObject");
            VisualObjects = GameObject.Find("VisualObjects");
            UICarrier = GameObject.Find("UICarrier");
            MessagePopupText = GameObject.Find("MessagePopupsText").GetComponent<Text>();

            RttCamera = new GameObject("RttCamera").AddComponent<Camera>();
            RttCamera.enabled = false;
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
}
