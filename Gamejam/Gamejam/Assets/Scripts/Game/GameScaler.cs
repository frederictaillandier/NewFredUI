using UnityEngine;
using System.Collections;

public class GameScaler : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt("canScale") == 1)
        {
            this.transform.localScale = Vector3.one * Screen.height / (16 * 18);

        }
    }
}
