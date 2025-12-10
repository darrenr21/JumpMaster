using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, Magnet, Shrink }
    public PowerUpType powerUpType;
    public float moveSpeed = 5f;
    public float duration = 5f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.ActivatePowerUp(powerUpType, duration);
            Destroy(gameObject);
        }
    }
}