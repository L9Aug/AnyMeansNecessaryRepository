using UnityEngine;
using System.Collections;

namespace UMA
{
	/// <summary>
	/// Auxillary slot which adds a CapsuleCollider and Rigidbody to a newly created character.
	/// </summary>
	public class CapsuleColliderSlotScript : MonoBehaviour 
	{
        public PhysicMaterial PM;

		public void OnDnaApplied(UMAData umaData)
		{
			var umaDna = umaData.GetDna<UMADnaHumanoid>();
			if (umaDna == null)
			{
				Debug.LogError("Failed to add Capsule Collider to: " + umaData.name);
				return;
			}

			var rigid = umaData.gameObject.GetComponent<Rigidbody>();
			if (rigid == null)
			{
				rigid = umaData.gameObject.AddComponent<Rigidbody>();
			}
			rigid.constraints = RigidbodyConstraints.FreezeRotation;
            rigid.mass = 10; // umaData.characterMass;
            rigid.interpolation = RigidbodyInterpolation.Interpolate;
            rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            CapsuleCollider[] capsules = umaData.GetComponents<CapsuleCollider>();
            CapsuleCollider capsule = umaData.gameObject.GetComponent<CapsuleCollider>();
            switch (capsules.Length)
            {
                case 0:
                    capsule = umaData.gameObject.AddComponent<CapsuleCollider>();
                    break;

                case 1:
                    if (!capsules[0].isTrigger)
                    {
                        capsule = capsules[0];
                    }
                    else
                    {
                        capsule = umaData.gameObject.AddComponent<CapsuleCollider>();
                    }
                    break;

                default:
                    foreach(CapsuleCollider col in capsules)
                    {
                        if (!col.isTrigger)
                        {
                            capsule = col;
                            break;
                        }
                    }                    
                    break;
            }

			capsule.radius = umaData.characterRadius;
			capsule.height = umaData.characterHeight;
			capsule.center = new Vector3(0, capsule.height * 0.5f, 0);
            capsule.material = PM;
        }
	}
}