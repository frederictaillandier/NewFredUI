using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour
{
    public UnityEngine.UI.Text languageStatus;//Text matching the definition of the language Activated
    public UnityEngine.UI.Text musicStatus;//Text matching the definition of the music activation
    public UnityEngine.UI.Text soundStatus;//Text matching the definition of the sound activation
    public UnityEngine.UI.Text scaleStatus;//Text matching the definition of the sound activation
  

    void OnEnable()//Initialise all the text statuses 
    {
        int musicOn = PlayerPrefs.GetInt("musicStatus");
        if (musicOn == 1)
        {
            musicStatus.text = Localize.getString("No music");
            musicStatus.name = "No music";
        }
        else
        {
            musicStatus.text = Localize.getString("Music");
            musicStatus.name = "Music";
        }
        int soundOn = PlayerPrefs.GetInt("soundStatus");
        if (soundOn == 1)
        {
            soundStatus.text = Localize.getString("No sound");
            soundStatus.name = "No sound";
        }
        else
        {
            soundStatus.text = Localize.getString("Sound");
            soundStatus.name = "Sound";
        }

        int canScale = PlayerPrefs.GetInt("canScale");
        if (canScale == 1)
        {
            scaleStatus.text = Localize.getString("Scale Map");
            scaleStatus.name = "Scale Map";
        }
        else
        {
            scaleStatus.text = Localize.getString("No Scale");
            scaleStatus.name = "No Scale";           
        }


        int language = (PlayerPrefs.GetInt("language"));
        
        languageStatus.text = Localize.getString("Language");
        
          
    }

    public void SwitchLanguage()
    {
        int language= (((PlayerPrefs.GetInt("language") + 1) % 2));
        PlayerPrefs.SetInt("language", language);
        Localize.Reload(language);
        if (PlayerPrefs.GetInt("soundStatus") == 0)
        GetComponent<AudioSource>().Play();
    }


    public void SwitchMusic()
    {
        int musicOn = (PlayerPrefs.GetInt("musicStatus") == 0 ? 1 : 0);
        PlayerPrefs.SetInt("musicStatus", musicOn);
        if (musicOn == 1)
        {
            musicStatus.text = Localize.getString("No music");
            musicStatus.name = "No music";
            MusicPlayer.Stop();
        }
        else
        {

            musicStatus.text = Localize.getString("Music");
            musicStatus.name = "Music";
            MusicPlayer.Play();
        }
        if (PlayerPrefs.GetInt("soundStatus") == 0)
        GetComponent<AudioSource>().Play();
    }
    public void SwitchSound()
    {
        int soundOn = (PlayerPrefs.GetInt("soundStatus") == 0 ? 1 : 0);
        PlayerPrefs.SetInt("soundStatus", soundOn);
        if (soundOn  == 1)
        {
            soundStatus.text = Localize.getString("No sound");
            soundStatus.name = "No sound";
        }
        else
        {
            soundStatus.text = Localize.getString("Sound");
            soundStatus.name = "Sound";
        }
        if (PlayerPrefs.GetInt("soundStatus") == 0)
        GetComponent<AudioSource>().Play();
    }
    public void SwitcScale()
    {
        int canScale = (PlayerPrefs.GetInt("canScale") == 0 ? 1 : 0);
        PlayerPrefs.SetInt("canScale", canScale);
        if (canScale == 1)
        {
            scaleStatus.text = Localize.getString("Scale Map");
            scaleStatus.name = "Scale Map";
          
        }
        else
        {
            scaleStatus.text = Localize.getString("No Scale");
            scaleStatus.name = "No Scale";    
        }
        if (PlayerPrefs.GetInt("soundStatus") == 0)
            GetComponent<AudioSource>().Play();
    }
   
}
