using UnityEngine;

public class NeutralObjectInteractions : MonoBehaviour
{
    private CharacterController cc;

    [SerializeField] private PlayerScriptableObject playerSettings;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("neutralInteractWithPlayer"))
        {
            if (hit.gameObject.TryGetComponent(out IDamageable script))
            {
                script.DamageReceived(cc.velocity.normalized,
                                      playerSettings.mass * cc.velocity.magnitude,
                                      playerSettings.mass * cc.velocity.magnitude);
            }
        }
    }
}
