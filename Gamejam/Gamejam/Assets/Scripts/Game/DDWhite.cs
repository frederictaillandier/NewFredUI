using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDWhite : MonoBehaviour
{
    public static List<DDWhite> poolOn = new List<DDWhite>();
    public static List<DDWhite> poolOff = new List<DDWhite>();

    static public DDWhite CreateFromPool()
    {
        DDWhite result = null;
      
        if (poolOff.Count == 0)
        {
            GameObject tmp = Instantiate<GameObject>(DDMapCreator.instance.squareReferences.whiteRef.gameObject);
            tmp.transform.SetParent(DDMapCreator.instance.transform);
            tmp.transform.localScale = Vector3.one;
            result = tmp.GetComponent<DDWhite>();
            poolOn.Add(result);
        }
        else
        {
            result = poolOff[0];
            poolOn.Add(result);
            poolOff.RemoveAt(0);
        }
        return (result);
    }
    
    void Repop()
    {
        gameObject.SetActive(false);
        poolOn.Remove(this);
        //poolOff.Add(this);
        Destroy(this);
        if (poolOn.Count + DDYellow.poolOn.Count + CubeCounter.instance.Count() < DDRed.poolOn.Count)
        {
            WinEvent.Loose();
        }
        SoundPool.PlayEatWhite();
    }  

    void OnTriggerEnter2D(Collider2D other)
    {
        Repop();
        if (other.gameObject.tag == "Player")
            CubeCounter.instance.Collect();
    }
    static public void FreePools()
    {
        poolOn.Clear();
        poolOn = new List<DDWhite>();
        poolOff.Clear();
        poolOff = new List<DDWhite>();
    }
}
