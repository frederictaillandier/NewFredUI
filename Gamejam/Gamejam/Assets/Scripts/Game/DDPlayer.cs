using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DDPlayer : MonoBehaviour
{
    static public DDPlayer instance = null;

    enum DDDirection { up, down, left, right };
    List<DDDirection> path = new List<DDDirection>();
    UnityEngine.UI.Image img = null;
    float imgIndex = 0;
    public UnityEngine.UI.Image character = null;
    List<Sprite> currentCollectionAnimator = null;
    public int nextX = 1;
    public int nextY = 62;

    public UnityEngine.UI.Image targetIMG = null;
    public int Speed = 10;
    public Transform ground = null;

    public float flashing = 0f;


    // Use this for initialization
    void Start()
    {
        instance = this;
        currentCollectionAnimator = SpriteAnimatorManager.instance.green.front;
        img = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!MapEditor.active)
        {
            if (flashing > 0)
            {
                flashing -= Time.deltaTime;
                if (flashing < 0)
                {
                    flashing = 0;
                    img.color = Color.green;
                }
                if (flashing > 0)
                    img.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }

            if (ground != null)
            {
                Vector3 move = -(new Vector3(nextX * 16, nextY * 16, 0) + ground.localPosition) * Time.deltaTime * Speed / 2;
                ground.localPosition = ground.localPosition + move;

                if (move.magnitude < 1f && path.Count > 0)
                {
                    if (path[0] == DDDirection.up)
                        ++nextY;
                    else if (path[0] == DDDirection.down)
                        --nextY;
                    else if (path[0] == DDDirection.right)
                        ++nextX;
                    else if (path[0] == DDDirection.left)
                        --nextX;

                    UpdateAnimator(path[0]);

                    path.RemoveAt(0);
                }
            }
        }

        if (currentCollectionAnimator != null && currentCollectionAnimator.Count > 0)
            character.sprite = currentCollectionAnimator[Mathf.FloorToInt(imgIndex)];
        imgIndex += Time.deltaTime * 10;
        imgIndex %= currentCollectionAnimator.Count;

    }



    bool prevFail = false;

    public void PathFinding(ref Vector3 target) //Well, I could implement a A*, but this one is faster to implement since the walls are far from each other :) 
    {
        int currX = nextX;
        int currY = nextY;

        path.Clear();

        while (!(currX == target.x && currY == target.y))
        {
            if ((!prevFail && Mathf.Abs(target.x - currX) > Mathf.Abs(target.y - currY)) || (prevFail && !(Mathf.Abs(target.x - currX) > Mathf.Abs(target.y - currY))))
            {

                if (currX < target.x)
                {
                    if (!prevFail)
                    {
                        if (currX + 1 >= 64)
                           break;
                        if (DDMapCreator.instance.walls[(int)currY * 64 + (int)currX + 1])
                        {
                            if (currY == target.y && currX + 1 == target.x)
                               break;
                            prevFail = true;
                            continue;
                        }
                        path.Add(DDDirection.right);
                        currX++;
                    }
                    else
                    {
                        if (currX + 1 >= 64)
                        {
                            path.Add(DDDirection.left);
                            currX--;

                        }
                        else
                        {
                            path.Add(DDDirection.right);
                            currX++;
                        }
                    }
                }
                else
                {
                    if (!prevFail)
                    {
                        if (currX - 1 < 0)
                            break;
                        if (DDMapCreator.instance.walls[(int)currY * 64 + (int)currX - 1])
                        {
                            if (currY == target.y && currX - 1 == target.x)
                               break;
                            prevFail = true;
                            continue;
                        }

                        path.Add(DDDirection.left);
                        currX--;
                    }
                    else
                    {
                        if (currX - 1 < 0)
                        {

                            path.Add(DDDirection.right);
                            currX++;
                        }
                        else
                        {
                            path.Add(DDDirection.left);
                            currX--;
                        }
                    }
                }
            }
            else
            {
                if (currY < target.y)
                {
                    if (!prevFail)
                    {
                        if (currY + 1 >= 64)
                            break;
                        if (DDMapCreator.instance.walls[(int)(currY + 1) * 64 + (int)currX])
                        {
                            if (currY + 1 == target.y && currX == target.x)
                                break;
                            prevFail = true;
                            continue;
                        }
                        path.Add(DDDirection.up);
                        currY++;
                    }
                    else
                    {
                        if (currY + 1 >= 64)
                        {
                            path.Add(DDDirection.down);
                            currY--;
                        }
                        else
                        {
                            path.Add(DDDirection.up);
                            currY++;
                        }
                        prevFail = false;
                    }
                }
                else
                {
                    if (!prevFail)
                    {
                        if (currY - 1 < 0)
                           break;
                        if (DDMapCreator.instance.walls[(int)(currY - 1) * 64 + (int)currX])
                        {
                            if (currY - 1 == target.y && currX == target.x)
                                break;
                            prevFail = true;
                            continue;
                        }

                        path.Add(DDDirection.down);
                        currY--;
                    }
                    else
                    {
                        if (currY - 1 < 0)
                        {
                            path.Add(DDDirection.up);
                            currY++;
                        }
                        else
                        {
                            path.Add(DDDirection.down);
                            currY--;
                        }
                        prevFail = false;
                    }
                }
            }
            prevFail = false;
        }
        ShowTargetImg(currX, currY);
    }

    void ShowTargetImg(int x, int y)
    {
        if (targetIMG != null)
        {
            targetIMG.transform.SetAsLastSibling();
            targetIMG.gameObject.SetActive(true);
            targetIMG.transform.localPosition = new Vector3(x, y, 0) * 16;
        }
    }


    void DropMine()
    {
        if (CubeCounter.instance.HasSome())
        {
            DDYellow newMine = DDYellow.CreateFromPool();
            CubeCounter.instance.Drop();
            newMine.transform.localPosition = new Vector3(DDPlayer.instance.nextX * 16, DDPlayer.instance.nextY * 16, 0);
        }
        else
            SoundPool.NoBomb();
    }

    void OnGUI() //event handling
    {
        if (flashing != 0)
            return;
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Space)
            {
                DropMine();
            }
        }
        if (Event.current.type == EventType.MouseDown)
        {
            if (Event.current.button == 1)
            {
                DropMine();
            }
            if (Event.current.button == 0 && !(Event.current.mousePosition.x > Screen.width - (0.2 * Screen.width) && Event.current.mousePosition.y > Screen.height - (0.2 * Screen.width)))
            {
                TriggerMove();
            }

        }
    }


    public void TriggerMove()
    {
        if (flashing != 0)
            return;

        Vector3 target;

        target.x = Mathf.FloorToInt((Input.mousePosition.x - Screen.width / 2)/transform.lossyScale.x / 16) + nextX;
        target.y = Mathf.FloorToInt((Input.mousePosition.y - Screen.height / 2) / transform.lossyScale.y / 16) + nextY;
        target.z = 0;



        PathFinding(ref target);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DDRed foe = other.GetComponent<DDRed>();

        if (foe != null && flashing == 0f)
        {
            flashing = 3f;
            LifeCounter.instance.Death();
            path.Clear();
        }
    }

    void UpdateAnimator(DDDirection dir)
    {
        if (dir == DDDirection.right)
            currentCollectionAnimator = SpriteAnimatorManager.instance.green.right;
        else if (dir == DDDirection.left)
            currentCollectionAnimator = SpriteAnimatorManager.instance.green.left;

        else if (dir == DDDirection.up)
            currentCollectionAnimator = SpriteAnimatorManager.instance.green.back;
        else
            currentCollectionAnimator = SpriteAnimatorManager.instance.green.front;


    }


}
