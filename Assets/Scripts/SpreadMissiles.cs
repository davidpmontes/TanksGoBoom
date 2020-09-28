using UnityEngine;

public class SpreadMissiles : MonoBehaviour, IWeaponSystem
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private AudioClip clip;
    private AudioSource audiosource;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void StartFiring()
    {
        audiosource.PlayOneShot(clip);

        var rocket1 = ObjectPool.Instance.GetFromPoolInactive(Pools.Rocket_Projectile);
        rocket1.GetComponent<Rocket>().Init(barrel);
        rocket1.SetActive(true);
        rocket1.GetComponent<Rocket>().StartSpreading("left");

        var rocket2 = ObjectPool.Instance.GetFromPoolInactive(Pools.Rocket_Projectile);
        rocket2.GetComponent<Rocket>().Init(barrel);
        rocket2.SetActive(true);

        var rocket3 = ObjectPool.Instance.GetFromPoolInactive(Pools.Rocket_Projectile);
        rocket3.GetComponent<Rocket>().Init(barrel);
        rocket3.SetActive(true);
        rocket3.GetComponent<Rocket>().StartSpreading("right");
    }

    public void StopFiring()
    {
    }
}
