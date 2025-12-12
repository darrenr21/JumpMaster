using UnityEngine;

public class ReversePortal : MonoBehaviour
{
    public float baseSpeed = 5f;

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        if (transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            ObstacleSpawner spawner = FindFirstObjectByType<ObstacleSpawner>();
            if (spawner != null)
            {
                spawner.StartPortalPause(3f);
            }

            player.ResetGravity();
            player.StopReverseMode();
            player.StartPortalImmunity(3f);
            GameManager.instance.StopSpecialModes();

            Destroy(gameObject);
        }
    }
}