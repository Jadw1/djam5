using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public HealthBar healthBar;
    public GameObject blend;
    public GameObject dead;
    public GameObject paused;
    public Button again;
    public Button menu;
    public Button resume;

    public void PauseScreen()
    {
        Time.timeScale = 0;
        paused.SetActive(true);
        dead.SetActive(false);
        
        var buttonResume = resume.GetComponent<Button>();
        buttonResume.onClick.AddListener(Resume);
        
        var buttonMenu = menu.GetComponent<Button>();
        buttonMenu.onClick.AddListener(MainMenu);
    }
    
    public void EndScreen()
    {
        Time.timeScale = 0;
        blend.SetActive(true);
        dead.SetActive(true);
        paused.SetActive(false);
        
        var buttonAgain = again.GetComponent<Button>();
        buttonAgain.onClick.AddListener(PlayAgain);
        
        var buttonMenu = menu.GetComponent<Button>();
        buttonMenu.onClick.AddListener(MainMenu);
    }

    void Resume()
    {
        GetComponentInParent<Player>().ResumeGame();
    }
    
    void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DevArcana");
    }

    void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
