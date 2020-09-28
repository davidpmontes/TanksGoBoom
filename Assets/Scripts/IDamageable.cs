using UnityEngine;

public interface IDamageable
{
    void DamageReceived(Vector3 direction, float force, float damage);
}
