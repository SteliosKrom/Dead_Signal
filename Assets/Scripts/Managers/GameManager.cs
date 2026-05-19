using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum GameState
{
    None,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private bool canPause;
    private bool hasPressedAnykey;

    #region SERVICES
    private UIManager uiManager;
    private AudioManager audioManager;
    #endregion

    #region COROUTINES
    private float openMainMenuCoroutine = 0.5f;
    private float closeCreditsMenuCoroutine = 1f;
    private float pauseCoroutine = 0.25f;
    #endregion

    #region INPUT
    private PlayerControls controls;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject player;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera menuCamera;
    [SerializeField] private Camera mainCamera;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private UIEvents uiEvents;
    #endregion

    #region STATES
    [Header("STATES")]
    [SerializeField] private GameState currentGameState;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    [SerializeField] private Animator titleMenuAnimator;
    [SerializeField] private Animator creditsMenuAnimator;
    #endregion

    #region PROPERTIES
    public GameState CurrentGameState { get => currentGameState; set => CurrentGameState = value; }
    #endregion

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.UI.AnyKey.performed += OnAnyKeyPressed;
        controls.UI.Back.performed += OnEscapeButtonPressed;
    }

    private void OnDisable()
    {
        controls.UI.AnyKey.performed -= OnAnyKeyPressed;
        controls.UI.Back.performed -= OnEscapeButtonPressed;
        controls.Disable();
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
        audioManager = ServiceManager.GetService<AudioManager>();

        canPause = true;

        Time.timeScale = 1f;
        player.SetActive(false);

        SwitchCameras();
    }

    public void OnAnyKeyPressed(InputAction.CallbackContext cxt)
    {
        if (uiManager.CurrentUIState == UIState.TitleMenu)
        {
            StartCoroutine(OpenMainMenuCoroutine());
        }
    }

    public void OnEscapeButtonPressed(InputAction.CallbackContext cxt)
    {
        if (uiManager.CurrentUIState == UIState.CreditsMenu)
            StartCoroutine(CloseCreditsMenuCoroutine());

        if (OnSettingsTab())
        {
            for (int i = 0; i < uiManager.SettingsTabMenus.Length; i++)
                uiManager.SettingsTabMenus[i].SetActive(false);

            uiManager.SettingsMenu.SetActive(true);
            uiManager.CurrentSettingsTab = SettingsTab.None;

            return;
        }
        else if (uiManager.CurrentUIState == UIState.PauseMenuSettings)
        {
            uiManager.SettingsMenu.SetActive(false);
            uiManager.PauseMenu.SetActive(true);
            uiManager.CurrentUIState = UIState.PauseMenu;

            return;
        }

        if (canPause)
        {
            switch (currentGameState)
            {
                case GameState.Playing:
                    currentGameState = GameState.Paused;
                    audioManager.PauseSound(SoundType.MainGame);
                    uiManager.PauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    break;
                case GameState.Paused:
                    currentGameState = GameState.Playing;
                    audioManager.UnPauseSound(SoundType.MainGame);
                    uiManager.PauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    break;
            }
            StartCoroutine(PauseCoroutine());
        }
    }

    public void SwitchCameras()
    {
        if (!LoadingManager.EnterGameplay)
            return;

        currentGameState = GameState.Playing;

        menuCamera.enabled = false;
        mainCamera.enabled = true;

        player.SetActive(true);
        uiManager.TitleMenu.SetActive(false);

        audioManager.PlaySoundTrack(SoundType.MainGame);
        audioManager.StopSound(SoundType.MainMenu);

        LoadingManager.EnterGameplay = false;
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("Demo_scene");
    }

    public void ResumeGame()
    {
        currentGameState = GameState.Playing;
        audioManager.UnPauseSound(SoundType.MainGame);
        uiManager.PauseMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void EnterMainMenuSettings()
    {
        uiEvents.RaiseOpenMainMenuSettings();
    }

    public void EnterPauseMenuSettings()
    {
        uiEvents.RaiseOpenPauseMenuSettings();
    }

    public void EnterCredits()
    {
        uiEvents.RaiseOpenCreditsMenu();
        creditsMenuAnimator.SetTrigger("FadeIn");
    }

    // Settings Tabs
    public void EnterAudio()
    {
        uiEvents.RaiseOpenAudioMenu();
    }
    public void EnterDisplay()
    {
        uiEvents.RaiseOpenDisplayMenu();
    }

    public void EnterGraphics()
    {
        uiEvents.RaiseOpenGraphicsMenu();
    }

    public void EnterControls()
    {
        uiEvents.RaiseOpenControlsMenu();
    }

    // Exit / Navigation / Return
    public void ExitSettingsTabs()
    {
        uiEvents.RaiseReturnFromSettingsTabs();
    }

    public void ExitSettings() => uiEvents.RaiseOnExitSettings();

    public void ExitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public bool OnSettingsTab()
    {
        return (uiManager.CurrentSettingsTab == SettingsTab.Audio
            || uiManager.CurrentSettingsTab == SettingsTab.Display
            || uiManager.CurrentSettingsTab == SettingsTab.Graphics
            || uiManager.CurrentSettingsTab == SettingsTab.Controls) && currentGameState == GameState.Paused;
    }

    public IEnumerator PauseCoroutine()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseCoroutine);
        canPause = true;
    }

    public IEnumerator OpenMainMenuCoroutine()
    {
        if (hasPressedAnykey) yield break;

        hasPressedAnykey = true;
        audioManager.PlaySFX(SoundType.PressAnyKey);
        titleMenuAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(openMainMenuCoroutine);
        uiEvents.RaiseOpenMainMenu();
    }

    public IEnumerator CloseCreditsMenuCoroutine()
    {
        creditsMenuAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(closeCreditsMenuCoroutine);
        uiEvents.RaiseOpenMainMenu();
    }
}