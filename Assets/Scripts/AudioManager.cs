using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip redCoinSound;
    public AudioClip powerUpSound;
    public AudioClip portalSound;
    public AudioClip gameOverSound;
    public AudioClip shieldBreakSound;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        // Add AudioSource if missing
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayJump()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void PlayCoin()
    {
        if (coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    public void PlayRedCoin()
    {
        if (redCoinSound != null)
        {
            audioSource.PlayOneShot(redCoinSound);
        }
    }

    public void PlayPowerUp()
    {
        if (powerUpSound != null)
        {
            audioSource.PlayOneShot(powerUpSound);
        }
    }

    public void PlayPortal()
    {
        if (portalSound != null)
        {
            audioSource.PlayOneShot(portalSound);
        }
    }

    public void PlayGameOver()
    {
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    public void PlayShieldBreak()
    {
        if (shieldBreakSound != null)
        {
            audioSource.PlayOneShot(shieldBreakSound);
        }
    }
}