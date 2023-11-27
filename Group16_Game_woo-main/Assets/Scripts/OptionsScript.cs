using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour
{
    public Animator animatorx;
    public static bool GameIsPaused = false;
    public GameObject OptionsMenuUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Resume()
    {
        OptionsMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        OptionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayOptionsAnimation(); // Call the method to play the animation
    }

    // New method to play the animation
    public void PlayOptionsAnimation()
    {
        animatorx.SetTrigger("NewStart");
    }
}
