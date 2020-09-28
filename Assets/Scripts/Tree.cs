using System.Collections;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    public void DamageReceived(Vector3 fromDirection, float power, float damage)
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<AudioSource>().Play();
        StartCoroutine(FallDown(fromDirection));
    }

    private IEnumerator FallDown(Vector3 fromDirection)
    {
        float degree = 0;
        var angleOfRotation = Vector3.Cross(fromDirection, Vector3.down);

        while(degree < 90)
        {
            transform.rotation = Quaternion.AngleAxis(degree, angleOfRotation);
            degree += 90 * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
