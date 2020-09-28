using System.Collections;
using UnityEngine;

public class LaserCannon : MonoBehaviour, IWeaponSystem
{
    [SerializeField] private GameObject[] barrels;

    private int barrelIdx = 0;
    private bool isFiring;

    public void StartFiring()
    {
        if (isFiring)
            return;

        isFiring = true;
        StartCoroutine(AlternateFiring());
    }

    public void StopFiring()
    {
        StopAllCoroutines();
        isFiring = false;
    }

    private IEnumerator AlternateFiring()
    {
        while(true)
        {
            var blueLaser = ObjectPool.Instance.GetFromPoolInactive(Pools.BlueLaser_Projectile);

            blueLaser.GetComponent<Laser>().Init(barrels[barrelIdx]);
            blueLaser.SetActive(true);

            barrelIdx = (barrelIdx + 1) % barrels.Length;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
