using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PathParameters
{
    public float PathHeight;
    public Material PathMat;
    public float StartWidth, EndWidth;
}

public class DisplayAIPaths : MonoBehaviour
{
    public static DisplayAIPaths DAP;
    public PathParameters LivePathParameters, StaticPathParameters;
    public float LivePathUpdateDelay;

    List<NavMeshAgent> NavMeshAgents = new List<NavMeshAgent>();
    //List<Enemy_Patrol> PatrolAI = new List<Enemy_Patrol>();

    List<GameObject> LivePathVisualisers = new List<GameObject>();
    List<GameObject> StaticPathVisualisers = new List<GameObject>();

    public bool LivePath = true;
    public bool StaticPath = true;

    void Awake()
    {
        DAP = this;
        NavMeshAgents.AddRange(FindObjectsOfType<NavMeshAgent>());
        //PatrolAI.AddRange(FindObjectsOfType<Enemy_Patrol>());
        //DisplayPaths();
    }

    void DisplayLivePaths()
    {
        foreach(NavMeshAgent agent in NavMeshAgents)
        {
            GameObject PathHolder = new GameObject();
            PathHolder.layer = gameObject.layer;
            PathHolder.transform.SetParent(transform);
            PathHolder.name = agent.gameObject.name + " Live Path Visuliser";
            LivePathVisualisers.Add(PathHolder);

            PathVisuliser nVisuliser = PathHolder.AddComponent<PathVisuliser>();
            nVisuliser.lr = PathHolder.AddComponent<LineRenderer>();
            nVisuliser.nma = agent;
            nVisuliser.LineRendMat = LivePathParameters.PathMat;
            nVisuliser.LineParams = new Vector2(LivePathParameters.StartWidth, LivePathParameters.EndWidth);
            nVisuliser.PathHeight = LivePathParameters.PathHeight;
            nVisuliser.LineUpdateCheckDelay = LivePathUpdateDelay;
        }
    }

    void ClearLivePaths()
    {
        foreach (GameObject go in LivePathVisualisers)
        {
            Destroy(go);
        }
    }

    void DisplayStaticPaths()
    {
        GameObject MyPathingObject = new GameObject();
        NavMeshAgent MyPathingAgent = MyPathingObject.AddComponent<NavMeshAgent>();

        foreach(NavMeshAgent agent in NavMeshAgents)
        {
            Enemy_Patrol tempPat = agent.GetComponent<Enemy_Patrol>();
            if(tempPat != null)
            {
                if (tempPat.Waypoints.Length > 1)
                {
                    for (int i = 1; i < tempPat.Waypoints.Length; ++i)
                    {
                        GameObject PathHolder = new GameObject();
                        PathHolder.layer = gameObject.layer;
                        PathHolder.transform.SetParent(transform);
                        PathHolder.name = agent.gameObject.name + " Static Path Visuliser";
                        StaticPathVisualisers.Add(PathHolder);

                        LineRenderer lr = PathHolder.AddComponent<LineRenderer>();
                        lr.SetWidth(StaticPathParameters.StartWidth, StaticPathParameters.EndWidth);
                        lr.material = StaticPathParameters.PathMat;

                        MyPathingAgent.radius = agent.radius;
                        MyPathingAgent.height = agent.height;
                        

                        NavMeshPath tempPath = new NavMeshPath();

                        //MyPathingAgent.transform.position = tempPat.Waypoints[i - 1].position;
                        MyPathingAgent.Warp(tempPat.Waypoints[i - 1].position);
                        
                        MyPathingAgent.CalculatePath(tempPat.Waypoints[i].position, tempPath);

                        if (tempPath.corners.Length > 0)
                        {
                            lr.SetVertexCount(tempPath.corners.Length);
                            for (int j = 0; j < tempPath.corners.Length; ++j)
                            {
                                lr.SetPosition(j, tempPath.corners[j] + new Vector3(0, StaticPathParameters.PathHeight, 0));
                            }
                        }
                    }
                }
            }
        }

        Destroy(MyPathingObject);
    }

    void ClearStaticPaths()
    {
        foreach (GameObject go in StaticPathVisualisers)
        {
            Destroy(go);
        }
    }

    public void DisplayPaths()
    {
        ClearStaticPaths();
        if (StaticPath)
        {
            DisplayStaticPaths();
        }

        ClearLivePaths();
        if (LivePath)
        {
            DisplayLivePaths();
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(DisplayAIPaths))]
public class DisplayAIPathsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Diplay Paths"))
        {
            ((DisplayAIPaths)target).DisplayPaths();
        }

    }

}
#endif