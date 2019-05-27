using System;
using UnityEngine;

namespace Assets.Scripts.Interaction.Tutorial
{
    class ManipIntroCompleteTriggerHandler : MonoBehaviour
    {
        public bool Triggered { get; private set; }

        public Func<int> GetNextRoomId { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("ManipIntroCube"))
            {
                Debug.Log("Cake");
                Triggered = true;
            }
        }
    }
}
