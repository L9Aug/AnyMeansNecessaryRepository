using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopInteraction : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            UIElements.ContextText.GetComponent<Text>().text = "Press 'F' to go to Shop";
            if (Input.GetButtonDown("Interact"))
            {
                PauseMenu.MenuOfPause.SetTransitionTarget(true, PauseMenu.PausedMachine.Shop);
            }
        }
    }

  
}
