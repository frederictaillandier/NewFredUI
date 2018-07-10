using UnityEngine;
using System.Collections;

public class SelectMapPage : Page
{
    public int[] Scenes;//List of scenes id matching the selectables levels
    public Transform[] tinyMaps;//Loadable preview of the selectables levels
  
    int _tinyMapIndex = 0;//Index of current selected level
    Transform currentTinyMap;//preview of the current level
    public Transform tinyMapPosition;//Position in scene to show the preview
    public UnityEngine.UI.Text bestScoreText;//best score matching the selected map

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        currentTinyMap = Instantiate(tinyMaps[_tinyMapIndex], tinyMapPosition.position, Quaternion.identity) as Transform;
        currentTinyMap.parent = transform;
    }

    void OnEnable()
    {
        string scoreString = ("Score" + Scenes[_tinyMapIndex].ToString());
        if (PlayerPrefs.HasKey(scoreString))
            bestScoreText.text = PlayerPrefs.GetInt(scoreString).ToString();
        else
            bestScoreText.text = "0";
    }


    public void NextMap()//Selects next map
    {
        if (PlayerPrefs.GetInt("soundStatus") == 1)
            GetComponent<AudioSource>().Play();
        Destroy(currentTinyMap.gameObject);
        _tinyMapIndex = (_tinyMapIndex + 1) % tinyMaps.Length;
        currentTinyMap = Instantiate(tinyMaps[_tinyMapIndex], tinyMapPosition.position, Quaternion.identity) as Transform;
        currentTinyMap.parent = transform;
        string scoreString = ("Score" + Scenes[_tinyMapIndex].ToString());
        if (PlayerPrefs.HasKey(scoreString))
            bestScoreText.text = PlayerPrefs.GetInt(scoreString).ToString();
        else
            bestScoreText.text = "0";
    }

    public void PreviousMap()//Selects previous map
    {
        if (PlayerPrefs.GetInt("soundStatus") == 1)
            GetComponent<AudioSource>().Play();
        Destroy(currentTinyMap.gameObject);
        _tinyMapIndex = (_tinyMapIndex - 1 + tinyMaps.Length) % tinyMaps.Length;
        currentTinyMap = Instantiate(tinyMaps[_tinyMapIndex], tinyMapPosition.position, Quaternion.identity) as Transform;
        currentTinyMap.parent = transform;
        string scoreString = ("Score" + Scenes[_tinyMapIndex].ToString());
        if (PlayerPrefs.HasKey(scoreString))
            bestScoreText.text = PlayerPrefs.GetInt(scoreString).ToString();
        else
            bestScoreText.text = "0";
    }

    void LoadLevel()
    {

        Application.LoadLevel(Scenes[_tinyMapIndex]);
    }

    public void LoadGame()//Load the selected map
    {
        Invoke("LoadLevel", 0.5f);
       
    }


    override protected void Update()//Rotate Animation
    {
        base.Update();
      //  currentTinyMap.Rotate(Vector3.up, Time.deltaTime * 10, Space.Self);
    }
}
