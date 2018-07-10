using UnityEngine;
using System.Collections;

public class CubeCounter : MonoBehaviour {

    public UnityEngine.UI.Text text = null;
    int collected = 0;
    public static CubeCounter instance = null;

    void Awake()
    {
        instance = this;
        UpdateText();
    }

    public void UpdateText()
    {
        
          text.text = collected.ToString();
    }

    public void Collect()
    {
        GetComponent<Animator>().Play("Scale");
        ++collected;
        UpdateText();
    }
    public void Drop()
    {
      
        --collected; 
        UpdateText();
    }

    public bool HasSome()
    {
        return (collected > 0);
    }

    public int Count()
    {
        return collected;
    }


}
