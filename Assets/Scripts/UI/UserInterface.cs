using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        public HealthBar healthBar;
        public DeadScreen deadScreen;
        public PauseScreen pauseScreen;

        public void PauseScreen()
        {
            Debug.LogError("Pause");
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(true);
            deadScreen.gameObject.SetActive(false); 
        
            pauseScreen.resume.onClick.AddListener(Resume);
            pauseScreen.menu.onClick.AddListener(MainMenu);
        }
    
        public void EndScreen()
        {
            Debug.LogError("Dead");
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(false);
            deadScreen.gameObject.SetActive(true);
        
            deadScreen.again.onClick.AddListener(PlayAgain);
            deadScreen.menu.onClick.AddListener(MainMenu);
        }

        public void Resume()
        {
            Debug.LogError("Click Resume");
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    
        void PlayAgain()
        {
            Debug.LogError("Click PlayAgain");
            Time.timeScale = 1;
            SceneManager.LoadScene("DevArcana");
        }

        void MainMenu()
        {
            Debug.LogError("Click MainMenu");
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
