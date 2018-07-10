using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDYellow : MonoBehaviour
{

    public static List<DDYellow> poolOn = new List<DDYellow>();
    public static List<DDYellow> poolOff = new List<DDYellow>();

    public float lifeTime = 5;

    static public DDYellow CreateFromPool()
    {
        DDYellow result = null;

        if (poolOff.Count == 0)
        {
            GameObject tmp = Instantiate<GameObject>(DDMapCreator.instance.squareReferences.yellowRef.gameObject);
            tmp.transform.SetParent(DDMapCreator.instance.transform);
            tmp.transform.localScale = Vector3.one;
            result = tmp.GetComponent<DDYellow>();
            poolOn.Add(result);
            result.gameObject.SetActive(true);
        }
        else
        {
            result = poolOff[0];
            poolOn.Add(result);
            poolOff.RemoveAt(0);
            result.gameObject.SetActive(true);
        }
        SoundPool.PlayBomb();
        return (result);
    }

    void Update()
    {
        if (!MapEditor.active)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
        poolOn.Remove(this);
        poolOff.Add(this);
        lifeTime = 5;
        ExplodeAnim anim = ExplodeAnim.CreateFromPool();
        anim.transform.position = this.transform.position;
    }

    public void EditorKill()
    {
        if (MapEditor.active)
            Death();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            DDRed foe = other.GetComponent<DDRed>();

            if (foe != null)
            {
                Death();
                foe.Death();
            }
        }
    }

    static public void FreePools()
    {
        poolOn.Clear();
        poolOff.Clear();
    }

}
