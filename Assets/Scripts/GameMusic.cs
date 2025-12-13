using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public static GameMusic instance;

    public AudioClip gameMusic;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        // If music already exists, destroy this duplicate
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Call this when returning to main menu to stop game music
    public void StopMusic()
    {
        Destroy(gameObject);
        instance = null;
    }
}