using UnityEngine;
using System.Collections;

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>

public class CivilianAI_Main : MonoBehaviour {
    public static bool Scared = false;
    public State _state;
    private NavMeshAgent Agent;
    public Transform[] hidePoints;
    public int randomHidePoint;
    public float fleeTimer;

    // Use this for initialization
    void Start () {

        Agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthComp>().healthChanged.Add(HealthTest);

        hidePoints[0] = GameObject.Find("HideLocations 1").transform;
        hidePoints[1] = GameObject.Find("HideLocations 2").transform;
        hidePoints[2] = GameObject.Find("HideLocations 3").transform;
        hidePoints[3] = GameObject.Find("HideLocations 4").transform;
    }

    void HealthTest(float health, float ChangeInHealth, float armour)
    {
        setState(State.FLEE);
        if (health <= 0)
        {
            setState(State.DEAD);
            GetComponent<Animator>().SetTrigger("Takedown");
        }
    }
    // Update is called once per frame
    void Update()
    {
        FSM();
    }
    void FSM()
    {
        switch (_state)
        {
            case State.WANDER:
                GetComponent<Civilian_Patrol>().Patrol();
                break;
            case State.FLEE:
                Flee();
                break;
            case State.DEAD:
                Agent.Stop();
                break;
        }
    }

    public enum State // basic FSM some states aren't in use.
    {
        WANDER,       //patrol for player
        FLEE,        //chase player
        DEAD,       //Attack player if siutation correct
    }
    public void setState(State newState)
    {
        _state = newState; // assigns new state based on value inputted.
    }
    private void Flee()
    {
        if (!Scared)
        {
            randomHidePoint = Random.Range(0, 3);
            Scared = true;
        }
        else if (Scared)
        {
            if (!Enemy_Patrol.detected)
            {
                if(fleeTimer >= 30)
                {
                    fleeTimer = 0;
                    setState(State.WANDER);
                    return;
                }
                fleeTimer += Time.deltaTime;
            }
            Agent.speed = 1;
         //   Agent.SetDestination(hidePoints[randomHidePoint].position);//picks 1 of the 4 corners from empty gameobjects and runs towards that specific point
        }
    }

}
