using System.Collections;
using UnityEngine;
using WSMGameStudio.Splines;

public class TrainCar : MonoBehaviour, IDamageable
{
    [SerializeField] private TrainCarScriptableObject tcso = default;

    [SerializeField] private GameObject mainChassis;
    [SerializeField] private GameObject[] subcomponents;
    [SerializeField] private GameObject tailcar;

    private SplineFollower splineFollower;

    private float life;

    private void Awake()
    {
        splineFollower = GetComponent<SplineFollower>();
        life = tcso.startingLife;
    }

    public void DamageReceived(Vector3 direction, float force, float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Disconnect();
            Destroy(gameObject);
        }
    }

    public void Disconnect()
    {
        if (tailcar)
        {
            tailcar.GetComponent<TrainCar>().Disconnect();
        }
        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown()
    {
        while (splineFollower.speed > 0)
        {
            splineFollower.speed -= Time.deltaTime;
            yield return null;
        }
        Destroy(splineFollower);
    }
}
