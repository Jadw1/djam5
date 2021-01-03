using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button settings;
    public Button exit;
    public UI.Settings Settings;
    
    void Start()
    {
        play.onClick.AddListener(PlayGame);
        settings.onClick.AddListener(OpenSettings);
        exit.onClick.AddListener(ExitGame);
    }
    
    private void PlayGame()
    {
        SceneManager.LoadScene("Levels");
    }
    
    private void OpenSettings()
    {
        Settings.gameObject.SetActive(true);
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}
