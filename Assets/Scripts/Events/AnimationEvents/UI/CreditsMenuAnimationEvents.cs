using UnityEngine;

public class CreditsMenuAnimationEvents : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
    }

    public void OnCreditsMenuFadeInCompleted()
    {
        uiManager.CreditsMenuFadeInCompleted();
    }
}
