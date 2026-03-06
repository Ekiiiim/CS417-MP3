using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject powerUpPrefab;

    [Header("Spawn Area")]
    // Highest X and Z coordinates power ups can spawn at
    [SerializeField] private Vector2 spawnBoundaryMax;
    // Lowest X and Z coordinates power ups can spawn at before unlocking power plant
    [SerializeField] private Vector2 spawnBoundMin;

    [Header("Variables")]
    [SerializeField] private float minTime = 0.0f;
    [SerializeField] private float maxTime = 1.0f;
    private float timeToSpawn;
    private float timer = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeToSpawn = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToSpawn)
        {
            SpawnPowerUp();
            timeToSpawn = Random.Range(minTime, maxTime);
            timer -= timeToSpawn;
        }
    }

    void SpawnPowerUp()
    {
        var spawnPoint = new Vector3(Random.Range(spawnBoundMin.x, spawnBoundaryMax.x), 1f, Random.Range(spawnBoundMin.y, spawnBoundaryMax.y));
        Instantiate(powerUpPrefab, spawnPoint, Quaternion.identity);
    }
}
