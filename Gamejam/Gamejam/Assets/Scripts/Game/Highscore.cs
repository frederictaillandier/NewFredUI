
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Highscore : MonoBehaviour
{
    static public Highscore instance = null;

    [System.Serializable]
    public class Score
    {
      
        public string name;
        public int points;

        public Score(string newName, int newScore)
        {
            name = newName;
            points = newScore;
        }
    }
    [System.Serializable]
   public class Scores
    {
        public string currentName = "Anonymus";
        public List<Score> scores = new List<Score>();
    }
    public Scores current = new Scores();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        current.scores.Sort(SortList);
    }

    public void AddScore(int val)
    {
        current.scores.Add(new Score(current.currentName, val));

        current.scores.Sort(SortList);

        while (current.scores.Count > 5)
        {
            current.scores.RemoveAt(current.scores.Count - 1);
        }

        Save();
    }

   int SortList(Score A, Score B)
    {
        return (A.points - B.points);

    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            instance.current = (Scores)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, instance.current);
        file.Close();
    }
}
