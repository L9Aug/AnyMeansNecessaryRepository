using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetectionMeter : MonoBehaviour {

    private Image meter;
    private Camera PlayerCam;
    public GameObject Enemy;
    
    float timer;
	// Use this for initialization
	void Start () {
        meter = gameObject.GetComponentInChildren<Image>();
        meter.type = Image.Type.Filled;
        PlayerCam = Camera.main;
        meter.fillAmount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + PlayerCam.transform.rotation * Vector3.forward, PlayerCam.transform.rotation * Vector3.up); // billboards detection meter to always be facing the camera
        DetetionTimer();
    }

    void DetetionTimer()
    {
        timer = Enemy.gameObject.GetComponent<FieldOfView>().detectedtimer;
        meter.fillAmount = timer / FieldOfView.detectionTimer;

        if (Enemy.gameObject.GetComponent<Base_Enemy>()._state == Base_Enemy.State.Dead || timer == 0)
        {
            meter.fillAmount = 0;
            Enemy.gameObject.GetComponent<FieldOfView>().detectedtimer = 0;
            Enemy.gameObject.GetComponent <Detectionspawn>().spawned = false;
            Destroy(gameObject);
        }
    }
}
