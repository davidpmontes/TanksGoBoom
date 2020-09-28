using UnityEngine;

public class Laser : Projectile
{
    public void Init(GameObject barrel)
    {
        transform.position = barrel.transform.position;
        transform.rotation = barrel.transform.rotation;
        timeSpent = 0;
    }

    private void Update()
    {
        transform.Translate(transform.forward * wso.speed * Time.deltaTime, Space.World);

        timeSpent += Time.deltaTime;
        if (timeSpent > wso.maxTime)
        {
            ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable obj))
        {
            obj.DamageReceived(transform.forward,
                               wso.mass * wso.speed,
                               wso.damage);
        }

        var laserImpact = ObjectPool.Instance.GetFromPoolInactive(Pools.MW_BlueLaserImpact);
        laserImpact.transform.position = transform.position;
        laserImpact.SetActive(true);

        ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
    }
}
