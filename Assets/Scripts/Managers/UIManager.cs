using UnityEngine;

public enum UIState
{
    None,
    TitleMenu,
    MainMenu,
    PauseMenu,
    MainMenuSettings,
    PauseMenuSettings,
    CreditsMenu
}

public enum SettingsTab
{
    None,
    Audio,
    Display,
    Graphics,
    Controls
}

public class UIManager : MonoBehaviour
{
    private UIManager uiManager;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private UIEvents uiEvents;
    #endregion

    #region STATES
    [Header("STATES")]
    [SerializeField] private UIState currentUIState;
    [SerializeField] private SettingsTab currentSettingsTab;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject controlsMenu;
    #endregion

    #region PROPERTIES
    public UIState CurrentUIState { get => currentUIState; set => currentUIState = value; }
    public SettingsTab CurrentSettingsTab { get => currentSettingsTab; set => currentSettingsTab = value; }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<UIManager>(this);
    }

    private void OnEnable()
    {
        // Animations
        uiEvents.OnTitleMenuFadeInCompleted += TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted += CreditsMenuFadeInCompleted;

        // Main Menus
        uiEvents.OnOpenMainMenu += OpenMainMenu;
        uiEvents.OnOpenSettingsMenu += OpenSettingsMenu;
        uiEvents.OnOpenCreditsMenu += OpenCreditsMenu;

        // Settings Tabs
        uiEvents.OnOpenAudioMenu += OpenAudioMenu;
        uiEvents.OnOpenDisplayMenu += OpenDisplayMenu;
        uiEvents.OnOpenGraphicsMenu += OpenGraphicsMenu;
        uiEvents.OnOpenControlsMenu += OpenControlsMenu;

        // Exit / Navigation / Return
        uiEvents.OnReturnFromSettingsTabs += ReturnFromSettingsTabs;
        uiEvents.OnReturnFromCreditsMenu += ReturnFromCreditsMenu;
        uiEvents.OnExitSettings += ExitSettings;
    }

    private void OnDisable()
    {
        // Animations
        uiEvents.OnTitleMenuFadeInCompleted -= TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted -= CreditsMenuFadeInCompleted;

        // Main Menus
        uiEvents.OnOpenMainMenu -= OpenMainMenu;
        uiEvents.OnOpenSettingsMenu -= OpenSettingsMenu;
        uiEvents.OnOpenCreditsMenu -= OpenCreditsMenu;

        // Settings Tabs
        uiEvents.OnOpenAudioMenu -= OpenAudioMenu;
        uiEvents.OnOpenDisplayMenu -= OpenDisplayMenu;
        uiEvents.OnOpenGraphicsMenu -= OpenGraphicsMenu;
        uiEvents.OnOpenControlsMenu -= OpenControlsMenu;

        // Exit / Navigation / Return
        uiEvents.OnReturnFromSettingsTabs -= ReturnFromSettingsTabs;
        uiEvents.OnReturnFromCreditsMenu -= ReturnFromCreditsMenu;
        uiEvents.OnExitSettings -= ExitSettings;
    }

    private void Start()
    {
        currentUIState = UIState.None;
        currentSettingsTab = SettingsTab.None;
    }

    public void TitleMenuFadeInCompleted()
    {
        currentUIState = UIState.TitleMenu;
    }

    public void CreditsMenuFadeInCompleted()
    {
        currentUIState = UIState.CreditsMenu;
    }

    public void OpenMainMenu()
    {
        currentUIState = UIState.MainMenu;
        titleMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        currentUIState = UIState.MainMenuSettings;
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OpenAudioMenu()
    {
        currentSettingsTab = SettingsTab.Audio;
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void OpenDisplayMenu()
    {
        currentSettingsTab = SettingsTab.Display;
        settingsMenu.SetActive(false);
        displayMenu.SetActive(true);
    }

    public void OpenGraphicsMenu()
    {
        currentSettingsTab = SettingsTab.Graphics;
        settingsMenu.SetActive(false);
        graphicsMenu.SetActive(true);
    }

    public void OpenControlsMenu()
    {
        currentSettingsTab = SettingsTab.Controls;
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void ReturnFromSettingsTabs()
    {
        switch (currentSettingsTab)
        {
            case SettingsTab.Audio:
                audioMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
            case SettingsTab.Display:
                displayMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
            case SettingsTab.Graphics:
                graphicsMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
            case SettingsTab.Controls:
                controlsMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
        }
    }

    public void ReturnFromCreditsMenu()
    {
        currentUIState = UIState.MainMenu;
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void ExitSettings()
    {
        switch (currentUIState)
        {
            case UIState.MainMenuSettings:
                currentUIState = UIState.MainMenu;
                settingsMenu.SetActive(false);
                mainMenu.SetActive(true);
                break;
            case UIState.PauseMenuSettings:
                currentUIState = UIState.PauseMenu;
                settingsMenu.SetActive(false);
                pauseMenu.SetActive(true);
                break;
        }
    }
}
