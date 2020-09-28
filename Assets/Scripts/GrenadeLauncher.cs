using UnityEngine;

public class GrenadeLauncher : MonoBehaviour, IWeaponSystem
{
    [SerializeField] private GameObject barrel;

    public void StartFiring()
    {
        var grenade = ObjectPool.Instance.GetFromPoolInactive(Pools.Grenade_Projectile);
        grenade.GetComponent<Grenade>().Init(barrel);
        grenade.SetActive(true);
    }

    public void StopFiring()
    {
    }
}
