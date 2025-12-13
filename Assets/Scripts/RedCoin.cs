using UnityEngine;

public class RedCoin : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float rotationSpeed = 200f;
    public float pulseSpeed = 5f;
    private bool isReverseCoin = false;
    private Vector3 originalScale;

    void Start()
    {
        isReverseCoin = GameManager.instance.isReverseMode;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;

        // Spin animation
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Pulse animation
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.2f;
        transform.localScale = originalScale * pulse;

        if (isReverseCoin)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (transform.position.x > 15f)
            {
                GameManager.instance.MissedRedCoin();
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (transform.position.x < -15f)
            {
                GameManager.instance.MissedRedCoin();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayRedCoin();
            }
            GameManager.instance.CollectRedCoin();
            Destroy(gameObject);
        }
    }
}