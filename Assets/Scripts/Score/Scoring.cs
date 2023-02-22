using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    protected static List<Scoring> ScoringItems = new List<Scoring>();

    public struct ScoreTotal
    {
        public string title;
        public int value;

        public ScoreTotal(string t, int v)
        {
            this.title = t;
            this.value = v;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ScoringItems.Add(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected virtual void getTotal(ref List<ScoreTotal> Totals)
    {
    }

    public static List<ScoreTotal> getTotals()
    {
        List<ScoreTotal> Totals = new List<ScoreTotal>();

        foreach (Scoring item in ScoringItems)
        {
            item.getTotal(ref Totals);
        }

        return Totals;
    }
}