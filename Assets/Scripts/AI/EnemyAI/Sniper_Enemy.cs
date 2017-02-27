using UnityEngine;
using System.Collections;

public class Sniper_Enemy : Base_Enemy {

    //public State _state;
    GameObject Player
    {
        get
        {
            return (PlayerController.PC != null) ? PlayerController.PC.gameObject : null;
        }
    }

    void Update()
    {
        FSM();
    }


    private void FSM()
    {
        switch (_state)
        {
            case State.Patrol:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Patrol>().Patrol();
                break;

            case State.Chase:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Chase>().Sniperchase();
                break;

            case State.wasShot:
                GetComponent<Enemy_Was_Shot>().WasShot();
                Vector3 PlayerLookPos = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
                transform.LookAt(PlayerLookPos); // semi buggy meh
                break;

            case State.InCover:
                GetComponent<Cover>().AICover();
                break;

            case State.Stunned:
                Stunai();
                break;

            case State.Dead:
                break;
        }
    }
}
