using UnityEngine;
using System.Collections;

//Simple rotation Animation
public class AutoRotate : MonoBehaviour {
    public float speed = 1;

	void Update () {
        this.transform.Rotate(Vector3.up, Time.deltaTime * 90 * speed, Space.Self);
	}
}
