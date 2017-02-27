using UnityEngine;
using System.Collections;

public class toolTip : MonoBehaviour {

    public GameObject panel;

// Use this for initialization
	void Start () {
        panel.SetActive(false);
	}

   public void overButton()
    {
        panel.SetActive(true);
    }


   public void offButton()
    {
        panel.SetActive(false);
    }

	
	
	
}
