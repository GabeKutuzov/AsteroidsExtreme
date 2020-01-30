using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public float minSpawnTimer = 3f;
    public float maxSpawnTimer = 5f;
    public float currentSpawnTimer;
    private float newSpawnTimer;

    public GameObject[] asteroidPrefabs;

    private void Start()
    {
        SetSpawnTimer();
    }

    private void Update()
    {
        currentSpawnTimer -= Time.deltaTime;

        if(currentSpawnTimer <= 0f)
        {
            SpawnAsteroid();
            SetSpawnTimer();
        }
    }

    private void SetSpawnTimer()
    {
        newSpawnTimer = Random.Range(minSpawnTimer, maxSpawnTimer);
        currentSpawnTimer = newSpawnTimer;
    }

    private void SpawnAsteroid()
    {
        GameObject instance = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        Instantiate(instance, transform.position, instance.transform.rotation);
    }
}