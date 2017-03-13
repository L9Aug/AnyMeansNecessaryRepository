using UnityEngine;
using System.Collections;

public class AudioDetection : MonoBehaviour {

    public float detectionRadius;
    public LayerMask targetMask;
    public bool audioDetected;
    NavMeshAgent Agent;

    private Vector3 audioLoation;
    private Vector3 MoveDirection;
    void Start()
    {
        detectionRadius = 1;
        Agent = GetComponent<NavMeshAgent>();
    }

    public bool audioDetection()
    {
        Collider[] objectsInDetectionRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array

        if (audioDetected)
        {
            MoveDirection = audioLoation - this.transform.position;
            if (MoveDirection.magnitude < 2)
            {
                Agent.speed = 0.5f;
                audioDetected = false;
            }
            else
            {
                Agent.speed = 1;
                Agent.SetDestination(audioLoation);
                return true;
            }
        }

        if (objectsInDetectionRadius.Length == 0)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < objectsInDetectionRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
            {
                audioDetected = true;
                audioLoation = objectsInDetectionRadius[i].transform.position;
                Agent.SetDestination(audioLoation);
                Debug.DrawLine(transform.position, audioLoation, Color.cyan); //simple debug to see that the object has been seen within the scene editor
            }
            return true;
        }
    }
}
