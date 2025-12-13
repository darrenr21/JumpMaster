using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip menuMusic;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
}