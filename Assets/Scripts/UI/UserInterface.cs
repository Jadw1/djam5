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
        public MutationsList mutationsList;

        public void PauseScreen()
        {
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(true);
            deadScreen.gameObject.SetActive(false); 
        
            pauseScreen.resume.onClick.AddListener(Resume);
            pauseScreen.menu.onClick.AddListener(MainMenu);
        }
    
        public void EndScreen()
        {
            Time.timeScale = 0;
            pauseScreen.gameObject.SetActive(false);
            deadScreen.gameObject.SetActive(true);
        
            deadScreen.again.onClick.AddListener(PlayAgain);
            deadScreen.menu.onClick.AddListener(MainMenu);
        }

        public void Resume()
        {
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    
        void PlayAgain()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Levels");
        }

        void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
