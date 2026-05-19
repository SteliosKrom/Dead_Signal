using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudioEvent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    #region SERVICES
    private AudioManager audioManager;
    #endregion

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.PlaySFX(SoundType.Hover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlaySFX(SoundType.Click);
    }
}
