using UnityEngine;

public class RedRing : MonoBehaviour
{
    public float baseSpeed = 5f;
    private bool isReverseRing = false;

    void Start()
    {
        isReverseRing = GameManager.instance.isReverseMode;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;

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