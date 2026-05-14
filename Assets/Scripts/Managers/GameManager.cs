using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private bool hasPressedAnykey;

    #region SERVICES
    private UIManager uiManager;
    private AudioManager audioManager;
    #endregion

    #region COROUTINES
    private float openMainMenuDelay = 0.5f;
    private float closeCreditsMenuDelay = 1f;
    #endregion

    #region INPUT
    private PlayerControls controls;
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

        currentGameState = GameState.None;
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
        {
            StartCoroutine(CloseCreditsMenuCoroutine());
        }
    }

    public void EnterGame()
    {
        // Start game here...
    }

    // Enter Main Menus
    public void EnterSettings()
    {
        uiEvents.RaiseOpenSettingsMenu();
    }
    public void EnterCredits()
    {
        uiEvents.RaiseOpenCreditsMenu();
        creditsMenuAnimator.SetTrigger("FadeIn");
    }

    // Enter Settings Tabs
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
        EditorApplication.ExitPlaymode();
        Application.Quit();
    }

    public IEnumerator OpenMainMenuCoroutine()
    {
        if (hasPressedAnykey) yield break;

        hasPressedAnykey = true;
        audioManager.PlaySFX(SFXType.PressAnyKey);
        titleMenuAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(openMainMenuDelay);
        uiEvents.RaiseOpenMainMenu();
    }

    public IEnumerator CloseCreditsMenuCoroutine()
    {
        creditsMenuAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(closeCreditsMenuDelay);
        uiEvents.RaiseOpenMainMenu();
    }
}