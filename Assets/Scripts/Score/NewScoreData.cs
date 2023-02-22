using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NewScoreData 
{
    public List<NewScore> scores;

    public NewScoreData()
    {
        scores = new List<NewScore>();
    }
}
