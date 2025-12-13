using UnityEngine;

public class RedRing : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float pulseSpeed = 4f;
    public float rotationSpeed = 50f;
    private bool isReverseRing = false;
    private Vector3 originalScale;

    void Start()
    {
        isReverseRing = GameManager.instance.isReverseMode;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;

        // Rotate animation
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Pulse animation
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.15f;
        transform.localScale = originalScale * pulse;

        if (isReverseRing)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (transform.position.x > 15f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.StartRedCoinChallenge(transform.position);
            Destroy(gameObject);
        }
    }
}