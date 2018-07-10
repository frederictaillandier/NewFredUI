using UnityEngine;
using System.Collections;

public class ModelCard : MonoBehaviour
{
    bool _up = true;
    public NewPage newPageRef = null;
    public bool invert = false;

    public void Invert()
    {
        _up = !_up;
        transform.Rotate(-10, 0, 0);
        PlayerPrefs.SetInt("UIStyle", (PlayerPrefs.GetInt("UIStyle") + 1) % 2);
        if (newPageRef != null)
            newPageRef.SetStyle(PlayerPrefs.GetInt("UIStyle"));
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDrag)
        {
            transform.Rotate(-Event.current.delta.y, -Event.current.delta.x, 0);

            if (_up && (transform.rotation * Vector3.up).y < -0.1)
            {
                Invert();
            }
            else if ((!_up) && (transform.rotation * Vector3.up).y > 0.1)
            {
                Invert();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (invert == true)
        {
            Invert();
            invert = false;
        }
        if (!Input.GetMouseButton(0))
        {
            if (_up)
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.identity, Time.deltaTime * 4);
            else
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(180, 0, 0), Time.deltaTime * 4);
        }

    }
}
