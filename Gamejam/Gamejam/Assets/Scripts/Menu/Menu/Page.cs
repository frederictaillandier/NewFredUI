using UnityEngine;
using System.Collections;
public enum MENUPAGESTATE { Main = 0, ChooseLevel = 1, Options = 2, Credits = 3, InGameUI = 4, GameOver = 5};
public class Page : MonoBehaviour {

    public MENUPAGESTATE pageState;
    //bool _selected = true;
    public Transform[] ModelToShow;

    delegate void AnimationUpdate();
    AnimationUpdate _animationUpdate;


    // Use this for initialization
	virtual protected void Start () 
    {
	}
	
    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            if (pageState == MENUPAGESTATE.Main)
                Application.Quit();
            else if (pageState == MENUPAGESTATE.InGameUI)
                PagesManager.SingletonSelect((int)MENUPAGESTATE.GameOver);
            else
                PagesManager.SingletonSelect((int)MENUPAGESTATE.Main);
        }
    }


	// Update is called once per frame
    virtual protected void Update()
    {
        if (_animationUpdate != null)
            _animationUpdate();
	}

    void ActiveAnimation()
    {
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, Time.deltaTime * 16);
        if (this.transform.localScale.magnitude > Vector3.one.magnitude * 0.9f)
        {
            this.transform.localScale = Vector3.one;
            _animationUpdate = null;
           
        }
    }

    void DeActiveAnimation()
    {
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero, Time.deltaTime * 16);
        if (this.transform.localScale.magnitude < 0.1f)
        {
            this.transform.localScale = Vector3.zero;
            _animationUpdate = null;
            this.gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        _animationUpdate = ActiveAnimation;
        this.gameObject.SetActive(true);
    }

    public virtual  void SetSelection(bool selected)
    {
        if (selected)
        {
            Invoke("Open", 0.2f);
        }
        else
            _animationUpdate = DeActiveAnimation;

        for (int i = 0; i < ModelToShow.Length; ++i)
        {
            if (ModelToShow[i] != null)
                ModelToShow[i].gameObject.SetActive(selected);
            else
                Debug.LogError("ModelMissing");
        
        }

    }
}
