using UnityEngine;
using System.Collections;

public class LifeCounter : MonoBehaviour {

    public UnityEngine.UI.Text text = null;
    int lives = 3;
    public static LifeCounter instance = null;

    Animator[] animators;

    void Awake()
    {
        instance = this;
        animators = GetComponentsInChildren<Animator>();
    }
   
    public void Death()
    {
        --lives;
       if (lives >= 0)
        animators[lives].SetBool("active", false);
        if (lives <= 0)
            WinEvent.Loose();
    }

    public bool HasSome()
    {
        return (lives > 0);
    }
}
