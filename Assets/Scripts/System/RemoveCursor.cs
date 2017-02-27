using UnityEngine;
using System.Collections;

/// <summary>
/// Will remove the cursor from view and lock it in place if the game is not running in engine.
/// </summary>
public class RemoveCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    // Update is called once per frame
    void Update ()
    {

    }


}
