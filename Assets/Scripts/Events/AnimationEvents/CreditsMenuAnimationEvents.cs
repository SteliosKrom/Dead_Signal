using UnityEngine;

public class CreditsMenuAnimationEvents : MonoBehaviour
{
    public void OnCreditsMenuFadeInCompleted()
    {
        UIManager.Instance.CreditsMenuFadeInCompleted();
    }
}
