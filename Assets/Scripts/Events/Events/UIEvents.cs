using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public event Action OnTitleMenuFadeInCompleted;
    public event Action OnCreditsMenuFadeInCompleted;

    public event Action<Slider, TextMeshProUGUI> OnUpdateSliderValue;
    public event Action OnDisplayGameOverMenu;

    // UI
    public void RaiseUpdateSliderValue(Slider slider, TextMeshProUGUI valueText) => OnUpdateSliderValue?.Invoke(slider, valueText);
    public void RaiseDisplayGameOverMenu() => OnDisplayGameOverMenu?.Invoke();

    // Animations 
    public void RaiseTitleMenuFadeInCompleted() => OnTitleMenuFadeInCompleted?.Invoke();
    public void RaiseCreditsMenuFadeInCompleted() => OnCreditsMenuFadeInCompleted?.Invoke();
}
