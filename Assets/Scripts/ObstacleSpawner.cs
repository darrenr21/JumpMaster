using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject flyingObstaclePrefab;
    public GameObject coinPrefab;
    public GameObject redRingPrefab;
    public GameObject shieldPrefab;
    public GameObject magnetPrefab;
    public GameObject shrinkPrefab;

    public float obstacleSpawnInterval = 1.5f;
    public float redRingSpawnInterval = 30f;

    private float obstacleTimer;
    private float redRingTimer;
    private float lastObstacleY;
    private bool lastWasFlying;

    void Update()
    {
        obstacleTimer += Time.deltaTime;
        redRingTimer += Time.deltaTime;

        if (obstacleTimer >= obstacleSpawnInterval)
        {
            SpawnObstacle();
            SpawnCoinAfterObstacle();
            TrySpawnPowerUp();
            obstacleTimer = 0f;
        }

        if (redRingTimer >= redRingSpawnInterval)
        {
            SpawnRedRing();
            redRingTimer = 0f;
        }
    }

    void SpawnObstacle()
    {
        float random = Random.value;

        if (random < 0.6f)
        {
            // Ground obstacle
            Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
            lastWasFlying = false;
            lastObstacleY = transform.position.y;
        }
        else
        {
            // Flying obstacle
            float flyingY = Random.Range(1.5f, 3f);
            Vector3 flyingPos = transform.position + new Vector3(0, flyingY, 0);
            Instantiate(flyingObstaclePrefab, flyingPos, Quaternion.identity);
            lastWasFlying = true;
            lastObstacleY = flyingPos.y;
        }
    }

    void SpawnCoinAfterObstacle()
    {
        // Spawn coin between this obstacle and the next
        // Position it halfway between obstacles
        float coinX = transform.position.x - (obstacleSpawnInterval * 2.5f);

        // Coin Y position: above ground but reachable
        // If last obstacle was flying, put coin lower so player can get it without jumping into obstacle
        float coinY;
        if (lastWasFlying)
        {
            coinY = Random.Range(0f, 1f); // Lower coins when flying obstacle
        }
        else
        {
            coinY = Random.Range(0.5f, 2.5f); // Higher coins when ground obstacle
        }

        Vector3 coinPosition = new Vector3(transform.position.x + 0.75f, transform.position.y + coinY, 0);
        Instantiate(coinPrefab, coinPosition, Quaternion.identity);
    }

    void TrySpawnPowerUp()
    {
        float random = Random.value;

        // 25% chance to spawn a power-up after an obstacle
        if (random < 0.25f)
        {
            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        float random = Random.value;

        // Safe Y position: above ground, not too high
        float safeY = Random.Range(1f, 2.5f);
        // Offset X so it doesn't overlap with obstacle
        Vector3 pos = transform.position + new Vector3(1.5f, safeY, 0);

        if (random < 0.4f)
        {
            // 40% - shield
            Instantiate(shieldPrefab, pos, Quaternion.identity);
        }
        else if (random < 0.8f)
        {
            // 40% - magnet
            Instantiate(magnetPrefab, pos, Quaternion.identity);
        }
        else
        {
            // 20% - shrink
            Instantiate(shrinkPrefab, pos, Quaternion.identity);
        }
    }

    void SpawnRedRing()
    {
        // Spawn red ring at a safe height, offset from obstacles
        Vector3 pos = transform.position + new Vector3(2f, 1.5f, 0);
        Instantiate(redRingPrefab, pos, Quaternion.identity);
    }
}