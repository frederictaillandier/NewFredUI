using UnityEngine;
using System.Collections;

public class NickNameUpdater : MonoBehaviour {

    public UnityEngine.UI.InputField input;

    public void LoadName()
    {
        Debug.Log("LoadName");
        input.text = Highscore.instance.current.currentName;

    }

	public void NewName()
    {
        Highscore.instance.current.currentName = input.text;
        Highscore.Save();
    }
}
