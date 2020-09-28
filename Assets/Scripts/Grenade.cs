using UnityEngine;

public class Grenade : Projectile
{
    private const float MAX_TIME = 5;
    private const float LAUNCH_SPEED = 25;

    private bool isInitialFlight;

    [SerializeField] private GameObject mesh;

    public void Init(GameObject barrel)
    {
        isInitialFlight = true;
        rb.isKinematic = true;

        transform.position = barrel.transform.position;
        transform.rotation = barrel.transform.rotation;

        rb.isKinematic = false;
        rb.velocity = transform.forward * LAUNCH_SPEED;

        timeSpent = 0;
    }

    private void Update()
    {
        timeSpent += Time.deltaTime;

        if (isInitialFlight)
            mesh.transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);

        if (timeSpent > MAX_TIME)
        {
            ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isInitialFlight = false;

        if (collision.gameObject.TryGetComponent(out IDamageable obj))
        {
            obj.DamageReceived(rb.velocity, 1000, wso.damage);
            ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
        }
    }
}
