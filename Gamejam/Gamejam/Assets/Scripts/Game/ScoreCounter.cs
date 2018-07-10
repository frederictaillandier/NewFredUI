using UnityEngine;
using System.Collections;

public class ScoreCounter : MonoBehaviour
{

    public UnityEngine.UI.Text text = null;
    float score = 0;
    int targetScore = 0;
    public static ScoreCounter instance = null;

    void Awake()
    {
        instance = this;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = "Score :" + Mathf.FloorToInt( score).ToString();
    }

    public void Collect()
    {
        GetComponent<Animator>().Play("Scale");
        targetScore += 100;
        UpdateText();
    }
    void Update()
    {
        if (score != targetScore)
        {
            score +=  Time.deltaTime * ((targetScore - score)) * 2;
            if (targetScore - score < 10)
                score = targetScore;
            UpdateText();
        }
    }
}
