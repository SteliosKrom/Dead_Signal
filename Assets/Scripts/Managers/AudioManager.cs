using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct AudioItem
    {
        public SoundType type;
        public AudioSource source;
        public AudioClip clip;
    }

    #region DATA
    [Header("DATA")]
    [SerializeField] private AudioItem[] audioItems;
    private Dictionary<SoundType, AudioItem> soundDict;
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<AudioManager>(this);

        soundDict = new Dictionary<SoundType, AudioItem>();

        foreach (AudioItem item in audioItems)
        {
            soundDict.Add(item.type, item);
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioItem item))
        {
            item.source.PlayOneShot(item.clip);
        }
    }

    public void PlaySoundTrack(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioItem item))
        {
            item.source.Play();
        }
    }

    public void StopSound(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioItem item))
        {
            item.source.Stop();
        }
    }

    public void PauseSound(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioItem item))
        {
            item.source.Pause();
        }
    }

    public void UnPauseSound(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioItem item))
        {
            item.source.UnPause();
        }
    }
}
