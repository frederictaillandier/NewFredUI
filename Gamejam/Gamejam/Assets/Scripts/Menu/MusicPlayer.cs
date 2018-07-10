using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

    public AudioClip[] musics;
    static MusicPlayer Instance = null;

    void Start()
    {

        Instance = this;
        if ((PlayerPrefs.GetInt("musicStatus") == 0))
        {
            GetComponent<AudioSource>().clip = musics[Random.Range(0, musics.Length)];
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
    }
    public static void Stop()
    {
        if (Instance != null)
            Instance.GetComponent<AudioSource>().Stop();
    }

    public static void Play()
    {
        if (Instance != null)
            Instance.Start();
    }

}
