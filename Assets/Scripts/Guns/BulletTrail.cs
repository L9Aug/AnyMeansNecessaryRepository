using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour {

	public void SetupBulletTrial(Transform StartPos, Vector3 EndPos, float DisplayTime)
    {
        transform.position = StartPos.position;
        transform.rotation = StartPos.rotation;

        LineRenderer lineRend = GetComponent<LineRenderer>();

        lineRend.SetVertexCount(2);

        lineRend.SetPositions(new Vector3[] { StartPos.position, EndPos });

        StartCoroutine(Lifetime(DisplayTime));        
    }

    IEnumerator Lifetime(float lifeTime)
    {

        float TimeLeft = lifeTime;

        while(TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
