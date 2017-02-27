using UnityEngine;
using System.Collections;

public class Detectionspawn : MonoBehaviour {

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>

    public bool spawned;
    float timer;
    public GameObject EnemyCanvas;
    void Start()
    {
        EnemyCanvas = (GameObject)Resources.Load("EnemyCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        DetetionTimer();
    }

    void DetetionTimer()
    {
        timer = GetComponent<FieldOfView>().detectedtimer;
        if (timer != 0 && !spawned)
        {
            spawned = true;
            GameObject canvas = (GameObject)Instantiate(EnemyCanvas, Vector3.zero, Quaternion.identity);
            canvas.GetComponent<DetectionMeter>().Enemy = this.gameObject;
        }
    }
}
