using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected WeaponScriptableObject wso;

    protected float timeSpent;
    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
