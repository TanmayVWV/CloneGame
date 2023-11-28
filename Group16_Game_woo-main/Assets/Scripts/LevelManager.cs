using SlimUI.ModernMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // If needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        UIManager ui = FindObjectOfType<UIManager>(); // Using FindObjectOfType to find UIManager
        if (ui != null)
        {
            ui.ShowGameOverScreen(); // Show the game over screen without referencing the death panel
        }
    }
}
