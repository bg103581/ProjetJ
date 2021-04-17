using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    RUN, STRAFE, JUMP, COLLISION, FATAL_COLLISION, BALLON, RAT, CLAQUETTES, OVNI, TMAX, TWINGO
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;

    [SerializeField] private Sound[] sounds;

    private List<AudioSource> sources = new List<AudioSource>();

    private void Awake() {
        current = this;
    }

    private void Start() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

            sources.Add(s.source);
        }
    }

    public void PlaySound(SoundType type) {
        Sound sound = GetSoundByType(type);
        if (sound != null) {
            sound.source.Play();
        }
        else {
            Debug.LogWarning("SoundType : " + type + " is not found in sounds array");
        }
    }

    public void StopSound(SoundType type) {
        Sound sound = GetSoundByType(type);
        if (sound != null) {
            sound.source.Stop();
        }
        else {
            Debug.LogWarning("SoundType : " + type + " is not found in sounds array");
        }
    }

    public void PauseSound(SoundType type) {
        Sound sound = GetSoundByType(type);
        if (sound != null) {
            sound.source.Pause();
        }
        else {
            Debug.LogWarning("SoundType : " + type + " is not found in sounds array");
        }
    }

    public void UnPauseSound(SoundType type) {
        Sound sound = GetSoundByType(type);
        if (sound != null) {
            sound.source.UnPause();
        }
        else {
            Debug.LogWarning("SoundType : " + type + " is not found in sounds array");
        }
    }

    private Sound GetSoundByType(SoundType type) {
        foreach (Sound s in sounds) {
            if (s.type == type) return s;
        }
        return null;
    }

    public void MuteSounds()
    {
        foreach (AudioSource source in sources)
        {
            source.enabled = false;
        }
    }

    public void DemuteSounds()
    {
        foreach (AudioSource source in sources)
        {
            source.enabled = true;
        }
    }

    public void MuteMusic()
    {

    }

    public void DemuteMusic()
    {

    }
}
