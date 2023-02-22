using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingPaintScoring : Scoring
{
    [SerializeField] protected string scoreTitle = "Score Title";

    protected override void getTotal(ref List<ScoreTotal> Totals)
    {
        if (GetComponent<HangingPaint>() == null) return;

        int score = 0;
        float angle = Vector3.Angle(Vector3.up, transform.up);
        if (angle < 3.0f)
        {
            score = 250;
        }
        else if (angle < 5.0f)
        {
            score = 150;
        }
        else if (angle < 15.0f)
        {
            score = -50;
        }
        else if (angle < 20.0f)
        {
            score = -100;
        }
        else
        {
            score = -200;
        }

        Totals.Add(new Scoring.ScoreTotal(scoreTitle, score));
    }
}