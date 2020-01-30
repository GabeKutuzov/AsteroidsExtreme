using UnityEngine;
using System.Collections;

public class UnlockableSpawner : MonoBehaviour{

    public int minSpawnScore = 2400;
    public int maxSpawnScore = 3000;
    public float currentSpawnScore;
    private float newSpawnScore;

    public GameObject unlockablePrefab;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        SetSpawnScore();
    }

    private void Update()
    {
        if(scoreManager.score >= currentSpawnScore)
        {
            SpawnUnlockable();
            SetSpawnScore();
        }
    }

    public void SetSpawnScore()
    {
        newSpawnScore = scoreManager.score + Random.Range(minSpawnScore, maxSpawnScore);
        currentSpawnScore = newSpawnScore;
    }

    private void SpawnUnlockable()
    {
        GameObject unlockable = GameObject.FindGameObjectWithTag("Unlockable");

        if (unlockable == null)
        {
            Instantiate(unlockablePrefab, transform.position, unlockablePrefab.transform.rotation);
        }
    }
}