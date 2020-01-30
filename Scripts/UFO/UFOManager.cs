using UnityEngine;
using System.Collections;

public class UFOManager : MonoBehaviour {

    public int minSpawnScore = 2400;
    public int maxSpawnScore = 3000;
    public int maxUFOCount = 2;
    public float currentSpawnScore;
    private float newSpawnScore;

    [HideInInspector]
    public bool UFOisSpawned = false;

    public GameObject ufoPrefab;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        SetSpawnScore();
    }

    private void Update()
    {
        if (scoreManager.score >= currentSpawnScore)
        {
            SpawnUFO();
            SetSpawnScore();
        }
    }

    public void SetSpawnScore()
    {
        newSpawnScore = scoreManager.score + Random.Range(minSpawnScore, maxSpawnScore);
        currentSpawnScore = newSpawnScore;
    }

    private void SpawnUFO()
    {
        // UFO Spawn Points
        GameObject[] allUFOSpawns = GameObject.FindGameObjectsWithTag("UFOSpawner");
        GameObject ufoSpawn = allUFOSpawns[Random.Range(0, allUFOSpawns.Length)];

        GameObject[] allUFOs = GameObject.FindGameObjectsWithTag("UFO");

        if(allUFOs.Length < maxUFOCount)
        {
            Instantiate(ufoPrefab, ufoSpawn.transform.position, ufoPrefab.transform.rotation);
        }
        else
        {
            SetSpawnScore();
        }
    }
}