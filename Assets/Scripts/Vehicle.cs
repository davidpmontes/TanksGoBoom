using System.Collections;
using UnityEngine;

public class Vehicle : MonoBehaviour, IDamageable
{
    [SerializeField] private CivilianVehiclesScriptableObject cvso = default;

    // MAX ---> .90MAX (catch on fire) ---> .50MAX (tires blow off) ---> 0MAX (final destroy)
    // stage 0        stage 1                      stage 2                    stage 3

    private Rigidbody rb;
    private float life;
    private int stage;

    [SerializeField] private GameObject[] wheels;

    private AudioSource audioSource;
    private bool isPlayingCrashSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        life = cvso.startingLife;
        stage = 0;
    }

    public void DamageReceived(Vector3 fromDirectionNormal, float force, float damage)
    {
        life -= damage;
        rb.AddForce(fromDirectionNormal * force, ForceMode.Impulse);

        if (!isPlayingCrashSound)
        {
            isPlayingCrashSound = true;
            StartCoroutine(PlayCrash());
        }

        CheckLife();
    }

    private IEnumerator PlayCrash()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        isPlayingCrashSound = false;
    }

    private void CheckLife()
    {
        if (stage == 0)
        {
            if (life < 0.9f * cvso.startingLife)
            {
                stage = 1;
                StartCoroutine(CatchOnFire());
            }
        }
        else if (stage == 1)
        {
            if (life < 0.5f * cvso.startingLife)
            {
                stage = 2;

                foreach (var wheel in wheels)
                {
                    wheel.GetComponent<Wheel>().Launch();
                }

                rb.AddForce(Vector3.up * cvso.finalExplosionForce, ForceMode.Impulse);
                rb.AddTorque(Random.onUnitSphere * cvso.finalExplosionForce / 100, ForceMode.Impulse);

                var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(Pools.ExplosionSmall);
                smallExplosion.transform.position = transform.position;
                smallExplosion.SetActive(true);
            }
        }
        else if (stage == 2)
        {
            if (life < 0f)
            {
                var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(Pools.ExplosionSmall);
                smallExplosion.transform.position = transform.position;
                smallExplosion.SetActive(true);

                LevelManager.Instance.GetPlayer1().GetComponent<PlayerController>().TargetReportingDestroyed(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator CatchOnFire()
    {
        var fire = ObjectPool.Instance.GetFromPoolInactive(Pools.MW_RedFire);
        fire.transform.parent = transform;
        fire.transform.localPosition = Vector3.zero;
        fire.SetActive(true);

        while (true)
        {
            life -= 5 * Time.deltaTime;
            CheckLife();
            yield return null;
        }
    }
}
