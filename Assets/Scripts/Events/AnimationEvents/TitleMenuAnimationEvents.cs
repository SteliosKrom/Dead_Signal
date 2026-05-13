using UnityEngine;

public class TitleMenuAnimationEvents : MonoBehaviour
{
    public void OnTitleMenuFadeInCompleted()
    {
        UIManager.Instance.TitleMenuFadeInCompleted();
    }
}
