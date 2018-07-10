using UnityEngine;
using System.Collections;

//Simple cam following the Player, using the base settings
public class GameCam : MonoBehaviour {

    public Transform Target = null;

    Vector3 _setUp;

	// Use this for initialization
	void Start () 
    {
        if (Target!= null)
        _setUp = this.transform.position - Target.position;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
             transform.position = Vector3.Lerp(transform.position,  Target.transform.position + _setUp, Time.deltaTime * 4) ;
        }
    }
}
