using System.Collections.Generic;
using UnityEngine;

public enum SFXType { Hover, Click, PressAnyKey }

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct AudioItem
    {
        public SFXType type;
        public AudioSource source;
        public AudioClip clip;
    }

    #region AUDIO
    [Header("SFX")]
    [SerializeField] private AudioItem[] audioItems;
    private Dictionary<SFXType, AudioItem> sfxDict;

    [Header("SOUNDTRACKS")]
    [SerializeField] private AudioSource mainMenuAudioSource;
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<AudioManager>(this);

        sfxDict = new Dictionary<SFXType, AudioItem>();

        foreach (AudioItem item in audioItems)
        {
            sfxDict.Add(item.type, item);
        }
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxDict.TryGetValue(type, out AudioItem item))
        {
            item.source.PlayOneShot(item.clip);
        }
    }

    public void PlaySound(AudioSource source)
    {
        source.Play();
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
