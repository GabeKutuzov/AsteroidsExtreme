using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public int score = 0;
    public Text scoreText;

    public Text yourScoreText;
    public Text highScoreText;

    public GameObject scoreAddTextPlacement;
    public GameObject scoreAddTextPrefab;

    public Vector3 scoreAddTextPosition;

    private string scoreAddText;

    private void Update()
    {
        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        scoreText.text = (score.ToString());

        yourScoreText.text = ("Your Score: " + score);
        highScoreText.text = ("Highscore: " + PlayerPrefs.GetInt("Highscore"));
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        scoreAddText = ("+" + scoreToAdd);
        GameObject  instance = (GameObject)Instantiate(scoreAddTextPrefab, scoreAddTextPlacement.transform.position, scoreAddTextPlacement.transform.rotation);
        instance.transform.SetParent(scoreAddTextPlacement.transform);
        instance.GetComponent<Text>().text = scoreAddText;
    }

    public void ResetScore()
    {
        score = 0;
    }
}