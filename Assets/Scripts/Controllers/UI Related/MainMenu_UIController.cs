#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_UIController : MonoBehaviour
{
    private enum MenuState { None, MainMenu, LevelSelectionMenu, OptionsMenu }
    private MenuState currentMenuState;

    public GameObject MainMenu;
    public GameObject LevelSelectionMenu;
    public GameObject OptionsMenu;
    
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        SwitchMenuState(MenuState.MainMenu);
    }

    public void OnPlayButtonClicked()
    {
        SwitchMenuState(MenuState.LevelSelectionMenu);
    }
    public void OptionsButtonClicked()
    {
        SwitchMenuState(MenuState.OptionsMenu);
        musicVolumeSlider.value = AudioManager.Instance.musicSource.volume;
        sfxVolumeSlider.value = AudioManager.Instance.sfxSource.volume;
    }
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnBackButtonClicked()
    {
        SwitchMenuState(MenuState.MainMenu);
    }
    public void OnLevelSelectionButtonClicked(string Level)
    {
        SceneManager.LoadScene(Level);
    }
    
    public void OnMusicVolumeSliderChanged()
    {
        AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);
    }
    public void OnSFXVolumeSliderChanged()
    {
        AudioManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
    }

    private void SwitchMenuState(MenuState newState)
    {
        MainMenu.SetActive(newState == MenuState.MainMenu);
        LevelSelectionMenu.SetActive(newState == MenuState.LevelSelectionMenu);
        OptionsMenu.SetActive(newState == MenuState.OptionsMenu);
    }
}
