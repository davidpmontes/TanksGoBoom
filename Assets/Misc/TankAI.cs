using UnityEngine;
using AnimatorStateMachineUtil;
using System.Collections;

public class TankAI : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject[] waypoints;

    private int wpIdx = 0;
    private GameObject turretRotateable;
    public Vector2 leftStickInput;
    public Vector2 rightStickInput;
    public bool primaryWeaponInput;

    private void Awake()
    {
        turretRotateable = Utils.FindChildByNameRecursively(transform, "TurretRotateable");
    }

    [StateEnterMethod("Base.Waypoints")]
    public void Search_Enter()
    {
        //StartCoroutine(SearchForPlayer());
    }

    [StateUpdateMethod("Base.Waypoints")]
    public void Search_Update()
    {
        MoveToWaypoint();
        TrackPlayer();
    }

    [StateExitMethod("Base.Waypoints")]
    public void Search_Exit()
    {
        StopAllCoroutines();
    }

    [StateEnterMethod("Base.Attack")]
    public void Attack_Enter()
    {

    }

    [StateUpdateMethod("Base.Attack")]
    public void Attack_Update()
    {
        Debug.Log("fire");
    }

    [StateExitMethod("Base.Attack")]
    public void Attack_Exit()
    {

    }

    private IEnumerator SearchForPlayer()
    {
        while (true)
        {
            if (target)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < 10f)
                {
                    GetComponent<Animator>().SetTrigger("FoundTarget");
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void MoveToWaypoint()
    {
        var dirToWayPoint = (waypoints[wpIdx].transform.position - transform.position).normalized;
        var stickDir = turretRotateable.transform.InverseTransformDirection(dirToWayPoint);

        leftStickInput = new Vector2(stickDir.x, stickDir.z);

        var distance = Vector3.Distance(waypoints[wpIdx].transform.position, transform.position);
        if (distance < 10)
        {
            wpIdx = (wpIdx + 1) % waypoints.Length;
        }
    }

    private void TrackPlayer()
    {
        if (!target)
        {
            primaryWeaponInput = false;
            return;
        }

        var dirToTarget = (target.transform.position - transform.position).normalized;
        var crossResult = Vector3.Cross(turretRotateable.transform.forward, dirToTarget);

        rightStickInput = new Vector2(crossResult.y, 0);
        primaryWeaponInput = true;
    }
}
