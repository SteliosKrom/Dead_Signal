using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropdownAudioEvent : MonoBehaviour, IPointerClickHandler
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.PlaySFX(SoundType.Click);
    }
}
