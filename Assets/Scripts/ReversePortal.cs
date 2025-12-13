using UnityEngine;

public class ReversePortal : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float pulseSpeed = 3f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        // Pulse animation
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
        transform.localScale = new Vector3(originalScale.x * pulse, originalScale.y, originalScale.z);

        if (transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayPortal();
            }

            PlayerController player = other.GetComponent<PlayerController>();

            ObstacleSpawner spawner = FindFirstObjectByType<ObstacleSpawner>();
            if (spawner != null)
            {
                spawner.DestroyAllObstacles();
                spawner.StartPortalPause(2f);
            }

            player.ResetGravity();
            player.StopReverseMode();
            player.StartPortalImmunity(3f);
            GameManager.instance.StopSpecialModes();

            Destroy(gameObject);
        }
    }
}