using UnityEngine;

public class Coin : MonoBehaviour
{
    public float baseSpeed = 5f;
    private bool isReverseCoin = false;

    void Start()
    {
        // Remember if this coin was spawned during reverse mode
        isReverseCoin = GameManager.instance.isReverseMode;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;

        if (isReverseCoin)
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
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayCoin();
            }
            GameManager.instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}