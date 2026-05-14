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
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<SettingsManager>(this);
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
    }

    public void LoadSettings()
    {
        // 
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
        uiEvents.RaiseUpdateSliderValue(sfxVolumeSlider, uiManager.SFXVolumeText);
    }
}
