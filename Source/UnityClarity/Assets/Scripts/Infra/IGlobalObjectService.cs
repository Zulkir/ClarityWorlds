using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Infra
{
    public interface IGlobalObjectService
    {
        GameObject EventObject { get; }
        GameObject VisualObjects { get; }
        GameObject UICarrier { get; }
        Text MessagePopupText { get; }
        // todo: think about moving to RenderService
        Camera RttCamera { get; }
        Camera MainCamera { get; set; }

        [CanBeNull]
        GameObject VrPlayerCarrier { get; set; }
        [CanBeNull]
        GameObject VrPlayer { get; set; }
        [CanBeNull]
        GameObject VrRightHand { get; set; }
        [CanBeNull]
        GameObject VrLeftHand { get; set; }
        [CanBeNull]
        GameObject VrGuiCanvas { get; set; }
    }
}