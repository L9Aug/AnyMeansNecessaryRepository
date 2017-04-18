using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>
    /// 
public class Enemy_Was_Shot : MonoBehaviour {
    NavMeshAgent Agent;
    private GameObject Player;
    public Vector3 playerLastPos;
    public List<AudioClip> PainSounds = new List<AudioClip>();
    public AudioSource myAudioSource;

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player"); // for who you want the ai to chase
        Agent = GetComponent<NavMeshAgent>();
        GetComponent<HealthComp>().healthChanged.Add(HealthUpdate);
    }

    void HealthUpdate(float health, float ChangeInHealth, float armour)
    {
        if (health <= 0)
        {
            GetComponent<Base_Enemy>().Killai();
        }
    }

    public void Shot()
    {
        PlayerShotLocation();
        if (gameObject.tag == "StandardEnemy")
        {
            GetComponent<Standard_Enemy>().setState(Base_Enemy.State.wasShot);
        }
        else if (gameObject.tag == "Sniper")
        {
            GetComponent<Sniper_Enemy>().setState(Base_Enemy.State.wasShot);
        }
        else if (gameObject.tag == "ArmoredEnemy")
        {
        }
        else if (gameObject.tag == "Hunter")
        {
            GetComponent<Hunter_Enemy>().setState(Base_Enemy.State.wasShot);
        }

        // pain audio
        if (PainSounds.Count > 0)
        {
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(PainSounds[Random.Range(0, PainSounds.Count)]);
            }
        }

    }

    private Vector3 distToLastPos;

    public void WasShot()
    {


        distToLastPos = transform.position - playerLastPos;
        //Debug.Log(GetComponent<FieldOfView>().FindVisibleTargets());

        if (gameObject.tag != "Sniper")
        {
            //Debug.Log(Agent);
            Agent.speed = 1;
            GetComponent<NavMeshAgent>().SetDestination(playerLastPos);
        }
    }

    private void PlayerShotLocation()
    {
        playerLastPos = Vector3.zero;
        Agent.speed = 1;
        playerLastPos = Player.transform.position;
    }
}
