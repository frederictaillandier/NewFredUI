using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class StencilManager : MonoBehaviour
{

    public int stencilValue = 0;
    public Color planeColor = Color.white;
    Renderer[] renderers;

    // Use this for initialization
    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            //renderers[i].material.SetInt("_StencilVal", stencilValue);

            Material tmpMaterial = new Material(renderers[i].sharedMaterial);//prevents errors in editor
            tmpMaterial.SetInt("_StencilVal", stencilValue);
            if (tmpMaterial.HasProperty("_ColorWindow"))
                tmpMaterial.SetColor("_ColorWindow", planeColor);

            renderers[i].sharedMaterial = tmpMaterial;

        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            //  Debug.Log("aaa");
            if (renderers == null)
                renderers = GetComponentsInChildren<Renderer>();
            if (renderers != null)
                for (int i = 0; i < renderers.Length; ++i)
                {
                    // renderers[i].material.SetInt("_StencilVal", stencilValue);
                    Material tmpMaterial = new Material(renderers[i].sharedMaterial);//prevents errors in editor
                    tmpMaterial.SetInt("_StencilVal", stencilValue);
                    if (tmpMaterial.HasProperty("_ColorWindow"))
                        tmpMaterial.SetColor("_ColorWindow", planeColor);

                    renderers[i].sharedMaterial = tmpMaterial;
                }
        }

    }

}
