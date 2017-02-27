using UnityEngine;
using System.Collections;

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>
    /// 

public class Enemy_Patrol : MonoBehaviour {
    public static bool detected = false; // used for informing all enemy ai's if player is detected
    NavMeshAgent Agent;
    GameObject Player;
	// Use this for initialization

    public Transform[] Waypoints; // use empty game objects in the inpector to set new waypoints
    private int currentWaypoint;

    private Vector3 MoveDirection;
    private Vector3 Target;
    private bool reverse;
    private Vector3 enemyStartPos;

    void Start()
    {
        Player = GameObject.Find("Player");
        Agent = GetComponent<NavMeshAgent>();
        enemyStartPos = transform.position; // removes the need for setting up an intital waypoint for stationary guards with no patrol route
        Target = enemyStartPos;
    }

    public void Patrol()
    {
        if (detected == true)// chases player if any ai detected the player excluding lone wolf.
        {
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Chase);
            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Sniper_Enemy.State.Chase);
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {
            }
            else if (gameObject.tag == "Hunter")
            {
            }
        }

            Agent.speed = 0.5f; // 0.5f = walking speed
        if (Waypoints.Length != 0) // if enemy has a patrol route then follow that otherwise after chasing player return to its spawn location
        {
            Target = Waypoints[currentWaypoint].position;
        }else
        {
            Target = enemyStartPos;
        }
            MoveDirection = Target - Agent.transform.position;

            if (!GetComponent<EnvironmentDetection>().Detection())
            {
                if (MoveDirection.magnitude < 1)
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
                    Agent.SetDestination(Target);
                }
        }
    }

    private Vector3 distToTarget;
    private Vector3 closestTarget;
    private Vector3 currentTarget;

    public Vector3 SniperClosestWayPoint() // returns the ai's closest waypoint to the player
    {
        distToTarget = Vector3.zero;
        closestTarget = Vector3.zero;
        for (int i = 0; i < Waypoints.Length; i++)
        {
            //determining closest waypoint point to player and sends sniper there to watch for the player
            distToTarget = Waypoints[i].transform.position - Player.transform.position;
            if (closestTarget == Vector3.zero)
            {
                closestTarget = Waypoints[i].transform.position - Player.transform.position;
                currentTarget = Waypoints[i].transform.position;
            }
            else if (closestTarget.magnitude > distToTarget.magnitude)
            {
                closestTarget = distToTarget;
                currentTarget = Waypoints[i].transform.position;
            }
        }

        if (currentTarget != Vector3.zero)
        {
            return currentTarget; // will always return here unless no waypoint was setup then it'll just return its initital starting point.
        }
        return Target;
    }
}
