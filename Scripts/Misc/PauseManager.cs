using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class PauseManager : MonoBehaviour {

    public KeyCode pauseKey = KeyCode.Escape;
    public bool isPaused = false;

    public GameObject pauseCanvas;

    private GameObject mainCamera;
    private Blur blur;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        blur = mainCamera.GetComponent<Blur>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            isPaused = !isPaused;
        }

        if(isPaused == true)
        {
            pauseCanvas.SetActive(true);
            blur.enabled = true;
            Time.timeScale = 0f;
        }
        else if(isPaused == false)
        {
            pauseCanvas.SetActive(false);
            blur.enabled = false;
            Time.timeScale = 1f;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Debug.Log("Quitting Application..");
        Application.Quit();
    }
}