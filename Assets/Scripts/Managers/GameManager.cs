using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool canPause;
    private bool hasPressedAnykey;
    private bool isBotMenuPanelOpen;

    private float fps;

    #region SERVICES
    private UIManager uiManager;
    private AudioManager audioManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Shoot shoot;
    [SerializeField] private PlayerSanity playerSanity;
    [SerializeField] private PlayerBotManager playerBotManager;
    [SerializeField] private RandomKeySpawner randomKeySpawner;
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
    [SerializeField] private GameObject zombie;
    [SerializeField] private GameObject ghost;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera menuCamera;
    [SerializeField] private Camera mainCamera;
    #endregion

    #region EVENTS
    [Header("EVENTS")]
    [SerializeField] private GameEvents gameEvents;
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
    public bool IsBotMenuPanelOpen { get => isBotMenuPanelOpen; set => isBotMenuPanelOpen = value; }
    public float FPS => fps;
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<GameManager>(this);
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        gameEvents.OnGameOver += GameOver;

        // Input Events
        controls.Enable();
        controls.UI.AnyKey.performed += OnAnyKeyPressed;
        controls.UI.Back.performed += OnEscapeButtonPressed;
        controls.UI.OpenBotMenu.performed += OnOpenBotMenuButtonPressed;
    }

    private void OnDisable()
    {
        gameEvents.OnGameOver -= GameOver;

        // Input Events
        controls.UI.AnyKey.performed -= OnAnyKeyPressed;
        controls.UI.Back.performed -= OnEscapeButtonPressed;
        controls.UI.OpenBotMenu.performed -= OnOpenBotMenuButtonPressed;
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

    private void Update()
    {
        CalculateFPS();
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
        if (isBotMenuPanelOpen) return;

        if (uiManager.CurrentUIState == UIState.CreditsMenu)
            StartCoroutine(CloseCreditsMenuCoroutine());

        if (OnSettingsTab())
        {
            for (int i = 0; i < uiManager.SettingsTabMenus.Length; i++)
                uiManager.HideObject(uiManager.SettingsTabMenus[i]);

            uiManager.ShowObject(uiManager.SettingsMenu);
            uiManager.CurrentSettingsTab = SettingsTab.None;

            return;
        }
        else if (uiManager.CurrentUIState == UIState.PauseMenuSettings)
        {
            uiManager.HideObject(uiManager.SettingsMenu);
            uiManager.ShowObject(uiManager.PauseMenu);
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
                    audioManager.PauseSound(SoundType.GhostSound);
                    audioManager.PauseSound(SoundType.Chase);
                    audioManager.PauseSound(SoundType.Reload);

                    uiManager.ShowCursor();

                    uiManager.HideObject(uiManager.HUDMenu);
                    uiManager.ShowObject(uiManager.PauseMenu);

                    Time.timeScale = 0f;
                    break;
                case GameState.Paused:
                    currentGameState = GameState.Playing;

                    audioManager.UnPauseSound(SoundType.MainGame);
                    audioManager.UnPauseSound(SoundType.GhostSound);
                    audioManager.UnPauseSound(SoundType.Chase);
                    audioManager.UnPauseSound(SoundType.Reload);

                    uiManager.HideCursor();

                    uiManager.HideObject(uiManager.PauseMenu);
                    uiManager.ShowObject(uiManager.HUDMenu);

                    Time.timeScale = 1f;
                    break;
            }
            StartCoroutine(PauseCoroutine());
        }
    }

    public void OnOpenBotMenuButtonPressed(InputAction.CallbackContext cxt)
    {
        if (CurrentGameState != GameState.Playing)
            return;

        if (playerBotManager.BotSpawned)
            return;

        if (!cxt.performed)
            return;

        isBotMenuPanelOpen = !isBotMenuPanelOpen;
        uiManager.BotMenu.SetActive(isBotMenuPanelOpen);

        if (isBotMenuPanelOpen)
        {
            uiManager.ShowCursor();
            uiManager.HideObject(uiManager.CrossHair);
            uiManager.HideObject(uiManager.InteractIcon);
        }
        else
        {
            uiManager.HideCursor();
        }
    }

    public void SwitchCameras()
    {
        if (!LoadingManager.EnterGameplay)
            return;;

        currentGameState = GameState.Playing;
        gameEvents.RaiseGameplayStarted();

        uiManager.HideCursor();

        menuCamera.enabled = false;
        mainCamera.enabled = true;

        player.SetActive(true);
        randomKeySpawner.EnableKeys();

        uiManager.ShowObject(uiManager.AmmoContent);
        uiManager.ShowObject(uiManager.SanityContent);

        uiManager.HideObject(uiManager.TitleMenu);
        uiManager.ShowObject(uiManager.HUDMenu);

        audioManager.PlaySoundTrack(SoundType.MainGame);
        audioManager.StopSound(SoundType.MainMenu);

        LoadingManager.EnterGameplay = false;
    }

    public float CalculateFPS()
    {
        fps = 1 / Time.unscaledDeltaTime;
        return fps;
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("Demo_scene");
    }

    public void ResumeGame()
    {
        currentGameState = GameState.Playing;

        uiManager.HideCursor();

        Time.timeScale = 1f;

        audioManager.UnPauseSound(SoundType.MainGame);
        audioManager.UnPauseSound(SoundType.GhostSound);

        uiManager.HideObject(uiManager.PauseMenu);
        uiManager.ShowObject(uiManager.HUDMenu);
    }

    public void GameOver()
    {
        currentGameState = GameState.GameOver;
        audioManager.StopSound(SoundType.MainGame);
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void EnterMainMenuSettings() => uiManager.OpenMainMenuSettings();
    public void EnterPauseMenuSettings() => uiManager.OpenPauseMenuSettings();

    public void EnterCredits()
    {
        uiManager.OpenCreditsMenu();
        creditsMenuAnimator.SetTrigger("FadeIn");
    }

    // Settings Tabs
    public void EnterAudio() => uiManager.OpenAudioMenu();
    public void EnterDisplay() => uiManager.OpenDisplayMenu();
    public void EnterGraphics() => uiManager.OpenGraphicsMenu();
    public void EnterControls() => uiManager.OpenControlsMenu();

    // Exit / Navigation / Return
    public void ExitSettingsTabs() => uiManager.ReturnFromSettingsTabs();
    public void ExitSettings() => uiManager.ExitSettings();
    public void ExitGame() => Application.Quit();

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
        uiManager.OpenMainMenu();
    }

    public IEnumerator CloseCreditsMenuCoroutine()
    {
        creditsMenuAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(closeCreditsMenuCoroutine);
        uiManager.OpenMainMenu();
    }
}