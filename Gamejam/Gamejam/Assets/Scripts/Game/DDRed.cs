using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDRed : MonoBehaviour
{

    public static List<DDRed> poolOn = new List<DDRed>();
    public static List<DDRed> poolOff = new List<DDRed>();

    float imgIndex = 0;
    public UnityEngine.UI.Image character = null;

    public Vector3 target = Vector3.zero;
    public Vector3 nextPos = Vector3.zero;
    public float speed = 5f;

    List<Sprite> currentCollectionAnimator = null;

    bool alert = false;

    static public DDRed CreateFromPool()
    {
        DDRed result = null;

        if (poolOff.Count == 0)
        {
            GameObject tmp = Instantiate<GameObject>(DDMapCreator.instance.squareReferences.redRef.gameObject);
            tmp.transform.SetParent(DDMapCreator.instance.transform);
            tmp.transform.localScale = Vector3.one;
            result = tmp.GetComponent<DDRed>();
            result.imgIndex = 0;
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

    // Use this for initialization
    void Start()
    {
        target = new Vector3(Random.Range(0, 64), Random.Range(0, 64), 0);
        nextPos = this.transform.localPosition / 16;
    }

    void UpdateNext()
    {

        if (((Mathf.Abs(transform.localPosition.x / 16 - DDPlayer.instance.nextX) < 5) ||
            (Mathf.Abs(transform.localPosition.y / 16 - DDPlayer.instance.nextY) < 5)) && DDPlayer.instance.flashing == 0)
        {
            target = new Vector3(DDPlayer.instance.nextX, DDPlayer.instance.nextY, 0);
            alert = true;
        }
        else if (alert)
        {
            alert = false;
            target = new Vector3(Random.Range(0, 64), Random.Range(0, 64), 0);
        }

        if (alert == false)
        {
            if (target.x == nextPos.x && target.y == nextPos.y)
                target = new Vector3(Random.Range(0, 64), Random.Range(0, 64), 0);
        }

        if (Mathf.Abs(target.x - nextPos.x) > Mathf.Abs(target.y - nextPos.y))
        {
            if (nextPos.x < target.x)
                if (!DDMapCreator.instance.walls[Mathf.RoundToInt(nextPos.y) * 64 + Mathf.RoundToInt(nextPos.x) + 1])
                {
                    nextPos.x++;
                }
                else
                    nextPos.y += (nextPos.y == 0 ? 1 : -1);
            else
                if (!DDMapCreator.instance.walls[Mathf.RoundToInt(nextPos.y) * 64 + Mathf.RoundToInt(nextPos.x) - 1])
                    nextPos.x--;
                else
                    nextPos.y += (nextPos.y == 0 ? 1 : -1);
        }
        else
        {
            if (nextPos.y < target.y)
                if (!DDMapCreator.instance.walls[(Mathf.RoundToInt(nextPos.y) + 1) * 64 + Mathf.RoundToInt(nextPos.x)])
                    nextPos.y++;
                else
                    nextPos.x += (nextPos.x == 0 ? 1 : -1);
            else
                if (!DDMapCreator.instance.walls[(Mathf.RoundToInt(nextPos.y) - 1) * 64 + Mathf.RoundToInt(nextPos.x)])
                    nextPos.y--;
                else
                    nextPos.x += (nextPos.x == 0 ? 1 : -1);
        }
    }

    public void EditorKill()
    {
        if (MapEditor.active)
            Death();

    }

    void UpdateAnimator(ref Vector3 move)
    {
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if (move.x > 0)
                currentCollectionAnimator = SpriteAnimatorManager.instance.red.right;
            else
                currentCollectionAnimator = SpriteAnimatorManager.instance.red.left;
        }
        else
        {
            if (move.y > 0)
                currentCollectionAnimator = SpriteAnimatorManager.instance.red.back;
            else
                currentCollectionAnimator = SpriteAnimatorManager.instance.red.front;

        }    
    }



    // Update is called once per frame
    void Update()
    {
        if (!MapEditor.active)
        {
            Vector3 move = (nextPos * 16 - transform.localPosition) * Time.deltaTime * speed;
            transform.localPosition += move;

            UpdateAnimator(ref move);


            if (Vector3.Distance(transform.localPosition / 16, nextPos) < 0.1f)
            {
                transform.localPosition = nextPos * 16;
                UpdateNext();
            }
        }
        if (currentCollectionAnimator != null)
        {
            character.sprite = currentCollectionAnimator[Mathf.FloorToInt(imgIndex)];
            imgIndex += Time.deltaTime * 10;
            imgIndex %= currentCollectionAnimator.Count;
        }
    }

    public void Death()
    {
        SoundPool.PlayDeath();
        this.gameObject.SetActive(false);
        DDRed.poolOn.Remove(this);
        DDRed.poolOff.Add(this);
        if (!MapEditor.active)
            ScoreCounter.instance.Collect();
        if (poolOn.Count == 0)
            WinEvent.Win();
       
    }

   
    static public void FreePools()
    {
        poolOn.Clear();
        poolOff.Clear();
    }
}
