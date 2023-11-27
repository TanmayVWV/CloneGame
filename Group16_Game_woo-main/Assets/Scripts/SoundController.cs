using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Slider soundSlider; 
    public AudioSource[] soundSources; 

    void Start()
    {
        
        if (soundSources != null && soundSlider != null)
        {
            foreach (AudioSource soundSource in soundSources)
            {
                soundSource.volume = soundSlider.value;
            }

            
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        }
        else
        {
            Debug.LogWarning("One or more references missing for sound sources or slider!");
        }
    }

    void OnSoundSliderChanged(float value)
    {
        
        foreach (AudioSource soundSource in soundSources)
        {
            soundSource.volume = value;
        }
    }
}
