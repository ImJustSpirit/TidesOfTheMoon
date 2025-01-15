using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_UIController : MonoBehaviour
{
    private enum MenuState { None, PauseMenu, OptionsMenu }
    private MenuState currentMenuState;

    public GameObject PauseMenu;
    public GameObject OptionsMenu;

    private void Start()
    {
        SwitchMenuState(MenuState.None);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchMenuState(MenuState.PauseMenu);
        }
    }

    public void OnPlayButtonClicked()
    {
        SwitchMenuState(MenuState.None);
    }
    public void OptionsButtonClicked()
    {
        SwitchMenuState(MenuState.OptionsMenu);
    }
    public void OnQuitToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnBackButtonClicked()
    {
        SwitchMenuState(MenuState.PauseMenu);
    }

    private void SwitchMenuState(MenuState newState)
    {
        PauseMenu.SetActive(newState == MenuState.PauseMenu);
        OptionsMenu.SetActive(newState == MenuState.OptionsMenu);

        if (newState == MenuState.None)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }
}
