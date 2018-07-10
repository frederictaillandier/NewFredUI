using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPage : MonoBehaviour
{

   public  Animator thisAnimator;
    public bool coexist = false;
    public string group = "default";
    static Dictionary<string, NewPage> currentPage = new Dictionary<string, NewPage>();
    public NewPage[] sonsObjects;

    [System.Serializable]
    public class Style
    {
        public Font font = null;
       public  Sprite sprite = null;    
    }

    public Style[] styleList;


    public void Activate()
    {
        if (thisAnimator == null)
            thisAnimator = GetComponent<Animator>();

        if (!coexist)
        {
            if (currentPage.ContainsKey(group) && currentPage[group] != null)
                currentPage[group].DeActivate();
            currentPage[group] = this;
        }
        gameObject.SetActive(true);
        thisAnimator.SetBool("active", true);
    }
    public void DeActivate()
    {
        if (thisAnimator == null)
            thisAnimator = GetComponent<Animator>();

        thisAnimator.SetBool("active", false);
    }

    public void ActivateAllSons()
    {
        for (int i = 0; i < sonsObjects.Length; ++i)
        {
            sonsObjects[i].Activate();
        }
    }

    public void ActivateSpecific(int index)
    {
        if (sonsObjects.Length > index)
            sonsObjects[index].Activate();
    }

    public void DeActivateAllSons()
    {
        for (int i = 0; i < sonsObjects.Length; ++i)
        {
            sonsObjects[i].DeActivate();
        }
    }
    public void DeActivateOver()
    {
        gameObject.SetActive(false);
    }

    public void SetStyle(int styleIndex)
    {
        UnityEngine.UI.Image img = GetComponent <UnityEngine.UI.Image>();
        if (img != null)
            img.sprite = styleList[styleIndex % styleList.Length].sprite;

        UnityEngine.UI.Text[] txt = GetComponentsInChildren<UnityEngine.UI.Text>();
        for (int i = 0; i < txt.Length;++i )
            txt[i].font = styleList[styleIndex % styleList.Length].font;

        for (int i = 0; i < sonsObjects.Length; ++i)
        {
            sonsObjects[i].SetStyle(styleIndex);
        }
    }
}
