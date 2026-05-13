using UnityEngine;

public class TitleMenuAnimationEvents : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();
    }

    public void OnTitleMenuFadeInCompleted()
    {
        uiManager.TitleMenuFadeInCompleted();
    }
}
