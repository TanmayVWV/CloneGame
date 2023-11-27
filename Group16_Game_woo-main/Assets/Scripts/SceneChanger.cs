using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadNextLevel();
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadGame(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadGame(int level)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(level);
    }
}
