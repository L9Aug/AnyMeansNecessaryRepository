using UnityEngine;
using System.Collections;

public class Standard_Enemy : Base_Enemy {

    /// <summary>
    /// CODED BY LEE BROOKES - UP687102  - LEEBROOKES@LIVE.COM
    /// </summary>

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
                GetComponent<BodyDetection>().FindBodies();
                GetComponent<EnvironmentDetection>().Detection();
                GetComponent<AudioDetection>().audioDetection();
                GetComponent<Enemy_Patrol>().Patrol();
                break;

            case State.Chase:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                GetComponent<Enemy_Chase>().Chase();
                GetComponent<Cover>().AICover();
                break;

            case State.Attack:
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;

            case State.wasShot:
                GetComponent<Enemy_Was_Shot>().WasShot();
                break;

            case State.Alerted:
                GetComponent<BodyDetection>().FindBodies();
                GetComponent<Enemy_Chase>().checkLost(GetComponent<FieldOfView>().FindVisibleTargets()); // constantly searches if player is within detection radius
                break;

            case State.InCover:
                GetComponent<Cover>().AICover();
                break;

            case State.Stunned:
                Stunai();
                break;

            case State.Dead:
                Agent.velocity = Vector3.zero;
                break;
        }
    }
}
