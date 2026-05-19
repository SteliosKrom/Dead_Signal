using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public event Action OnTitleMenuFadeInCompleted;
    public event Action OnCreditsMenuFadeInCompleted;

    #region OPEN MENUS ACTIONS
    public event Action OnOpenMainMenu;
    public event Action OnOpenMainMenuSettings;
    public event Action OnOpenPauseMenuSettings;
    public event Action OnOpenCreditsMenu;
    #endregion

    #region OPEN SETTINGS TAB MENUS ACTIONS
    public event Action OnOpenAudioMenu;
    public event Action OnOpenDisplayMenu;
    public event Action OnOpenGraphicsMenu;
    public event Action OnOpenControlsMenu;
    #endregion

    #region RETURN / EXIT
    public event Action OnReturnFromSettingsTabs;
    public event Action OnReturnFromCreditsMenu;
    public event Action OnExitSettings;
    #endregion

    public event Action<Slider, TextMeshProUGUI> OnUpdateSliderValue;

    // UI
    public void RaiseUpdateSliderValue(Slider slider, TextMeshProUGUI valueText) => OnUpdateSliderValue?.Invoke(slider, valueText);

    // Animations 
    public void RaiseTitleMenuFadeInCompleted() => OnTitleMenuFadeInCompleted?.Invoke();
    public void RaiseCreditsMenuFadeInCompleted() => OnCreditsMenuFadeInCompleted?.Invoke();

    // Main Menus
    public void RaiseOpenMainMenu() => OnOpenMainMenu?.Invoke();
    public void RaiseOpenMainMenuSettings() => OnOpenMainMenuSettings?.Invoke();
    public void RaiseOpenPauseMenuSettings() => OnOpenPauseMenuSettings?.Invoke();
    public void RaiseOpenCreditsMenu() => OnOpenCreditsMenu?.Invoke();

    // Settings Tabs
    public void RaiseOpenAudioMenu() => OnOpenAudioMenu?.Invoke();
    public void RaiseOpenDisplayMenu() => OnOpenDisplayMenu?.Invoke();
    public void RaiseOpenGraphicsMenu() => OnOpenGraphicsMenu?.Invoke();
    public void RaiseOpenControlsMenu() => OnOpenControlsMenu?.Invoke();

    // Exit / Navigation / Return
    public void RaiseReturnFromSettingsTabs() => OnReturnFromSettingsTabs?.Invoke();
    public void RaiseReturnFromCreditsMenu() => OnReturnFromCreditsMenu?.Invoke();
    public void RaiseOnExitSettings() => OnExitSettings?.Invoke();
}
