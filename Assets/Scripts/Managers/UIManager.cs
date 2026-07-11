using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private float elapsedTime;
    private float duration = 1;

    #region SERVICES
    private GameManager gameManager;
    private PlayerSanity playerSanity;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Shoot shoot;
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
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI currentReserveAmmoText;
    [SerializeField] private TextMeshProUGUI currentSanityCounterText;
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

    [SerializeField] private GameObject HUDmenu;
    [SerializeField] private GameObject ammoContent;
    [SerializeField] private GameObject sanityContent;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject fpsMenu;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject botMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject[] settingsTabMenus;
    #endregion

    #region PROPERTIES
    public GameObject[] SettingsTabMenus => settingsTabMenus;
    public GameObject InteractIcon => interactIcon;
    public GameObject AmmoContent => ammoContent;
    public GameObject SanityContent => sanityContent;
    public GameObject CrossHair => crossHair;
    public GameObject FPSMenu => fpsMenu;
    public GameObject PauseMenu => pauseMenu;
    public GameObject TitleMenu => titleMenu;
    public GameObject SettingsMenu => settingsMenu;
    public GameObject HUDMenu { get => HUDmenu; set => HUDmenu = value; }
    public GameObject BotMenu { get => botMenu; set => botMenu = value; }

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
        uiEvents.OnDisplayGameOverMenu += DisplayGameOverMenu;

        // Animations
        uiEvents.OnTitleMenuFadeInCompleted += TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted += CreditsMenuFadeInCompleted;
    }

    private void OnDisable()
    {
        // UI
        uiEvents.OnUpdateSliderValue -= UpdateSliderValueUI;
        uiEvents.OnDisplayGameOverMenu -= DisplayGameOverMenu;

        // Animations
        uiEvents.OnTitleMenuFadeInCompleted -= TitleMenuFadeInCompleted;
        uiEvents.OnCreditsMenuFadeInCompleted -= CreditsMenuFadeInCompleted;
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        playerSanity = ServiceManager.GetService<PlayerSanity>();

        currentSettingsTab = SettingsTab.None;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            elapsedTime = 0f;
            UpdateFPSUI();
            return;
        }
    }

    public void TitleMenuFadeInCompleted() => currentUIState = UIState.TitleMenu;
    public void CreditsMenuFadeInCompleted() => currentUIState = UIState.CreditsMenu;

    // Generic methods
    public void OpenSettingsMenu(GameObject targetMenu, UIState state)
    {
        currentUIState = state;
        HideObject(targetMenu);
        ShowObject(settingsMenu);
    }

    public void OpenSettingsTab(GameObject targetMenu, SettingsTab tab)
    {
        currentSettingsTab = tab;
        HideObject(settingsMenu);
        ShowObject(targetMenu);
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowObject(GameObject obj) => obj.SetActive(true);
    public void HideObject(GameObject obj) => obj.SetActive(false);

    public void UpdateFPSUI()
    {
        float fps = gameManager.CalculateFPS();
        fpsText.text = fps.ToString("0");
    }

    public void UpdateCurrentAmmoUI() => currentAmmoText.text = shoot.CurrentAmmo.ToString("0");
    public void UpdateSanityCounterUI() => currentSanityCounterText.text = playerSanity.Sanity.ToString("0");

    public void UpdateFullAmmoCapacityUI()
    {
        currentAmmoText.text = shoot.CurrentAmmo.ToString("0");
        currentReserveAmmoText.text = shoot.CurrentReserveAmmo.ToString("0");
    }

    // Open menus methods 
    public void OpenMainMenu()
    {
        currentUIState = UIState.MainMenu;
        HideObject(titleMenu);
        ShowObject(mainMenu);
    }

    public void DisplayGameOverMenu()
    {
        ShowObject(gameOverMenu);
        ShowCursor();

        HideObject(crossHair);
        HideObject(sanityContent);
        HideObject(ammoContent);
    }

    public void CloseBotMenu()
    {
        HideObject(botMenu);
        gameManager.IsBotMenuPanelOpen = false;
    }

    public void OpenMainMenuSettings() => OpenSettingsMenu(mainMenu, UIState.MainMenuSettings);
    public void OpenPauseMenuSettings() => OpenSettingsMenu(pauseMenu, UIState.PauseMenuSettings);

    // Open settings tab menus
    public void OpenAudioMenu() => OpenSettingsTab(audioMenu, SettingsTab.Audio);
    public void OpenDisplayMenu() => OpenSettingsTab(displayMenu, SettingsTab.Display);
    public void OpenGraphicsMenu() => OpenSettingsTab(graphicsMenu, SettingsTab.Graphics);
    public void OpenControlsMenu() => OpenSettingsTab(controlsMenu, SettingsTab.Controls);

    public void OpenCreditsMenu()
    {
        HideObject(mainMenu);
        ShowObject(creditsMenu);
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
                HideObject(audioMenu);
                ShowObject(settingsMenu);
                break;
            case SettingsTab.Display:
                HideObject(displayMenu);
                ShowObject(settingsMenu);
                break;
            case SettingsTab.Graphics:
                HideObject(graphicsMenu);
                ShowObject(settingsMenu);
                break;
            case SettingsTab.Controls:
                HideObject(controlsMenu);
                ShowObject(settingsMenu);
                break;
        }
    }

    public void ReturnFromCreditsMenu()
    {
        currentUIState = UIState.MainMenu;
        ShowObject(mainMenu);
        HideObject(creditsMenu);
    }

    public void ExitSettings()
    {
        switch (currentUIState)
        {
            case UIState.MainMenuSettings:
                currentUIState = UIState.MainMenu;
                HideObject(settingsMenu);
                ShowObject(mainMenu);
                break;
            case UIState.PauseMenuSettings:
                currentUIState = UIState.PauseMenu;
                HideObject(settingsMenu);
                ShowObject(pauseMenu);
                break;
        }
        PlayerPrefs.Save();
    }
}
