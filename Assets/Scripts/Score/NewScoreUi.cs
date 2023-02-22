using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            scoreManager.AddScore(new NewScore(name: item.name, score: item.score));
        }

        scoreManager.AddScore(new NewScore(name: "çáåv ", score: scorePrefab.GetComponent<ScoreHolder>().TotalScore));

        var scores  = scoreManager.GetScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }

    private void Update()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            if (child.transform.position.y >= 600 || child.transform.position.y <= 200)
            {
                child.GetComponent<CanvasGroup>().alpha = 0;
            }
            else
            {
                child.GetComponent<CanvasGroup>().alpha = 1;
            }

            if (gameObject.transform.GetChild(transform.childCount-1).transform.position.y <= 200)
            {
                child.transform.Translate(Vector3.up * Time.deltaTime * 80, Space.Self);
            }
            
        }
    }
}
