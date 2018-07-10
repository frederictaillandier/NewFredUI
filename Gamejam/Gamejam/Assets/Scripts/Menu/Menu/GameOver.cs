using UnityEngine;
using System.Collections;

public class GameOver : Page
{ 
    //Set the game to GameOver and shows the UImatching
    public static void Show()
    {
        PagesManager.SingletonSelect((int)MENUPAGESTATE.GameOver);
    }

   

    //Override of "pages" specific to GameOver state
    public override void SetSelection(bool selected)
    {
        base.SetSelection(selected);
        if (this.gameObject.activeInHierarchy == true && selected == false)//by exiting the gameOver, we go to the main menu
          
        Application.LoadLevel(0);
        if (selected == true && (PlayerPrefs.GetInt("soundStatus") == 1)) //Play GameOver Sound
        {
            MusicPlayer.Stop();
            GetComponent<AudioSource>().Play();
        }
    }

}
