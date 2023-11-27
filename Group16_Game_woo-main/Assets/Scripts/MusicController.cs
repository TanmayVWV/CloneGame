using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider musicSlider; 

    private AudioSource musicSource; 

    void Start()
    {
        
        musicSource = GetComponent<AudioSource>();

        
        if (musicSource != null && musicSlider != null)
        {
            
            musicSource.volume = musicSlider.value;

            
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
        else
        {
            Debug.LogWarning("Music source or slider reference is missing!");
        }
    }

    void OnMusicSliderChanged(float value)
    {
        
        musicSource.volume = value;
    }
}
