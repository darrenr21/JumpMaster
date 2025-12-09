using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public float spawnInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObjects();
            timer = 0f;
        }
    }

    void SpawnObjects()
    {
        // Randomly spawn obstacle or coin
        if (Random.value > 0.5f)
        {
            Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 coinPosition = transform.position + new Vector3(0, 1f, 0);
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }
}