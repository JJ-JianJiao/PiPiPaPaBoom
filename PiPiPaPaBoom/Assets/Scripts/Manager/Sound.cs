using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound
{
    public SoundName sName;
    public Musicname mName;


    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}


public enum SoundName
{
    PlayerGetHurt,
    PlayerDie,
    EnimyGetHurt,
    EnimyDie,
    BoomExplode,
    WhaleDie,
    Swallow,
    KickBall,
    ThrowBall,
    None
}

public enum Musicname
{
    Main,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    None
}