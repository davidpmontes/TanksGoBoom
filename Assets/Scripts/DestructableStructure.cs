using System.Collections;
using UnityEngine;

public class DestructableStructure : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxLife;

    private float life;
    private Bounds bounds;
    private float sinkingRate = 2f;

    private void Awake()
    {
        life = maxLife;
        bounds = GetComponent<Renderer>().bounds;
    }

    public void DamageReceived(Vector3 fromDirection, float power, float damage)
    {
        life -= 1f;
        if (life <= 0)
        {
            GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(DestroyStructure());
        }
    }

    private IEnumerator DestroyStructure()
    {
        float height = bounds.size.y;
        float sinkDistance = 0;

        StartCoroutine(SpawnExplosions());
        StartCoroutine(ShakeStructure());

        while(sinkDistance < height)
        {
            transform.Translate(Vector3.down * sinkingRate * Time.deltaTime);
            sinkDistance += sinkingRate * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator ShakeStructure()
    {
        while(true)
        {
            Quaternion newRotation = Quaternion.Euler(Random.Range(-3, 3),
                                                      Random.Range(-3, 3),
                                                      Random.Range(-3, 3));
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, newRotation, 100 * Time.deltaTime);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SpawnExplosions()
    {
        while(true)
        {
            var smallExplosion = ObjectPool.Instance.GetFromPoolInactive(Pools.ExplosionSmall);
            smallExplosion.transform.position = new Vector3(Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x),
                                                            bounds.center.y - bounds.extents.y,
                                                            Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z));
            smallExplosion.SetActive(true);

            yield return new WaitForSeconds(0.25f);
        }
    }
}