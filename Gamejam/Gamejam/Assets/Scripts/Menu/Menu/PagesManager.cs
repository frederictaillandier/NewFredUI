using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PagesManager : MonoBehaviour {

     static Page[] _pageList;//All the pages we manage at this moment
     public Page[] pageListConstructor;//Helper to set it up into the inspector because the inspector doesn t handle statics
     static PagesManager Instance = null;//Singleton

    void Awake()
     {
        if (Instance != null)//There can be only one
        {
            Destroy(this.gameObject);
            return;
        }
         Instance = this;
         DontDestroyOnLoad(gameObject);//Keep the UI throught the scenes
        _pageList = pageListConstructor;//buid the static dictionary with the localOne
     }

    static public void SingletonSelect(int state)//Shows the selected UI
    {
        if (PlayerPrefs.GetInt("soundStatus") == 1)
        Instance.GetComponent<AudioSource>().Play();
        for (int i = 0; i < _pageList.Length; ++i)
        {
            if (i != state)
            _pageList[i].SetSelection(false);
        }
        _pageList[state].SetSelection(true);
    }

    static public void SingletonSelect(Page toActivate)//Shows the selected UI
    {
        if (PlayerPrefs.GetInt("soundStatus") == 1)
        Instance.GetComponent<AudioSource>().Play();
        for (int i = 0; i < _pageList.Length; ++i)
        {
            if (_pageList[i] != toActivate)
            _pageList[i].SetSelection(false);
        }
        toActivate.SetSelection(true);
    }

    public void Select(MENUPAGESTATE state)//Shows the selected UI
    {
        PagesManager.SingletonSelect((int)state);
    }
    public void Select(Page page)//Shows the selected UI
    {
        PagesManager.SingletonSelect(page);
    }

}
