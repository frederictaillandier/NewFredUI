using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen instance = null;
    int sceneToLoad = 0;
    bool loading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.Invoke("StopFakeLoad", 2f);
           // gameObject.SetActive(false);
            loading = true;
        }
        else
            Destroy(gameObject);
    }

    static public void LoadScene(int scene)
    {
        instance.gameObject.SetActive(true);
        instance.Invoke("RealLoad", 2f);
        instance.loading = true;
        instance.sceneToLoad = scene;
    }

    void RealLoad()
    {
        Application.LoadLevel(sceneToLoad);
        loading = false;
    }

    void StopFakeLoad()
    {
        loading = false;
    }

    void Update()
    {
        if (!loading && !Application.isLoadingLevel)
        {
            this.gameObject.SetActive(false);
        }
    }
}
