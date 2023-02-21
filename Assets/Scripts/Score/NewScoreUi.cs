using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class NewScoreUi : MonoBehaviour
{
    public GameObject scorePrefab;

    public RowUi rowUi;
    public NewScoreManager scoreManager;


    private void Start()
    {
        scorePrefab = GameObject.FindGameObjectWithTag("ScoreTable");

        foreach (var item in scorePrefab.GetComponent<ScoreHolder>().tableRecords)
        {
            Debug.Log(item.name + " " + item.score);
        }

        foreach (var item in scorePrefab.GetComponent<ScoreHolder>().tableRecords)
        {
            scoreManager.AddScore(new NewScore(name: item.name, score: item.score));
        }


        scoreManager.AddScore(new NewScore(name: "Total Score ", score: scorePrefab.GetComponent<ScoreHolder>().TotalScore));

        var scores  = scoreManager.GetScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}
