using UnityEngine;

public class PlayerBotManager : MonoBehaviour
{
    private bool botSpawned;
    private PlayerBot currentBot;

    #region SERVICES
    private UIManager uiManager;
    private GameManager gameManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private SoldierBot soldierBot;
    [SerializeField] private ExplorerBot explorerBot;
    [SerializeField] private BodyguardBot bodyguardBot;
    [SerializeField] private GuardBot guardBot;
    #endregion

    #region PROPERTIES
    public bool BotSpawned { get => botSpawned; set => botSpawned = value; }
    #endregion
    private void Awake()
    {
        ServiceManager.RegisterService<PlayerBotManager>(this);
    }

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
        gameManager = ServiceManager.GetService<GameManager>();
    }

    private void Update()
    {
        if (gameManager.CurrentGameState != GameState.Playing)
            return;
    }

    public void ChooseBotRole(int roleIndex)
    {
        switch (roleIndex)
        {
            case 1:
                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(soldierBot);
                soldierBot.InitializeBot();
                break;
            case 2:
                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(explorerBot);
                explorerBot.InitializeBot();
                break;
            case 3:
                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(bodyguardBot);
                bodyguardBot.InitializeBot();
                break;
            case 4:
                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(guardBot);
                guardBot.InitializeBot();
                break;
        }
    }

    public void SpawnBot(PlayerBot bot)
    {
        if (currentBot)
        {
            bot.transform.position = currentBot.transform.position;
            bot.transform.rotation = currentBot.transform.rotation;
            DespawnBot(currentBot);
        }

        bot.gameObject.SetActive(true);
        currentBot = bot;
        botSpawned = true;
    }

    public void DespawnBot(PlayerBot bot)
    {
        bot.gameObject.SetActive(false);
        botSpawned = false;
    }
}
