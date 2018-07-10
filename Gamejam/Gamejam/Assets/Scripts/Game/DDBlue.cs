using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDBlue : MonoBehaviour {

    public static List<DDBlue> poolOn = new List<DDBlue>();
    public static List<DDBlue> poolOff = new List<DDBlue>();   

    static public DDBlue CreateFromPool()
    {
        DDBlue result = null;

        if (poolOff.Count == 0)
        {
            GameObject tmp = Instantiate<GameObject>(DDMapCreator.instance.squareReferences.blueRef.gameObject);
            tmp.transform.SetParent(DDMapCreator.instance.transform);
            tmp.transform.localScale = Vector3.one;
            result = tmp.GetComponent<DDBlue>();
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

    static public void FreePools()
    {
        poolOn.Clear();
        poolOff.Clear();
    }
}
