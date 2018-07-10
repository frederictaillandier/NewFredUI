using UnityEngine;
using System.Collections;

public class WinEvent : MonoBehaviour
{

    static WinEvent instance;
    public Transform WinScreen = null;
    public Transform LooseScreen = null;

    void Awake()
    {
        instance = this;
    }

    static public void Win()
    {
        instance.FreePools();
        Highscore.instance.AddScore( Mathf.FloorToInt( Time.timeSinceLevelLoad));
        instance.WinScreen.gameObject.SetActive(true);
        instance.Invoke("LoadMenu", 5f);
    }

    static public void Loose()
    {
        instance.FreePools();
        instance.LooseScreen.gameObject.SetActive(true);
        instance.Invoke("LoadMenu", 5f);
    }

    public void FreePools()
    {
        DDRed.FreePools();
        DDYellow.FreePools();
        DDWhite.FreePools();
        DDBlue.FreePools();
        ExplodeAnim.FreePools();
    }

    void OnGUI()
    {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
        {
            instance.FreePools();
            LoadMenu();
        }
    }

    public void LoadMenu()
    {       
        LoadingScreen.LoadScene(0);
    }
}
