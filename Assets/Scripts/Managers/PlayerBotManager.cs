using UnityEngine;

public class PlayerBotManager : MonoBehaviour
{
    private bool botSpawned;

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
                soldierBot.CurrentRole = BotRole.Soldier;

                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(soldierBot);

                // Event OnSolderBotSpawned...
                soldierBot.InitialRandomWaitTime = Random.Range(2f, 5f); // Replace with an event...
                StartCoroutine(soldierBot.SelectNewPatrolPointCoroutine()); // Replace with an event...
                break;
            case 2:
                explorerBot.CurrentRole = BotRole.Explorer;

                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(explorerBot);
                break;
            case 3:
                bodyguardBot.CurrentRole = BotRole.Bodyguard;

                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(bodyguardBot);
                break;
            case 4:
                guardBot.CurrentRole = BotRole.Guard;

                uiManager.CloseBotMenu();
                uiManager.HideCursor();

                SpawnBot(guardBot);
                break;
        }
    }

    public void SpawnBot(PlayerBot bot)
    {
        bot.gameObject.SetActive(true);
        botSpawned = true;
    }
}
