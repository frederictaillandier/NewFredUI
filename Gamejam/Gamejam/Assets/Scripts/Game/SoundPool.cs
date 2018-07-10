using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPool : MonoBehaviour
{
    [System.Serializable]
    public class SoundsLib
    {
        public AudioClip whitepicked;
        public AudioClip death;
        public AudioClip bomb;
        public AudioClip errorNoBomb;
    }

    public SoundsLib soundLib = new SoundsLib();

    public AudioSource source;

    static SoundPool instance = null;

    void Awake()
    {
        instance = this;
    }

    static public void PlayEatWhite()
    {
        if (PlayerPrefs.GetInt("soundStatus") == 0)
            instance.source.PlayOneShot(instance.soundLib.whitepicked);
    }
    static public void PlayDeath()
    {
        if (PlayerPrefs.GetInt("soundStatus") == 0)
            instance.source.PlayOneShot(instance.soundLib.death);
    }
    static public void PlayBomb()
    {
        if (PlayerPrefs.GetInt("soundStatus") == 0)
            instance.source.PlayOneShot(instance.soundLib.bomb);
    }
    static public void NoBomb()
    {
        if (PlayerPrefs.GetInt("soundStatus") == 0)
            instance.source.PlayOneShot(instance.soundLib.errorNoBomb);
    }

}
