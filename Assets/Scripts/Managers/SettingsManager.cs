using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region SERVICES
    private UIManager uiManager;
    #endregion

    #region CONSTANTS
    private const string MASTER_VOL = "MasterVol";
    private const string GAME_VOL = "GameVol";
    private const string MENU_VOL = "MenuVol";
    private const string SFX_VOL = "SFXVol";
    #endregion

    #region AUDIO PLAYER PREFS
    private const string MASTER_VOL_KEY = "MasterVolume";
    private const string GAME_VOL_KEY = "GameVolume";
    private const string MENU_VOL_KEY = "MenuVolume";
    private const string SFX_VOL_KEY = "SFXVolume";
    #endregion

    #region DISPLAY PLAYER PREFS
    private const string DISPLAY_MODE_KEY = "DisplayMode";
    private const string RESOLUTION_MODE_KEY = "ResolutionMode";
    private const string SENSITIVITY_KEY = "Sensitivity";
    private const string FPS_KEY = "FPSKey";
    #endregion

    #region GRAPHICS PLAYER PREFS
    private const string QUALITY_LEVEL_KEY = "QualityLevel";
    private const string VSYNC_COUNT = "VSyncCount";
    #endregion

    #region DISPLAY MODE
    private int currentScreenWidth;
    private int currentScreenHeight;

    private int windowedScreenWidth = 1280;
    private int windowedScreenHeight = 720;

    private const float sensMultiplier = 10f;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private CameraController cameraController;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private UIEvents uiEvents;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioMixer audioMixer;
    #endregion

    #region UI
    [Header("SLIDERS")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameVolumeSlider;
    [SerializeField] private Slider menuVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI sensitivitySliderText;

    [Header("DROPDOWNS")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionModeDropdown;
    [SerializeField] private TMP_Dropdown qualityLevelDropdown;

    [Header("TOGGLE")]
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Toggle fpsToggle;
    #endregion

    #region PROPERTIES
    public Slider SensitivitySlider => sensitivitySlider;
    public AudioMixer AudioMixer => audioMixer;
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<SettingsManager>(this);
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
        LoadSettings();

        currentScreenWidth = Screen.currentResolution.width;
        currentScreenHeight = Screen.currentResolution.height;
    }

    public void LoadSettings()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat(MASTER_VOL_KEY, 1f);
        float savedGameVolume = PlayerPrefs.GetFloat(GAME_VOL_KEY, 0.8f);
        float savedMenuVolume = PlayerPrefs.GetFloat(MENU_VOL_KEY, 0.6f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY, 0.9f);

        float savedSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_KEY, 1f);

        int savedDisplayMode = PlayerPrefs.GetInt(DISPLAY_MODE_KEY, 0);
        int savedResolutionMode = PlayerPrefs.GetInt(RESOLUTION_MODE_KEY, 0);

        int savedQualityLevel = PlayerPrefs.GetInt(QUALITY_LEVEL_KEY, 1);

        bool savedVSync = GetBool(VSYNC_COUNT, false);
        bool savedFPS = GetBool(FPS_KEY, false);

        // APPLY AUDIO
        masterVolumeSlider.value = savedMasterVolume;
        gameVolumeSlider.value = savedGameVolume;
        menuVolumeSlider.value = savedMenuVolume;
        sfxVolumeSlider.value = savedSFXVolume;

        // APPLY DISPLAY
        displayModeDropdown.value = savedDisplayMode;
        resolutionModeDropdown.value = savedResolutionMode;
        SensitivitySlider.value = savedSensitivity;
        fpsToggle.isOn = savedFPS;

        // APPLY GRAPHICS
        qualityLevelDropdown.value = savedQualityLevel;
        vSyncToggle.isOn = savedVSync;
    }

    public void AdjustMasterVolume()
    {
        float masterVolume = masterVolumeSlider.value;
        float dB;

        if (masterVolume <= 0.0001)
            dB = -80f;
        else
            dB = Mathf.Log10(masterVolume) * 20f;

        audioMixer.SetFloat(MASTER_VOL, dB);
        PlayerPrefs.SetFloat(MASTER_VOL_KEY, masterVolume);
        uiEvents.RaiseUpdateSliderValue(masterVolumeSlider, uiManager.MasterVolumeText);
    }

    public void AdjustGameVolume()
    {
        float gameVolume = gameVolumeSlider.value;
        float dB;

        if (gameVolume <= 0.0001)
            dB = -80f;
        else
            dB = Mathf.Log10(gameVolume) * 20f;

        audioMixer.SetFloat(GAME_VOL, dB);
        PlayerPrefs.SetFloat(GAME_VOL_KEY, gameVolume);
        uiEvents.RaiseUpdateSliderValue(gameVolumeSlider, uiManager.GameVolumeText);
    }

    public void AdjustMenuVolume()
    {
        float menuVolume = menuVolumeSlider.value;
        float dB;

        if (menuVolume <= 0.0001)
            dB = -80f;
        else
            dB = Mathf.Log10(menuVolume) * 20;

        audioMixer.SetFloat(MENU_VOL, dB);
        PlayerPrefs.SetFloat(MENU_VOL_KEY, menuVolume);
        uiEvents.RaiseUpdateSliderValue(menuVolumeSlider, uiManager.MenuVolumeText);
    }

    public void AdjustSFXVolume()
    {
        float sfxVolume = sfxVolumeSlider.value;
        float dB;

        if (sfxVolume <= 0.0001)
            dB = -80f;
        else
            dB = Mathf.Log10(sfxVolume) * 20;

        audioMixer.SetFloat(SFX_VOL, dB);
        PlayerPrefs.SetFloat(SFX_VOL_KEY, sfxVolume);
        uiEvents.RaiseUpdateSliderValue(sfxVolumeSlider, uiManager.SFXVolumeText);
    }

    public void AdjustSensitivity()
    {
        cameraController.MouseSensitivity = SensitivitySlider.value * sensMultiplier;
        sensitivitySliderText.text = SensitivitySlider.value.ToString("0%");
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, SensitivitySlider.value);
    }

    public void ChooseDisplayMode()
    {
        switch (displayModeDropdown.value)
        {
            case 0:
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.ExclusiveFullScreen);
                break;
            case 1:
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.MaximizedWindow);
                break;
            case 2:
                Screen.SetResolution(windowedScreenWidth, windowedScreenHeight, FullScreenMode.Windowed);
                break;
        }
        PlayerPrefs.SetInt(DISPLAY_MODE_KEY, displayModeDropdown.value);
    }

    public void ChooseResolutionMode()
    {
        switch (resolutionModeDropdown.value)
        {
            case 0:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
        }
        PlayerPrefs.SetInt(RESOLUTION_MODE_KEY, resolutionModeDropdown.value);
    }

    public void ChooseQualityLevel()
    {
        switch (qualityLevelDropdown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
        }
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, qualityLevelDropdown.value);
    }

    public void SetFPSToggle()
    {
        if (fpsToggle.isOn)
            uiManager.ShowObject(uiManager.FPSMenu);
        else
            uiManager.HideObject(uiManager.FPSMenu);

        SetBool(FPS_KEY, fpsToggle.isOn);
    }

    public void SetVSync()
    {
        if (vSyncToggle.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
        SetBool(VSYNC_COUNT, vSyncToggle.isOn);
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }
}
