using System.Collections;
using UnityEngine;

public class Rocket : Projectile
{
    public void Init(GameObject barrel)
    {
        transform.position = barrel.transform.position;
        transform.rotation = barrel.transform.rotation;

        timeSpent = 0;
    }

    public void StartSpreading(string spreadDirection)
    {
        StartCoroutine(SpreadOut(spreadDirection));
    }

    private IEnumerator SpreadOut(string direction)
    {
        float targetLateralDistance = 3f;
        float lateralTraveled = 0f;

        while (lateralTraveled < targetLateralDistance)
        {
            if (direction == "left")
            {
                transform.Translate(-transform.right * wso.spreadSpead * Time.deltaTime, Space.World);
            }
            else if (direction == "right")
            {
                transform.Translate(transform.right * wso.spreadSpead * Time.deltaTime, Space.World);
            }
            lateralTraveled += wso.spreadSpead * Time.deltaTime;

            yield return null;
        }
    }

    private void Update()
    {
        transform.Translate(transform.forward * wso.speed * Time.deltaTime, Space.World);

        timeSpent += Time.deltaTime;

        if (timeSpent > wso.maxTime)
        {
            BlowUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable obj))
        {
            obj.DamageReceived(transform.forward, wso.mass * wso.speed, wso.damage);
        }

        BlowUp();
    }

    private void BlowUp()
    {
        var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(Pools.ExplosionSmall);
        smallExplosion.transform.position = transform.position;
        smallExplosion.SetActive(true);

        ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
    }
}