using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ShowScore : MonoBehaviour
{
   public UnityEngine.UI.Text text = null;

    public void Show()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("");

        for (int i = 0; i < Highscore.instance.current.scores.Count && i < 5; ++i)
        {
            sb.Append(Highscore.instance.current.scores[i].name);
            sb.Append(" ");
            sb.Append(Highscore.instance.current.scores[i].points.ToString());
            sb.Append("seconds\n");
        }
        text.text = sb.ToString();

        
    }
}
