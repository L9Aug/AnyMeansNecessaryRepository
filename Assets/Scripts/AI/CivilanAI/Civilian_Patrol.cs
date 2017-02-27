using UnityEngine;
using System.Collections;

public class Civilian_Patrol : MonoBehaviour {
    NavMeshAgent Agent;
    //patrol variables//
    public Transform[] Waypoints; // use empty game objects in the inpector to set new waypoints
    private Vector3 MoveDirection;
    private Vector3 Target;
    private bool reverse;
    public int currentWaypoint;

    void Start () {
        Agent = GetComponent<NavMeshAgent>();
	}

    public void Patrol()
    {
        if (Enemy_Patrol.detected)
        {
            GetComponent<CivilianAI_Main>().setState(CivilianAI_Main.State.FLEE);
            return;
        }
        if (Waypoints.Length != 0){
            Agent.speed = 0.5f; // 0.5f = walking speed
            Target = Waypoints[currentWaypoint].position;
            MoveDirection = Target - transform.position;

            if (MoveDirection.magnitude < 2)
            {
                ///determing if at end of patrol route or start then sending it in reverse direction
                if (reverse == false)
                {
                    if (Waypoints.Length > 1)
                    {
                        currentWaypoint++;
                        if (currentWaypoint == Waypoints.Length - 1)
                            reverse = true;
                    }
                }
                else
                {
                    currentWaypoint--;
                    if (currentWaypoint == 0)
                        reverse = false;
                }
            }
            else
            {
                Agent.SetDestination(Waypoints[currentWaypoint].position);
            }
        }
        
    }
	
}
