using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalType { GravityFlip, Reverse, Normal }
    public PortalType portalType;
    public float baseSpeed = 5f;

    void Update()
    {
        float moveSpeed = baseSpeed * GameManager.gameSpeed;
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
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayPortal();
            }

            PlayerController player = other.GetComponent<PlayerController>();

            ObstacleSpawner spawner = FindFirstObjectByType<ObstacleSpawner>();
            if (spawner != null)
            {
                spawner.StartPortalPause(3f);
            }

            switch (portalType)
            {
                case PortalType.GravityFlip:
                    player.FlipGravity();
                    player.StartPortalImmunity(3f);
                    GameManager.instance.StartGravityFlipMode();
                    break;
                case PortalType.Reverse:
                    player.StartReverseMode();
                    GameManager.instance.StartReverseMode();
                    break;
                case PortalType.Normal:
                    player.ResetGravity();
                    player.StopReverseMode();
                    player.StartPortalImmunity(3f);
                    GameManager.instance.StopSpecialModes();
                    break;
            }

            Destroy(gameObject);
        }
    }
}