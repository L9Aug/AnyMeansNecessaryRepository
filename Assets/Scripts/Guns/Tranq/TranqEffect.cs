using UnityEngine;
using System.Collections;

public class TranqEffect : MonoBehaviour
{
    public static float TranqSpeed = 4;
    public static float BaseTranqSpeed = 4;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Tranquilising());
	}
	
    IEnumerator Tranquilising()
    {
        float timer = TranqSpeed;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        GetComponent<Base_Enemy>().setState(Base_Enemy.State.Stunned);
        Destroy(this);
    }

}
