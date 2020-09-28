using UnityEngine;

public class SimpleProp : MonoBehaviour, IDamageable
{
    public void DamageReceived(Vector3 fromDirection, float power, float damage)
    {
        Destroy(gameObject);
    }
}
