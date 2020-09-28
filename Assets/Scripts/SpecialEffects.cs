using UnityEngine;

public class SpecialEffects : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    public void FootStep()
    {
        audioSource.PlayOneShot(clip);
    }
}
