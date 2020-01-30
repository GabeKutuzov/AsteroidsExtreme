using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject menuCanvas;
    public GameObject loadingCanvas;
    public GameObject settingsCanvas;

    public AudioClip buttonClip;
    private AudioSource audSource;

    public Text musicToggleText;
    public GameObject backgroundMusicPrefab;

    public MenuState menuState = new MenuState();
    public enum MenuState { Main, Settings, Loading };

    private bool musicOn = true;

    private void Start()
    {
        SetMenuState(MenuState.Main);
        SpawnBackgroundMusic();

        audSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        PlayButtonClick();

        Debug.Log("Loading Game..");
        SetMenuState(MenuState.Loading);
        SceneManager.LoadSceneAsync("Game");
    }

    public void ExitGame()
    {
        PlayButtonClick();

        Debug.Log("Quit Application..");
        Application.Quit();
    }

    private void SpawnBackgroundMusic()
    {
        GameObject bgMusicObj = GameObject.FindGameObjectWithTag("BackgroundMusic");

        if(bgMusicObj == null)
        {
            Instantiate(backgroundMusicPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    public void SetMenuStateMain()
    {
        PlayButtonClick();

        SetMenuState(MenuState.Main);
    }

    public void SetMenuStateSettings()
    {
        PlayButtonClick();

        SetMenuState(MenuState.Settings);
    }

    public void SetMenuState(MenuState state)
    {
        menuState = state;

        if(menuState == MenuState.Main)
        {
            menuCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
            loadingCanvas.SetActive(false);
        }
        else if(menuState == MenuState.Settings)
        {
            menuCanvas.SetActive(false);
            settingsCanvas.SetActive(true);
            loadingCanvas.SetActive(false);
        }
        else if(menuState == MenuState.Loading)
        {
            menuCanvas.SetActive(false);
            settingsCanvas.SetActive(false);
            loadingCanvas.SetActive(true);
        }
    }

    public void ToggleMusic()
    {
        PlayButtonClick();

        musicOn = !musicOn;

        if(musicOn == true)
        {
            GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>().Play();
            musicToggleText.text = ("Music: ON");
        }
        else if(musicOn == false)
        {
            GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>().Stop();
            musicToggleText.text = ("Music: OFF");
        }
    }

    public void PlayButtonClick()
    {
        audSource.PlayOneShot(buttonClip);
    }
}