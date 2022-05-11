using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject optionsPanel;
    public LevelManager levelManager;
    public AudioMixer audioMixer;
    public Slider soundSlider;
    private bool isPaused = false;
    private bool isOptionsActive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {

            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        menu.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        isOptionsActive = false;
        optionsPanel.SetActive(false);
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Options()
    {
        isOptionsActive = !isOptionsActive;

        if (isOptionsActive)
        {
            float newVolume;
            audioMixer.GetFloat("MasterVolume", out newVolume);
            soundSlider.value = -Mathf.Sqrt(Mathf.Pow(80, 2) - Mathf.Pow(newVolume + 80, 2));
        }

        optionsPanel.SetActive(isOptionsActive);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        levelManager.LoadScene("TitleScreen");
    }

    public void setVolume(float volume)
    {
        volume = Mathf.Sqrt(Mathf.Pow(80, 2) - Mathf.Pow(volume, 2)) - 80;
        audioMixer.SetFloat("MasterVolume", volume);
    }
}
