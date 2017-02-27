using UnityEngine;
using System.Collections;


namespace UMA
{
    public class CameraControllerSlotScript : MonoBehaviour
    {
        public Vector3 FirstPersonPosition;
        public Vector3 ThirdPersonAnchorPosition;
        public float CameraDistance;

        public void OnDnaApplied(UMAData umaData)
        {
            var camCont = umaData.GetComponent<CameraController>();
            if (camCont == null)
            {
                camCont = umaData.gameObject.AddComponent<CameraController>();
            }
            camCont.FirstPersonPosition = FirstPersonPosition;
            camCont.ThirdPersonAnchor = ThirdPersonAnchorPosition;
            camCont.ThirdPersonCameraDistance = CameraDistance;
        }

    }
}