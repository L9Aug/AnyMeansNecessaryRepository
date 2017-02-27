using UnityEngine;
using System.Collections;

namespace UMA
{
    /// <summary>
    /// Auxillary slot which adds player movement script on character creation.
    /// </summary>
    public class PlayerControllerSlotScript : MonoBehaviour
    {       
        public float TakedownFOV; 
        public void OnDnaApplied(UMAData umaData)
        {
            var playerController = umaData.GetComponent<PlayerController>();
            if (playerController == null)
            {
                playerController = umaData.gameObject.AddComponent<PlayerController>();
                CapsuleCollider TriggerVolume = GetTriggerVolume(umaData);
                TriggerVolume.isTrigger = true;
                TriggerVolume.radius = 2;
                TriggerVolume.height = umaData.characterHeight;
                TriggerVolume.center = new Vector3(0, umaData.characterHeight * 0.5f - 0.04f, 0);
            }
            playerController.TakedownFOV = TakedownFOV;

            var healthComp = umaData.GetComponent<HealthComp>();
            if(healthComp == null)
            {
                healthComp = umaData.gameObject.AddComponent<HealthComp>();
            }
            healthComp.MaxHealth = 100;
            healthComp.healthChanged.Add(playerController.HealthCheck);
            UIElements UIE = FindObjectOfType<UIElements>();
            if (UIE != null)
            {
                healthComp.healthChanged.Add(UIE.UpdateHealth);
            }
        }

        CapsuleCollider GetTriggerVolume(UMAData umaData)
        {
            CapsuleCollider[] cols = umaData.gameObject.GetComponents<CapsuleCollider>();
            switch (cols.Length)
            {
                case 0:
                    return umaData.gameObject.AddComponent<CapsuleCollider>();

                default:
                    foreach(CapsuleCollider col in cols)
                    {
                        if (col.isTrigger)
                        {
                            return col;
                        }
                    }
                    return umaData.gameObject.AddComponent<CapsuleCollider>();
            }
        }
    }

}
