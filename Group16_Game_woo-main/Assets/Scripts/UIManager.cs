using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;

    public void ShowGameOverScreen()
    {
        if (deathPanel != null)
        {
            // Activate the death panel UI GameObject
            deathPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Death Panel GameObject not assigned in UIManager.");
            // Handle the situation where the death panel is not assigned
        }
    }
    public void RetryGame()
    {
        // You can add additional reset logic here if needed
        SceneManager.LoadScene("Prototype_level");
    }
    public void BackToMain()
    {
        // You can add additional reset logic here if needed
        SceneManager.LoadScene("MainMenu");
    }
}
