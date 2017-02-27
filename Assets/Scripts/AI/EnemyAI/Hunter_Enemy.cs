using UnityEngine;
using System.Collections;

public class Hunter_Enemy : Base_Enemy {

    //public State _state;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
                GetComponent<Enemy_Chase>().Chase();
                break;

            case State.wasShot:
                GetComponent<Enemy_Was_Shot>().WasShot();
                break;

            case State.InCover:
                break;

            case State.Stunned:
                Stunai();
                break;

            case State.Dead:
                break;
        }
    }
}
