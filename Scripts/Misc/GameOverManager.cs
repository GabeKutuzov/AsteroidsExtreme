using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public GameObject gameOverCanvas;
    public float waitToContinueTimer = 2f;
    private float waitToContinueTimerStart;

    private bool gameOver = false;

    [HideInInspector]
    public bool canContinue = false;

    private void Start()
    {
        waitToContinueTimerStart = waitToContinueTimer;
    }

    private void Update()
    {
        if(gameOver == true && canContinue == false)
        {
            waitToContinueTimer -= Time.deltaTime;
        }

        if(waitToContinueTimer <= 0)
        {
            canContinue = true;
        }
    }

    public void GameOverSet(bool isGameOver)
    {
        gameOver = isGameOver;

        if(isGameOver == true)
        {
            gameOverCanvas.SetActive(true);
        }
        else if(isGameOver == false)
        {
            gameOverCanvas.SetActive(false);
            canContinue = false;
            waitToContinueTimer = waitToContinueTimerStart;
        }
    }


}