using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    None,
    TitleMenu,
    MainMenu,
    PauseMenu,
    CreditsMenu,
    MainMenuSettings,
    PauseMenuSettings,
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
    #region SERVICES
    private UIManager uiManager;
    private SettingsManager settingsManager;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private UIEvents uiEvents;
    #endregion

    #region STATES
    [Header("STATES")]
    [SerializeField] private UIState currentUIState;
    [SerializeField] private SettingsTab currentSettingsTab;
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI gameVolumeText;
    [SerializeField] private TextMeshProUGUI menuVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
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
    [SerializeField] private GameObject[] settingsTabMenus;
    #endregion

    #region PROPERTIES
    public GameObject[] SettingsTabMenus => settingsTabMenus;
    public GameObject PauseMenu => pauseMenu;
    public GameObject TitleMenu => titleMenu;
    public GameObject SettingsMenu => settingsMenu;
    public TextMeshProUGUI MasterVolumeText => masterVolumeText;
    public TextMeshProUGUI GameVolumeText => gameVolumeText;
    public TextMeshProUGUI MenuVolumeText => menuVolumeText;
    public TextMeshProUGUI SFXVolumeText => sfxVolumeText;
    public UIState CurrentUIState { get => currentUIState; set => currentUIState = value; }
    public SettingsTab CurrentSettingsTab { get => currentSettingsTab; set => currentSettingsTab = value; }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<UIManager>(this);
    }

    private void OnEnable()
    {
        // UI
        uiEvents.OnUpdateSliderValue += UpdateSliderValueUI;

        // Animations
        uiEvents.OnTitleMenuFadeInCompleted += TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted += CreditsMenuFadeInCompleted;

        // Main Menus
        uiEvents.OnOpenMainMenu += OpenMainMenu;
        uiEvents.OnOpenMainMenuSettings += OpenMainMenuSettings;
        uiEvents.OnOpenPauseMenuSettings += OpenPauseMenuSettings;
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
        // UI
        uiEvents.OnUpdateSliderValue -= UpdateSliderValueUI;

        // Animations
        uiEvents.OnTitleMenuFadeInCompleted -= TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted -= CreditsMenuFadeInCompleted;

        // Main Menus
        uiEvents.OnOpenMainMenu -= OpenMainMenu;
        uiEvents.OnOpenMainMenuSettings -= OpenMainMenuSettings;
        uiEvents.OnOpenPauseMenuSettings -= OpenPauseMenuSettings;
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
        settingsManager = ServiceManager.GetService<SettingsManager>();

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

    // Generic methods
    public void OpenSettingsMenu(GameObject targetMenu, UIState state)
    {
        currentUIState = state;
        targetMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenSettingsTab(GameObject targetMenu, SettingsTab tab)
    {
        currentSettingsTab = tab;
        settingsMenu.SetActive(false);
        targetMenu.SetActive(true);
    }

    // Open menus methods 
    public void OpenMainMenu()
    {
        currentUIState = UIState.MainMenu;
        titleMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenMainMenuSettings()
    {
        OpenSettingsMenu(mainMenu, UIState.MainMenuSettings);
    }

    public void OpenPauseMenuSettings()
    {
        OpenSettingsMenu(pauseMenu, UIState.PauseMenuSettings);
    }

    public void OpenCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OpenAudioMenu()
    {
        OpenSettingsTab(audioMenu, SettingsTab.Audio);
    }

    public void OpenDisplayMenu()
    {
        OpenSettingsTab(displayMenu, SettingsTab.Display);
    }

    public void OpenGraphicsMenu()
    {
        OpenSettingsTab(graphicsMenu, SettingsTab.Graphics);
    }

    public void OpenControlsMenu()
    {
        OpenSettingsTab(controlsMenu, SettingsTab.Controls);
    }

    public void UpdateSliderValueUI(Slider slider, TextMeshProUGUI valueText)
    {
        valueText.text = slider.value.ToString("0%");
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
        PlayerPrefs.Save();
    }
}
