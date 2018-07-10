using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class MapEditor : MonoBehaviour {

    static public bool active = false;
    static bool redSelected = true;

    public Transform red = null;
    public Transform yellow = null;

    KeyCode[] code = {KeyCode.B, KeyCode.L, KeyCode.U, KeyCode.E, KeyCode.B, KeyCode.Y, KeyCode.T, KeyCode.E};
    int currentIndex = 0;

    void OnGUI()
    {
        if ((Event.current.isKey && Event.current.keyCode == code[currentIndex]) || Event.current.keyCode == KeyCode.F1)
        {
            ++currentIndex;
            if (currentIndex == code.Length)
                Activate();
        }
    }
    void Activate()
    {
        currentIndex = 0;
        active = true;
        GetComponent<Animator>().SetBool("active", true);
    }

    public void SelectYellow()
    {
        redSelected = false;
        yellow.localScale = Vector3.one * 1.3f;
        red.localScale = Vector3.one;
    }

    public void SelectRed()
    {
        redSelected = true;
       
        yellow.localScale = Vector3.one;
        red.localScale = Vector3.one * 1.3f;
    }

    public void OnPlaceOnMap()
    {
        if (active)
        {
            Vector3 target;

            target.x = Mathf.FloorToInt( ((Input.mousePosition.x - Screen.width / 2)/DDMapCreator.instance.transform.lossyScale.x  - DDMapCreator.instance.transform.localPosition.x) / 16);
            target.y = Mathf.FloorToInt(((Input.mousePosition.y - Screen.height / 2)/DDMapCreator.instance.transform.lossyScale.x - DDMapCreator.instance.transform.localPosition.y) / 16);
            target.z = 0;

            if (redSelected == true)
            {
                DDRed newFoe = DDRed.CreateFromPool();
                newFoe.transform.localPosition = target * 16;
            }

            if (redSelected == false)
            {
                DDYellow newTrap = DDYellow.CreateFromPool();
                newTrap.transform.localPosition = target * 16;
            }
        }
    }   

    public void DeActivate()
    {
        active = false;
        GetComponent<Animator>().SetBool("active", false);
    }

}
