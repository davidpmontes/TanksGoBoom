using UnityEngine;

public class JukeBox : MonoBehaviour
{
    public static JukeBox Instance { get; private set; }

    [SerializeField] private AudioClip[] soundtracks;

    private AudioSource audioSource;

    public void Init()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.clip = soundtracks[0];
        audioSource.Play();
    }
}
