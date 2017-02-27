using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>


public class Cover : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    private bool hideReady;

    private GameObject Player;
    private Vector3 distToTarget;
    private Vector3 closestTarget;
    private Vector3 currentTarget;
    private Vector3 distToPlayer;
    private Vector3 distCoverToPlayer;
    public bool allowCover;
    public bool movingToCover;
    private float hiddenTimer;
    private float hideLenght = 6;

    NavMeshAgent Agent;
    void Start()
    {
        movingToCover = false;
        Player = GameObject.Find("Player"); // for who you want the ai to chase
        Agent = GetComponent<NavMeshAgent>();
    }

    public void AICover()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //creates sphare collider using view radius and the target mask(player) for it to collide with and stores hits as an array

        if (targetsInViewRadius.Length != 0)
        {
            distToPlayer = transform.position - Player.transform.position; // stops ai entering cover if theyre > certian distance from the player
            if (allowCover) // ai won't attempt to search for cover unless they find one in the vicinity
            {
                for (int i = 0; i < targetsInViewRadius.Length; i++) // checks list of targets placed based on whats in the colliding sphare.
                {
                    Vector3 dirToTarget = (targetsInViewRadius[i].transform.position - Player.transform.position).normalized;
                    if (Vector3.Angle(Player.transform.forward, dirToTarget) < 180 / 2 || this.GetComponent<Base_Enemy>()._state == Base_Enemy.State.InCover) // determines if cover is behind player
                    {
                        //determining closest cover point to ai and to send ai there or to continue in chase state based on distance.
                        distToTarget = targetsInViewRadius[i].transform.position - transform.position;
                        distCoverToPlayer = currentTarget - Player.transform.position;
                        if (closestTarget == Vector3.zero)
                        {
                            closestTarget = targetsInViewRadius[i].transform.position - transform.position;
                            currentTarget = targetsInViewRadius[i].transform.position;
                        }
                        else if (closestTarget.magnitude <= 2.2f)
                        {

                            if (!allowCover) // simple timer for ai to remain in cover.
                            {
                                allowCover = true;
                            }
                            else if (hiddenTimer < hideLenght && allowCover)
                            {
                                if (targetsInViewRadius[i].GetComponent<CrouchCover>().isCrouchCover)
                                {
                                    GetComponent<Animator>().SetTrigger("Crouch");
                                }
                                hiddenTimer += Time.deltaTime;
                            }
                            else if (hiddenTimer >= hideLenght && allowCover)
                            {
                                Debug.Log("leaving cover");
                                movingToCover = false;
                                allowCover = false;
                                hiddenTimer = 0;
                                distToTarget = Vector3.zero;
                                closestTarget = Vector3.zero;
                                GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Chase);
                            }
                        }
                        else if (closestTarget.magnitude > distToTarget.magnitude)
                        {
                            closestTarget = distToTarget;
                            currentTarget = targetsInViewRadius[i].transform.position;
                        }
                        else if (distToPlayer.magnitude <= 15 && distToPlayer.magnitude >= 4.5f && distCoverToPlayer.magnitude >= 3.5) // if cover point is close enough to the player ai goes there. (stops ai who are coming from across the map from entering random cover points along the way which is pointless)
                        {
                            Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position, Color.black);
                            Agent.speed = 1;
                            movingToCover = true;
                            Agent.SetDestination(currentTarget);
                        }
                        else
                        {
                            Debug.DrawLine(transform.position, targetsInViewRadius[i].transform.position);
                            Debug.Log("not in range");
                            movingToCover = false;
                            GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Chase); // if no close cover point then continues to chase until cover is next called.
                        }
                    }else
                    {
                        allowCover = false;
                        hiddenTimer = 0;
                        distToTarget = Vector3.zero;
                        closestTarget = Vector3.zero; GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.Chase);
                    }
                }
            }
            else if (hiddenTimer < hideLenght && !allowCover)
            {
                 hiddenTimer += Time.deltaTime;
            }
            else if (hiddenTimer >= hideLenght && !allowCover)
            {
                hiddenTimer = 0; // reset to zero as currently same variable used when reaching the cover position

                if (distToPlayer.magnitude <= 15) // blocks ai entering cover state in random locations ridiculously far from the player
                {
                    allowCover = true;
                    distToTarget = Vector3.zero; //cleaning values to avoid an annoying bug.
                    closestTarget = Vector3.zero;
                    GetComponent<Standard_Enemy>().setState(Standard_Enemy.State.InCover);
                }
            }
        }
        else
        {
            distToTarget = Vector3.zero; // just resets to zero when not in use, it stops a bug.
            closestTarget = Vector3.zero;
        }
    }

}

#if UNITY_EDITOR
#region unityeditor debug visuals
[CustomEditor(typeof(Cover))]
public class CompanionHideEditor : Editor
{
    void OnSceneGUI()
    {
        Cover fow = (Cover)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
    }
}
#endregion
#endif