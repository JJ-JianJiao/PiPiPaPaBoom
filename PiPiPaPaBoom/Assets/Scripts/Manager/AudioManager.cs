using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    public List<Sound> sounds;
    public List<Sound> musics;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    public void Play(SoundName name) {

        foreach (Sound s in sounds) {
            if (s.sName == name) {
                s.source.Play();
            }
        }
    }


    public void Play(Musicname name)
    {
        foreach (Sound m in musics)
        {
            if (m.source.isPlaying) m.source.Stop();
        }

        foreach (Sound m in musics)
        {
            if (m.mName == name)
            {
                m.source.Play();
            }
        }
    }
}
