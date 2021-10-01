using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer SFXMixer;

    public Dropdown resolutionDropdown;
    public Dropdown languageDropdown;

    private Resolution[] resolutions;
    public Locale[] languages;

    private void Start()
    {
        StartResolution();
        StartLanguage();
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", volume);
        Debug.Log(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXMixer.SetFloat("SFXVolume", volume);
        Debug.Log(volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetLanguage(int languageIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];

        Interactable[] interactables =  GameObject.FindObjectsOfType<Interactable>();
        foreach(Interactable i in interactables)
        {
            i.RefreshText();
        }
    }

    private void StartResolution()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> availableResolutions = new List<string>();

        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string availableResolution = resolutions[i].width + " x " + resolutions[i].height;
            availableResolutions.Add(availableResolution);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropdown.AddOptions(availableResolutions);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
    }

    //CHANGE TO BE IN CORRECT LANGUAGE!! 
    private void StartLanguage()
    {
        languageDropdown.ClearOptions();

        List<string> availableLanguages = new List<string>();

        string currentLanguage = System.Globalization.CultureInfo.CurrentCulture.Name;
        Debug.Log("System language: " + currentLanguage);
        int currentLanguageIndex = 0;
        for (int i = 0; i < languages.Length; i++)
        {
            string availableLanguage = languages[i].LocaleName;
            availableLanguages.Add(availableLanguage);

            Debug.Log("Added language: " + availableLanguage);

            if (languages[i].LocaleName == currentLanguage)
            {
                currentLanguageIndex = i;
                Debug.Log("CURRENT LANUAGE WAS FOUND TO BE: " + languages[i].LocaleName);
            }
        }

        languageDropdown.AddOptions(availableLanguages);
        languageDropdown.value = currentLanguageIndex;
        languageDropdown.RefreshShownValue();
    }
}
