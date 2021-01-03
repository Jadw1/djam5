using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button settings;
    public Button exit;
    
    void Start()
    {
        play.onClick.AddListener(PlayGame);
        settings.onClick.AddListener(OpenSettings);
        exit.onClick.AddListener(ExitGame);
    }
    
    private void PlayGame()
    {
        SceneManager.LoadScene("DevArcana");
    }
    
    private void OpenSettings()
    {
        throw new System.NotImplementedException();
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}
