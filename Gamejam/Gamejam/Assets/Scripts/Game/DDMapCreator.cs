using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDMapCreator : MonoBehaviour
{
    static public DDMapCreator instance = null;

    public int width = 64;
    public int height = 64;

    [System.Serializable]
    public class Settings
    {
        public int squareWidth = 16;
    }
    public Settings settings;

    [System.Serializable]
    public class SquaresReferences
    {
        public DDWhite whiteRef = null;
        public DDYellow yellowRef = null;
        public DDRed redRef = null;
        public DDBlue blueRef = null;
        public ExplodeAnim explode = null;
    }
    public SquaresReferences squareReferences;

    bool[] wallsInit;
    public bool[] walls;
    public DDPlayer player = null;

    void InitWhite()
    {
        DDWhite.poolOn = new List<DDWhite>();
        DDWhite.poolOff = new List<DDWhite>();

        for (int i = 0; i < 64; ++i)
        {
            for (int j = 0; j < 64; ++j)
            {
                if (i % 4 == 0 && j % 4 == 0)
                {
                    DDWhite white = DDWhite.CreateFromPool();

                    white.transform.localPosition = new Vector3(i * 16, j * 16, 0);
                    wallsInit[j * 64 + i] = true;
                }
            }
        }
    }

    void OnGUI()
    {
        if (MapEditor.active && Event.current.type == EventType.MouseDrag)
        {
            this.transform.localPosition += new Vector3(Event.current.delta.x, -Event.current.delta.y, 0);
        }
    }


    void InitBlue()
    {
        DDBlue.poolOn = new List<DDBlue>();
        DDBlue.poolOff = new List<DDBlue>();

        int wallsToBuild = 32;

        while (wallsToBuild >= 0)
        {
            int dice = Random.Range(0, 64 * 64);
            if (!wallsInit[dice])
            {
                wallsToBuild--;
                int y = dice / 64;
                int x = dice % 64;

                DDBlue blue = DDBlue.CreateFromPool();
                blue.transform.localPosition = new Vector3(x, y, 0) * 16;
                walls[y * 64 + x] = true;
                for (int i = -3; i < 3; ++i)
                {
                    for (int j = -3; j < 3; ++j)
                    {
                        if (y + j >= 0 && y + j < 64 && x + i >= 0 && x + i < 64)

                            wallsInit[(y + j) * 64 + (x + i)] = true;
                    }
                }
            }
        }
    }

    void InitRed()
    {
        DDRed.poolOn = new List<DDRed>();
        DDRed.poolOff = new List<DDRed>();

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                DDRed red = DDRed.CreateFromPool();
                red.transform.localPosition = new Vector3(22 + i * 4, 10 + j * 4, 0) * 16;
                wallsInit[j * 64 + i] = true;
            }
        }
    }

    void Init()
    {
        ExplodeAnim.poolOn = new List<ExplodeAnim>();
        ExplodeAnim.poolOff = new List<ExplodeAnim>();

        InitWhite();
        InitRed();
        InitBlue();
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        wallsInit = new bool[64 * 64];
        walls = new bool[64 * 64];
        Init();
    }
}
