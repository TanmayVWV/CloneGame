using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Animator animator;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }  
    }
    public void Resume()
    {
        animator.SetTrigger("END");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;  
        GameIsPaused = false;
    }
    void Pause()
    {
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenuUI");
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
}
