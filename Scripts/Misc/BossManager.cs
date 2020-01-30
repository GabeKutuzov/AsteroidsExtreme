using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossManager : MonoBehaviour {

    public int minSpawnScore = 100000;
    public int maxSpawnScore = 120000;
    public float currentSpawnScore;
    private float newSpawnScore;

    public bool bossActive = false;

    public GameObject bossHealthCanvas;
    public Image bossHealthBar;

    public GameObject[] bossPrefabs;

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
            SpawnBoss();
            SetSpawnScore();
        }
    }

    private void SpawnBoss()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        bossActive = true;

        bossHealthCanvas.SetActive(true);

        if (boss == null)
        {
            Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }

    public void BossDeath()
    {
        bossHealthCanvas.SetActive(false);
        bossActive = false;
    }

    public void SetSpawnScore()
    {
        newSpawnScore = scoreManager.score + Random.Range(minSpawnScore, maxSpawnScore);
        currentSpawnScore = newSpawnScore;
    }

}