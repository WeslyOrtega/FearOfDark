using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject panel;
    public AudioMixer audioMixer;
    public Slider soundSlider;
    private bool isActive = false;

    public void Toggle()
    {
        isActive = !isActive;

        if (isActive)
        {
            float newVolume;
            audioMixer.GetFloat("MasterVolume", out newVolume);
            soundSlider.value = -Mathf.Sqrt(Mathf.Pow(80, 2) - Mathf.Pow(newVolume + 80, 2));
        }

        panel.SetActive(isActive);
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Sqrt(Mathf.Pow(80, 2) - Mathf.Pow(volume, 2)) - 80;
        audioMixer.SetFloat("MasterVolume", volume);
    }
}
