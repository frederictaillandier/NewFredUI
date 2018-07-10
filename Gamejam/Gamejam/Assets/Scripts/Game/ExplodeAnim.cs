using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodeAnim : MonoBehaviour
{

    public Sprite[] anim;
    public float speed = 5;
    float time = 0;

    public UnityEngine.UI.Image img = null;

    public static List<ExplodeAnim> poolOn = new List<ExplodeAnim>();
    public static List<ExplodeAnim> poolOff = new List<ExplodeAnim>();

    static public ExplodeAnim CreateFromPool()
    {
        ExplodeAnim result = null;        

        if (poolOff.Count == 0)
        {
            GameObject tmp = Instantiate<GameObject>(DDMapCreator.instance.squareReferences.explode.gameObject);
            tmp.transform.SetParent(DDMapCreator.instance.transform);
            tmp.transform.localScale = Vector3.one;
            result = tmp.GetComponent<ExplodeAnim>();
            poolOn.Add(result);           
        }
        else
        {
            result = poolOff[0];
            poolOn.Add(result);
            poolOff.RemoveAt(0);
        }
        result.gameObject.SetActive(true);
        return (result);
    }

    public void Death()
    {       
        this.gameObject.SetActive(false);
        ExplodeAnim.poolOn.Remove(this);
        ExplodeAnim.poolOff.Add(this);
        time = 0;
    }


    void Update()
    {
        time += Time.deltaTime * speed;
        if (Mathf.FloorToInt(time) < anim.Length)
        {
            img.sprite = anim[Mathf.FloorToInt(time)];
        }
        else
        {
            Death();
        }
    }

    static public void FreePools()
    {
        poolOn.Clear();
        poolOff.Clear();
    }
}
