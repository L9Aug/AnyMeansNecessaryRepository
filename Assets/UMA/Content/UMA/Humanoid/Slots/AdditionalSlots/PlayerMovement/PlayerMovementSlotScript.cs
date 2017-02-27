using UnityEngine;
using System.Collections;


namespace UMA
{
    /// <summary>
    /// Auxillary slot which adds player movement script on character creation.
    /// </summary>
    public class PlayerMovementSlotScript : MonoBehaviour
    {
        public void OnDnaApplied(UMAData umaData)
        {
            /*var thirdPersonMovement = umaData.GetComponent<ThirdPersonMovement>();
            if (thirdPersonMovement == null)
            {
                thirdPersonMovement = umaData.gameObject.AddComponent<ThirdPersonMovement>();
            }*/

            var firstPersonMovement = umaData.GetComponent<FirstPersonMovement>();
            if(firstPersonMovement == null)
            {
                firstPersonMovement = umaData.gameObject.AddComponent<FirstPersonMovement>();
            }
            //firstPersonMovement.enabled = false;

            var movementController = umaData.GetComponent<PlayerMovementController>();
            if(movementController == null)
            {
                movementController = umaData.gameObject.AddComponent<PlayerMovementController>();
            }
            //movementController.m_TPM = thirdPersonMovement;
            movementController.m_FPM = firstPersonMovement;

        }
    }

}
