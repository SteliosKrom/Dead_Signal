using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public event Action OnTitleMenuFadeInCompleted;
    public event Action OnCreditsMenuFadeInCompleted;

    public event Action OnOpenMainMenu;
    public event Action OnOpenSettingsMenu;
    public event Action OnOpenCreditsMenu;
    public event Action OnOpenAudioMenu;
    public event Action OnOpenDisplayMenu;
    public event Action OnOpenGraphicsMenu;
    public event Action OnOpenControlsMenu;

    public event Action OnReturnFromSettingsTabs;
    public event Action OnReturnFromCreditsMenu;
    public event Action OnExitSettings;

    // Animations 
    public void RaiseTitleMenuFadeInCompleted() => OnTitleMenuFadeInCompleted?.Invoke();
    public void RaiseCreditsMenuFadeInCompleted() => OnCreditsMenuFadeInCompleted?.Invoke();

    // Main Menus
    public void RaiseOpenMainMenu() => OnOpenMainMenu?.Invoke();
    public void RaiseOpenSettingsMenu() => OnOpenSettingsMenu?.Invoke();
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
