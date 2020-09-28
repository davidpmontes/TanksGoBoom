using System.Collections;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public void Launch()
    {
        var wheelRb = gameObject.AddComponent<Rigidbody>();
        wheelRb.mass = 100;
        gameObject.AddComponent<BoxCollider>();

        wheelRb.AddForce((transform.position - transform.parent.transform.position) * 1000, ForceMode.Impulse);
        wheelRb.AddTorque(Random.onUnitSphere * 100, ForceMode.Impulse);

        transform.parent = null;

        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
