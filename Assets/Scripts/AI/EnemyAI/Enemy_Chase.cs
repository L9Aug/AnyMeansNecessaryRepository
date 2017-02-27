using UnityEngine;
using System.Collections;

public class Enemy_Chase : MonoBehaviour {

    public float lostTimer;
    private GameObject Player;
    NavMeshAgent Agent;

    void Start () {
        Player = GameObject.Find("Player"); // for who you want the ai to chase
        Agent = GetComponent<NavMeshAgent>();
	}
	
    public void Chase()
    {
        if (!this.GetComponent<Cover>().movingToCover)
        {
            Agent.speed = 1;
            gameObject.transform.LookAt(Player.transform); //rotate and face player 
            Vector3 distToPlayer = transform.position - Player.transform.position;
            if (distToPlayer.magnitude < 10) // stops ai running straight into the players face.
            {
                Agent.speed = 0;
            }
            else
            {
                Agent.speed = 1;
                Agent.SetDestination(Player.transform.position); // pathfind to player with slight offset otherwise constantly pushes player
            }
        }
    }

    public void Sniperchase()
    {
        Vector3 distToWaypoint = transform.position - GetComponent<Enemy_Patrol>().SniperClosestWayPoint();
        Vector3 distToPlayer = transform.position - Player.transform.position;
        Agent.velocity = Vector3.zero;
        if (distToPlayer.magnitude > 4)
        {
            if (distToWaypoint.magnitude < 0.3f) // stops ai running straight into the players face.
            {
                Vector3 PlayerLookPos = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
                transform.LookAt(PlayerLookPos);
                Agent.speed = 0;
            }
            else
            {
                Agent.speed = 1;
                Agent.SetDestination(GetComponent<Enemy_Patrol>().SniperClosestWayPoint()); // pathfind to player with slight offset otherwise constantly pushes player
            }
        }else
        {
            Agent.velocity = Vector3.zero;
            Agent.speed = 0;
            Vector3 PlayerLookPos = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            transform.LookAt(PlayerLookPos);
        }
    }

    public void checkLost(bool found) // called from fieldofview script //basic lost function with a 3 second timer
    {
        if (found && gameObject.tag != "Hunter" || this.GetComponent<Base_Enemy>()._state == Base_Enemy.State.InCover || this.GetComponent<Cover>().movingToCover)
        {
            lostTimer = 0.0f;
            Enemy_Patrol.detected = true;
        }
        else if (this.GetComponent<Base_Enemy>()._state != Base_Enemy.State.InCover || !this.GetComponent<Cover>().movingToCover)
        {
            lostTimer += Time.deltaTime;
        }
        if (lostTimer >= 6)
        {
            if (gameObject.tag == "StandardEnemy")
            {
                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Patrol);
            }
            else if (gameObject.tag == "Sniper")
            {
                GetComponent<Sniper_Enemy>().setState(Sniper_Enemy.State.Patrol);
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {
            }
            else if (gameObject.tag == "Hunter")
            {
                GetComponent<Hunter_Enemy>().setState(Hunter_Enemy.State.Patrol);
            }
            Enemy_Patrol.detected = false; //alerts all ai that player has been lost
            lostTimer = 0.0f;
        }
    }
}
