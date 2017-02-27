using UnityEngine;
using System.Collections;

public class Base_Enemy : MonoBehaviour 
{
    private GameObject canvas;
    public static int StandardKills;
    public static int armoredKills;
    public static int sniperKills;
    public static int hunterKills;

	public State _state;
    public NavMeshAgent Agent;
    public static int killCount;
    public static float stunTimer;

    private bool Stunned;
    private float currentStunTimer;


    void Start()
    {
        canvas = GameObject.Find("mainCanvas");
        _state = State.Patrol;
        Agent = GetComponent<NavMeshAgent>();
    }

	public void setState(State newState)
	{
		_state = newState; // assigns new state based on value inputted.
	}

	public enum State // basic FSM some states aren't in use.
	{
		Patrol,       //patrol for player
		Chase,        //chase player
		Attack,       //Attack player if siutation correct
        wasShot,      //if ai was shot      
		Dead,         //dead
		Alerted,      //used when finding bodies
		InCover,
        Stunned,
	}

    public virtual void Killai()
    {
        if (_state != State.Dead)
        {
            Agent.velocity = Vector3.zero; // stops ai from sliding to last set destination
            killCount++;
            canvas.GetComponent<UIElements>().xpGain(15);

            setState(State.Dead);
            if (gameObject.tag == "StandardEnemy")
            {
                StandardKills++;
            }
            else if (gameObject.tag == "Sniper")
            {
                sniperKills++;
            }
            else if (gameObject.tag == "ArmoredEnemy")
            {
                armoredKills++;
            }
            else if (gameObject.tag == "Hunter")
            {
                hunterKills++;
            }

            GetComponent<Animator>().SetTrigger("Takedown");
        }
    }
    public virtual void Stunai()
    {
        if (Stunned)
        {
            if (currentStunTimer <= stunTimer)
            {

                if (GetComponent<HealthComp>().GetHealth() <= 0)
                {
                    currentStunTimer = 0;
                    Stunned = false;
                    setState(State.Dead);
                }
                currentStunTimer += Time.deltaTime;
            }
            else
            {
                currentStunTimer = 0;
                Stunned = false;
                if (GetComponent<HealthComp>().GetHealth() > 0)
                { 
                GetComponent<Animator>().SetTrigger("Revived");
                setState(State.Patrol);
                }else
                {
                    setState(State.Dead);
                }
            }
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Takedown");
            Stunned = true;
        }
    }

}
