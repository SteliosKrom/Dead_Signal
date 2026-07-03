using UnityEngine;

public class PlayerBotManager : MonoBehaviour
{
    #region SERVICES
    private UIManager uiManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerBot bot;
    #endregion

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
    }

    public void ChooseBotRole(int roleIndex)
    {
        switch (roleIndex)
        {
            case 1:
                bot.CurrentRole = BotRole.Soldier;
                uiManager.CloseBotMenu();
                uiManager.HideCursor();
                bot.gameObject.SetActive(true);
                break;
            case 2:
                bot.CurrentRole = BotRole.Explorer;
                uiManager.CloseBotMenu();
                uiManager.HideCursor();
                bot.gameObject.SetActive(true);
                break;
            case 3:
                bot.CurrentRole = BotRole.Bodyguard;
                uiManager.CloseBotMenu();
                uiManager.HideCursor();
                bot.gameObject.SetActive(true);
                break;
            case 4:
                bot.CurrentRole = BotRole.Guard;
                uiManager.CloseBotMenu();
                uiManager.HideCursor();
                bot.gameObject.SetActive(true);
                break;
        }
    }
}
