using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UMA {

    public class AddPistolSlotScript : MonoBehaviour
    {

        public void OnDnaApplied(UMAData umaData)
        {
            List<Transform> children = new List<Transform>();
            children.AddRange(umaData.GetComponentsInChildren<Transform>());
            Transform RightHand = null;

            foreach (Transform t in children)
            {
                if (t.name == "hand_R")
                {
                    RightHand = t;
                    break;
                }
            }

            GameObject GunHolder = new GameObject();
            GunHolder.transform.position = Vector3.zero;
            GunHolder.transform.rotation = Quaternion.identity;
            GunHolder.transform.SetParent(RightHand, false);

            GameObject pistol = (GameObject)Instantiate(Resources.Load("Guns/" + umaData.GetComponent<AILoadout>().Weapon.ToString()), Vector3.zero, Quaternion.identity);

            pistol.transform.position = new Vector3(-0.0901f, -0.0428f, 0.03421f);
            pistol.transform.rotation = Quaternion.Euler(184.443f, 91.212f, -15.281f);
            pistol.transform.SetParent(GunHolder.transform, false);

            umaData.gameObject.GetComponent<FieldOfView>().CurrentWeapon = pistol.GetComponent<BaseGun>();
        }
    }
}
