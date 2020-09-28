using UnityEngine;

public class EnemyMine : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject wso;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable obj))
        {
            obj.DamageReceived(Vector3.up,
                               1000, wso.damage);

            var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(Pools.ExplosionSmall);
            smallExplosion.transform.position = transform.position;
            smallExplosion.SetActive(true);

            Destroy(gameObject);
        }
    }
}
