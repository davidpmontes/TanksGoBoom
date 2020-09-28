using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float currentLife;
    [SerializeField] private Pools smallExplosionPool;
    [SerializeField] private Pools finalExplosionPool;
    private bool isDying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDying)
        {
            return;
        }

        currentLife -= 1;

        if (currentLife <= 0)
        {
            isDying = true;

            var currVelocity = GetComponent<CharacterController>().velocity;

            GetComponent<VehicleController>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<AudioSource>().enabled = false;

            var rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 1000;
            rb.velocity = currVelocity;
            rb.AddForce(Vector3.up * 10000, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere * 10000, ForceMode.Impulse);

            gameObject.layer = LayerMask.NameToLayer("dyingEnemy");
        }
        else
        {
            var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(smallExplosionPool);
            smallExplosion.transform.position = transform.position;
            smallExplosion.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying)
        {
            var finalExplosion = ObjectPool.Instance.GetFromPoolInactive(finalExplosionPool);
            finalExplosion.transform.position = transform.position;
            finalExplosion.SetActive(true);

            Destroy(gameObject);
        }
    }
}
