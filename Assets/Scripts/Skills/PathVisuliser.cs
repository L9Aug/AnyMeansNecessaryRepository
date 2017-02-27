using UnityEngine;
using System.Collections;

public class PathVisuliser : MonoBehaviour {

    public NavMeshAgent nma;
    public LineRenderer lr;
    public Material LineRendMat;
    public Vector2 LineParams;
    public float PathHeight;
    public float LineUpdateCheckDelay;

    NavMeshPath navMeshPath;

	// Use this for initialization
	void Start () {
        //nma = GetComponent<NavMeshAgent>();
        //lr = GetComponent<LineRenderer>();
        navMeshPath = nma.path;
        lr.material = LineRendMat;
        lr.SetWidth(LineParams.x, LineParams.y);
        StartCoroutine(CheckPathStatus());
	}

    IEnumerator CheckPathStatus()
    {
        while (true)
        {
            if (nma.path != navMeshPath)
            {
                navMeshPath = nma.path;
                lr.SetVertexCount(navMeshPath.corners.Length);
                for (int i = 0; i < navMeshPath.corners.Length; ++i)
                {
                    lr.SetPosition(i, navMeshPath.corners[i] + new Vector3(0, PathHeight, 0));
                }
            }
            yield return new WaitForSeconds(LineUpdateCheckDelay);
        }
    }
    
}
