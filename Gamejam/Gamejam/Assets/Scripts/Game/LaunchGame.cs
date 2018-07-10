using UnityEngine;
using System.Collections;

public class LaunchGame : MonoBehaviour
{
    public void LaunchGameNow()
    {
        LoadingScreen.LoadScene(1);
    }
}
