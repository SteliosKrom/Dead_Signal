using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.PlaySFX(SFXType.Hover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlaySFX(SFXType.Click);
    }
}
