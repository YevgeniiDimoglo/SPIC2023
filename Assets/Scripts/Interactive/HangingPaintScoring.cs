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
        if (angle < 5.0f)
        {
            score = 100;
        }
        else if (angle < 10.0f)
        {
            score = 50;
        }
        else if (angle < 15.0f)
        {
            score = -20;
        }
        else
        {
            score = -50;
        }

        Totals.Add(new Scoring.ScoreTotal(scoreTitle, score));
    }
}