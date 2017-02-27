using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
/// </summary>


public class EnvironmentDetection : MonoBehaviour {

    public float detectionRadius;
    public LayerMask targetMask;
    NavMeshAgent Agent;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public bool Detection()
    {
        Collider[] objectsInDetectionRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array

        if(objectsInDetectionRadius.Length == 0)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < objectsInDetectionRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
            {
                Agent.SetDestination(objectsInDetectionRadius[i].transform.position);
                Debug.DrawLine(transform.position, objectsInDetectionRadius[i].transform.position, Color.yellow); //simple debug to see that the object has been seen within the scene editor
            }
                return true;
        }
    }
}

#if UNITY_EDITOR
#region Debug Editor
//allows visualization within the scene viewer
[CustomEditor(typeof(EnvironmentDetection))]
public class EnvironmentDetectionEditor : Editor
{
    void OnSceneGUI()
    {
        ///this is for debugging purposes it allos visualization within the scene window of the ai's detection radius
        EnvironmentDetection fow = (EnvironmentDetection)target;
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.detectionRadius);

    }
}

#endregion
#endif