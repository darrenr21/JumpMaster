using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Static Obstacles")]
    public GameObject obstaclePrefab;
    public GameObject ceilingObstaclePrefab;

    [Header("Moving Obstacles")]
    public GameObject movingGroundObstaclePrefab;
    public GameObject movingCeilingObstaclePrefab;
    public float movingObstacleScaleSpeed = 2f;

    [Header("Rotating Obstacles")]
    public GameObject rotatingObstaclePrefab;
    public float rotatingObstacleSpeed = 100f;

    [Header("Portals")]
    public GameObject purplePortalPrefab;
    public GameObject greenPortalPrefab;
    public GameObject bluePortalPrefab;
    public GameObject blueReversePortalPrefab;
    public float portalSpawnInterval = 20f;
    public float bluePortalDelay = 30f;
    public float reverseSpawnX = -15f;

    [Header("Collectibles")]
    public GameObject coinPrefab;
    public GameObject redRingPrefab;

    [Header("Power-ups")]
    public GameObject shieldPrefab;
    public GameObject magnetPrefab;
    public GameObject shrinkPrefab;

    [Header("Spawn Timing")]
    public float obstacleSpawnInterval = 1.5f;
    public float powerUpSpawnInterval = 10f;
    public float redRingSpawnInterval = 30f;

    [Header("Gap Settings")]
    public float gapSize = 3f;
    public float groundY = -4.5f;
    public float ceilingY = 5.5f;
    public float minObstacleHeight = 1f;
    public float maxObstacleHeight = 4f;

    [Header("Obstacle Set Chances")]
    [Range(0, 100)] public int staticGapChance = 40;
    [Range(0, 100)] public int movingGapChance = 30;
    [Range(0, 100)] public int rotatingObstacleChance = 30;

    private float obstacleTimer;
    private float powerUpTimer;
    private float redRingTimer;
    private float portalTimer;
    private float bluePortalTimer;
    private bool specialModeActive = false;
    private bool waitingForBluePortal = false;
    private float lastGapBottomY;
    private float lastGapTopY;
    private float portalPauseTimer = 0f;
    private bool isPaused = false;
    private bool lastPortalWasPurple = false;

    private bool redCoinChallengeActive = false;
    private float redCoinPauseTimer = 0f;

    void Update()
    {
        // Handle red coin challenge pause
        if (redCoinChallengeActive)
        {
            redCoinPauseTimer -= Time.deltaTime;
            if (redCoinPauseTimer <= 0)
            {
                redCoinChallengeActive = false;
            }

            // Still update portal and blue portal timers during red coin challenge
            if (waitingForBluePortal)
            {
                bluePortalTimer += Time.deltaTime;
                if (bluePortalTimer >= bluePortalDelay)
                {
                    SpawnBluePortal();
                    bluePortalTimer = 0f;
                    waitingForBluePortal = false;
                }
            }

            // Don't spawn obstacles during red coin challenge
            return;
        }

        // Handle portal pause
        if (isPaused)
        {
            portalPauseTimer -= Time.deltaTime;
            if (portalPauseTimer <= 0)
            {
                isPaused = false;
            }
            if (waitingForBluePortal)
            {
                bluePortalTimer += Time.deltaTime;
                if (bluePortalTimer >= bluePortalDelay)
                {
                    SpawnBluePortal();
                    bluePortalTimer = 0f;
                    waitingForBluePortal = false;
                }
            }
            return;
        }

        obstacleTimer += Time.deltaTime;
        powerUpTimer += Time.deltaTime;
        redRingTimer += Time.deltaTime;
        portalTimer += Time.deltaTime;

        if (waitingForBluePortal)
        {
            bluePortalTimer += Time.deltaTime;
            if (bluePortalTimer >= bluePortalDelay)
            {
                SpawnBluePortal();
                bluePortalTimer = 0f;
                waitingForBluePortal = false;
            }
        }

        if (obstacleTimer >= obstacleSpawnInterval)
        {
            SpawnObstacleSet();
            obstacleTimer = 0f;
        }

        if (powerUpTimer >= powerUpSpawnInterval)
        {
            SpawnPowerUp();
            powerUpTimer = 0f;
        }

        if (redRingTimer >= redRingSpawnInterval)
        {
            SpawnRedRing();
            redRingTimer = 0f;
        }

        if (portalTimer >= portalSpawnInterval && !specialModeActive && !waitingForBluePortal && !GameManager.instance.isReverseMode && !GameManager.instance.isGravityFlipped)
        {
            SpawnSpecialPortal();
            portalTimer = 0f;
        }
    }

    public void StartPortalPause(float duration)
    {
        isPaused = true;
        portalPauseTimer = duration;
    }

    public void StartRedCoinChallengePause(float duration)
    {
        redCoinChallengeActive = true;
        redCoinPauseTimer = duration;

        // Destroy all existing obstacles
        DestroyAllObstacles();
    }

    public void DestroyAllObstacles()
    {
        // Find and destroy all obstacles
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        // Also destroy regular coins (not red coins)
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            // Only destroy if it's not a red coin
            if (coin.GetComponent<RedCoin>() == null)
            {
                Destroy(coin);
            }
        }

        // Destroy power-ups
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerUp in powerUps)
        {
            Destroy(powerUp);
        }
    }

    void SpawnSpecialPortal()
    {
        float middleY = 1f; // Slightly higher
        Vector3 pos = new Vector3(transform.position.x, middleY, 0);

        if (lastPortalWasPurple)
        {
            Instantiate(greenPortalPrefab, pos, Quaternion.identity);
            lastPortalWasPurple = false;
        }
        else
        {
            Instantiate(purplePortalPrefab, pos, Quaternion.identity);
            lastPortalWasPurple = true;
        }

        specialModeActive = true;
        waitingForBluePortal = true;
    }

    void SpawnBluePortal()
    {
        float middleY = 1f; // Slightly higher

        if (GameManager.instance.isReverseMode)
        {
            Vector3 pos = new Vector3(reverseSpawnX, middleY, 0);
            Instantiate(blueReversePortalPrefab, pos, Quaternion.identity);
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x, middleY, 0);
            Instantiate(bluePortalPrefab, pos, Quaternion.identity);
        }

        specialModeActive = false;
    }

    void SpawnObstacleSet()
    {
        int random = Random.Range(0, 100);

        float spawnX;
        if (GameManager.instance.isReverseMode)
        {
            spawnX = reverseSpawnX;
        }
        else
        {
            spawnX = transform.position.x;
        }

        if (random < staticGapChance)
        {
            SpawnStaticGap(spawnX);
            SpawnCoinInGap(spawnX, 0.75f);
        }
        else if (random < staticGapChance + movingGapChance)
        {
            SpawnMovingGap(spawnX);
            SpawnCoinInGap(spawnX, 0.75f);
        }
        else
        {
            SpawnRotatingObstacle(spawnX);
        }
    }

    void SpawnStaticGap(float spawnX)
    {
        float groundObstacleHeight = Random.Range(minObstacleHeight, maxObstacleHeight);
        float groundObstacleTopY = groundY + groundObstacleHeight;

        lastGapBottomY = groundObstacleTopY;
        lastGapTopY = groundObstacleTopY + gapSize;

        float ceilingObstacleHeight = ceilingY - lastGapTopY;

        if (ceilingObstacleHeight < minObstacleHeight)
        {
            ceilingObstacleHeight = minObstacleHeight;
            lastGapTopY = ceilingY - ceilingObstacleHeight;
        }

        Vector3 groundPos = new Vector3(spawnX, groundY + (groundObstacleHeight / 2f), 0);
        GameObject groundObs = Instantiate(obstaclePrefab, groundPos, Quaternion.identity);
        groundObs.transform.localScale = new Vector3(1, groundObstacleHeight, 1);

        Vector3 ceilingPos = new Vector3(spawnX, ceilingY - (ceilingObstacleHeight / 2f), 0);
        GameObject ceilingObs = Instantiate(ceilingObstaclePrefab, ceilingPos, Quaternion.identity);
        ceilingObs.transform.localScale = new Vector3(1, ceilingObstacleHeight, 1);
    }

    void SpawnMovingGap(float spawnX)
    {
        float totalSpace = ceilingY - groundY;
        float availableSpace = totalSpace - gapSize;

        float startGroundHeight = Random.Range(minObstacleHeight, maxObstacleHeight);
        float startCeilingHeight = availableSpace - startGroundHeight;

        float minHeight = minObstacleHeight;
        float maxHeight = availableSpace - minObstacleHeight;

        Vector3 groundPos = new Vector3(spawnX, groundY + (startGroundHeight / 2f), 0);
        GameObject groundObs = Instantiate(movingGroundObstaclePrefab, groundPos, Quaternion.identity);
        groundObs.transform.localScale = new Vector3(1, startGroundHeight, 1);

        MovingObstacle groundMoving = groundObs.GetComponent<MovingObstacle>();
        groundMoving.scaleSpeed = movingObstacleScaleSpeed;
        groundMoving.minScaleY = minHeight;
        groundMoving.maxScaleY = maxHeight;
        groundMoving.isGroundObstacle = true;
        groundMoving.groundY = groundY;
        groundMoving.ceilingY = ceilingY;
        groundMoving.isGrowing = Random.value > 0.5f;

        Vector3 ceilingPos = new Vector3(spawnX, ceilingY - (startCeilingHeight / 2f), 0);
        GameObject ceilingObs = Instantiate(movingCeilingObstaclePrefab, ceilingPos, Quaternion.identity);
        ceilingObs.transform.localScale = new Vector3(1, startCeilingHeight, 1);

        MovingObstacle ceilingMoving = ceilingObs.GetComponent<MovingObstacle>();
        ceilingMoving.scaleSpeed = movingObstacleScaleSpeed;
        ceilingMoving.minScaleY = minHeight;
        ceilingMoving.maxScaleY = maxHeight;
        ceilingMoving.isGroundObstacle = false;
        ceilingMoving.groundY = groundY;
        ceilingMoving.ceilingY = ceilingY;
        ceilingMoving.isGrowing = !groundMoving.isGrowing;

        lastGapBottomY = groundY + startGroundHeight;
        lastGapTopY = lastGapBottomY + gapSize;
    }

    void SpawnRotatingObstacle(float spawnX)
    {
        float minY = groundY + 2f;
        float maxY = ceilingY - 2f;
        float randomY = Random.Range(minY, maxY);

        Vector3 pos = new Vector3(spawnX, randomY, 0);

        GameObject rotatingObs = Instantiate(rotatingObstaclePrefab, pos, Quaternion.identity);

        RotatingObstacle rotating = rotatingObs.GetComponent<RotatingObstacle>();
        rotating.rotationSpeed = rotatingObstacleSpeed;

        lastGapBottomY = groundY + 1f;
        lastGapTopY = ceilingY - 1f;
    }

    void SpawnCoinInGap(float spawnX, float xOffset)
    {
        float coinY = (lastGapBottomY + lastGapTopY) / 2f;

        float coinX;
        if (GameManager.instance.isReverseMode)
        {
            coinX = spawnX - xOffset;
        }
        else
        {
            coinX = spawnX + xOffset;
        }

        Vector3 coinPosition = new Vector3(coinX, coinY, 0);
        Instantiate(coinPrefab, coinPosition, Quaternion.identity);
    }

    void SpawnPowerUp()
    {
        float random = Random.value;

        float spawnX;
        if (GameManager.instance.isReverseMode)
        {
            spawnX = reverseSpawnX;
        }
        else
        {
            spawnX = transform.position.x;
        }

        float safeY = Random.Range(lastGapBottomY + 0.5f, lastGapTopY - 0.5f);
        Vector3 pos = new Vector3(spawnX + 1.5f, safeY, 0);

        if (random < 0.4f)
        {
            Instantiate(shieldPrefab, pos, Quaternion.identity);
        }
        else if (random < 0.8f)
        {
            Instantiate(magnetPrefab, pos, Quaternion.identity);
        }
        else
        {
            Instantiate(shrinkPrefab, pos, Quaternion.identity);
        }
    }

    void SpawnRedRing()
    {
        float spawnX;
        if (GameManager.instance.isReverseMode)
        {
            spawnX = reverseSpawnX;
        }
        else
        {
            spawnX = transform.position.x;
        }

        float safeY = (lastGapBottomY + lastGapTopY) / 2f;
        Vector3 pos = new Vector3(spawnX + 2f, safeY, 0);
        Instantiate(redRingPrefab, pos, Quaternion.identity);
    }
}