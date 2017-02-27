using UnityEngine;
using System.Collections;

public class MiniMapLookAt : MonoBehaviour
{

    public Transform player;
    public RenderTexture MiniMapTexture;
    public Material MiniMapMaterial;

    private int FirstOffset;
    private int SecondOffset;

    // Use this for initialization
    void Start()
    {
        FirstOffset = 256;
        SecondOffset = 10;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(player.position.x, 50, player.position.z);
       
        Graphics.DrawTexture(new Rect(Screen.width - FirstOffset - SecondOffset, Screen.height + FirstOffset + SecondOffset, 256, 256), MiniMapTexture, MiniMapMaterial);

    }

  

}
