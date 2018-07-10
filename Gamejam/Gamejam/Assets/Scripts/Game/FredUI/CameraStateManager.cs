using UnityEngine;
using System.Collections;

public class CameraStateManager : MonoBehaviour {

    Animator animator = null;

    public void setState(int val)
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("state", val);
        }
    }
}
