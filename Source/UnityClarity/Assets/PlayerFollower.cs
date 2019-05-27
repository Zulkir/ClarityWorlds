using UnityEngine;

namespace Assets
{
    public class PlayerFollower : MonoBehaviour
    {

        public GameObject headTransform;
        // Start is called before the first frame update
        void Start()
        {
            headTransform = GameObject.Find("VRCamera");
        }

        // Update is called once per frame
        void Update()
        {
            var canvasPosition = headTransform.transform.position + headTransform.transform.up * (-0.2f) + headTransform.transform.forward * 0.4f;
            var canvasRotation = headTransform.transform.rotation;

            this.transform.SetPositionAndRotation(canvasPosition, canvasRotation);
        }
    }
}
