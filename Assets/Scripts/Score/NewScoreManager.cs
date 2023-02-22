using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScoreManager : MonoBehaviour
{
    private NewScoreData newScoreData;
    // Start is called before the first frame update
    void Awake()
    {
        newScoreData = new NewScoreData();
    }

    // Update is called once per frame
    public IEnumerable<NewScore> GetScores()
    {
        return newScoreData.scores;
    }
    public void AddScore(NewScore score)
    {
        newScoreData.scores.Add(score);
    }
}
