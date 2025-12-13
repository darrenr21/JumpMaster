using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, Magnet, Shrink }
    public PowerUpType powerUpType;
    public float baseSpeed = 5f;
    public float duration = 5f;
    public float bobSpeed = 3f;
    public float bobHeight = 0.3f;
    private bool isReversePowerUp = false;
    private float startY;

    void Start()
    {
        isReversePowerUp = GameManager.instance.isReverseMode;
        startY = transform.position.y;
    }

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;

        // Bob up and down animation
        float newY = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        if (isReversePowerUp)
        {
            transform.position = new Vector3(
                transform.position.x + moveSpeed * Time.deltaTime,
                newY,
                transform.position.z
            );

            if (transform.position.x > 15f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = new Vector3(
                transform.position.x - moveSpeed * Time.deltaTime,
                newY,
                transform.position.z
            );

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
                AudioManager.instance.PlayPowerUp();
            }
            PlayerController player = other.GetComponent<PlayerController>();
            player.ActivatePowerUp(powerUpType, duration);
            Destroy(gameObject);
        }
    }
}